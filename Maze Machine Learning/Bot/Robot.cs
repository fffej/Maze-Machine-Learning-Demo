using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Maze_Machine_Learning.ABUtil;
using Maze_Machine_Learning.Maze;
using Maze_Machine_Learning.Neural;

namespace Maze_Machine_Learning.Bot
{
    public class Robot : PhysicalCircle
    {
        public double Energy { get; set; }
        public double MaxEnergy { get; set; }
        public bool Alive { get; set; }
        public bool Goal { get; set; }

        public NNet Brain;
        public Eye[] Eyes;

        public Robot(int numEyes, double maxEnergy, double radius, double sightRange, double fieldOfView)
            :base(new Vector(), 1, radius)
        {
            MaxEnergy = maxEnergy;
            Eyes = new Eye[numEyes];

            for (var i = 0; i < numEyes; ++i)
            {
                Eyes[i] = new Eye {Offset = (-0.5 + i/(double) numEyes)*fieldOfView/2.0};
                Eyes[i].Ray.Angle.Magnitude = sightRange;
            }
        }

        public void Init(Vector position)
        {
            Alive = true;
            Goal = false;
            Energy = MaxEnergy;
            MoveTo(position);
            TurnTo(0);
            Stop();
        }

        public void MoveTo(Vector pos)
        {
            Position = pos;
            foreach (var eye in Eyes) eye.MoveTo(pos);
        }

        public void TurnTo(double theta)
        {
            Velocity.Theta = theta;
            foreach (var eye in Eyes) eye.TurnTo(theta);
        }

        public void Turn(double theta)
        {
            Energy -= theta;
            Velocity.Theta += theta;
            foreach (var eye in Eyes) eye.Turn(theta);
        }

        public void Forward(double force)
        {
            Energy -= force;
            Force += new Angle(Velocity.Theta, force);
        }

        public Coordinate MazeCoordinates(ref PaintMaze maze)
        {
            return (Position / maze.CellSize).Coord;
        }

        public void Update(double time)
        {
            if (Energy <= 0) Alive = false;
            if (!Alive) return;

            //do some thinking
            //do some forcing

            Accelerate(time);

            foreach (var eye in Eyes) eye.MoveTo(Position);
        }

        public void Draw(PaintEventArgs e)
        {
            foreach (var eye in Eyes) eye.Draw(e);
            e.Graphics.DrawEllipse(Pens.CornflowerBlue, (float)(Position.X - Radius), (float)(Position.Y - Radius), (float)(Position.X + Radius), (float)(Position.Y + Radius));
        }
    }

    /*class Robot_Old
    {
        public Robot(ref GridMaze m)
        {
            this.m = m;
            num_eyes = Program.settings.bot_eyes;

            int neurons = num_eyes +
                (Program.settings.think_distance ? 1 : 0) +
                (Program.settings.think_path ? 5 : 0) +
                (Program.settings.think_velocity ? 3 : 0);

            brain = new Brain(neurons, neurons, 2);
            eyes = new Eye[num_eyes];
            velocity = new Ray(new CoordD(0.5, 0.5), 0.0, Program.settings.bot_speed);

            for (int i = 0; i < num_eyes; ++i)
            {
                double theta = (double)i / (double)num_eyes * Consts.η2 - Consts.η;
                eyes[i] = new Eye(velocity.O, 2, 0, theta, 100);
            }

            init(new CoordD(0, 0));
        }

        /// <summary>
        /// Initialise in a given position.
        /// </summary>
        /// <param name="pos">The position to move to.</param>
        public void init(CoordD pos)
        {
            max_energy = Program.settings.maze_time;
            num_eyes = Program.settings.bot_eyes;
            radius = Program.settings.bot_radius;

            alive = true;
            goal = false;
            energy = max_energy;
            grid_pos = new CoordI(pos / m.cell_size);

            move(pos);
            turn_to(0.0);
        }

        /// <summary>
        /// Act on given brain outputs.
        /// </summary>
        /// <param name="force">The force to move forwards with.</param>
        /// <param name="theta">The degree to turn by.</param>
        public void act(float force, float theta)
        {
            if (!alive)
                return;
            else if (grid_pos == m.goal)
            {
                alive = false;
                goal = true;
                return;
            }

            turn((theta * 2 - 1) / Consts.η);
            forward(force);
            speed = force * velocity.v * velocity.r;
            this.energy = Math.Max(0.0, energy - 1);

            if (energy == 0.0) alive = false;
        }

        /// <summary>
        /// Use the <c>Brain</c> with variable inputs based on <c>Program.settings</c>.
        /// </summary>
        public void think()
        {
            if (!alive) return;

            for (int i = 0; i < num_eyes; ++i)
                brain.set_input(i, (float)eyes[i].ray.v);

            int extra = 0;

            if (Program.settings.think_distance)
            {
                brain.set_input(num_eyes + extra++, (float)(((m.goal * m.cell_size - velocity.O) / ((m.walls.Size + 1) * m.cell_size)).length()));
            }

            if (Program.settings.think_path)
            {
                int this_pos = m.walls.get_path_value(m.start) + 1;

                if (m.walls.in_bounds(grid_pos))
                    this_pos = m.walls.get_path_value(grid_pos);

                brain.set_input(num_eyes + extra++, (float)this_pos / (float)m.walls.get_path_value(m.start));

                int dir = 0;
                if (velocity.north_quad()) dir = 1;
                else if (velocity.east_quad()) dir = 2;
                else if (velocity.south_quad()) dir = 3;

                for (int i = 0; i < 4; ++i)
                {
                    int index = (i + dir) % 4;
                    if (m.walls.test_wall(grid_pos, index) || this_pos < m.walls.get_path_value(grid_pos + WallGridMaze.Order[index]))
                        brain.set_input(num_eyes + extra++, 0);
                    else
                        brain.set_input(num_eyes + extra++, 1);
                }
            }

            if (Program.settings.think_velocity)
            {
                CoordD magnitude = new CoordD(0, 0).by_angle(velocity.θ, 1.0);

                brain.set_input(num_eyes + extra++, (float)magnitude.x);
                brain.set_input(num_eyes + extra++, (float)magnitude.y);
                brain.set_input(num_eyes + extra++, (float)speed);
            }

            brain.think();

            act(brain.get_output(0), brain.get_output(1));
        }

        public void move(int x, int y)
        {
            velocity.O.x = x;
            velocity.O.y = y;
            grid_pos = new CoordI(velocity.O / m.cell_size);

            for (int i = 0; i < num_eyes; ++i)
                eyes[i].move(velocity.O);
        }

        public void move(CoordD pos)
        {
            velocity.O.x = pos.x;
            velocity.O.y = pos.y;
            grid_pos = new CoordI(velocity.O / m.cell_size);

            for (int i = 0; i < num_eyes; ++i)
                eyes[i].move(velocity.O);
        }

        public void move(CoordI pos)
        {
            velocity.O.x = pos.x;
            velocity.O.y = pos.y;
            grid_pos = new CoordI(velocity.O / m.cell_size);

            for (int i = 0; i < num_eyes; ++i)
                eyes[i].move(velocity.O);
        }

        /// <summary>
        /// Move forward based on a raycast to detect collisions.
        /// </summary>
        /// <param name="s">The speed to move at.</param>
        public void forward(double s)
        {
            m.raycast(ref velocity, radius);
            CoordD to = velocity.O + ((velocity.cast_point() - velocity.O) * s);
            move(to);
        }

        public void turn_to(double theta)
        {
            for (int i = 0; i < eyes.Length; ++i)
                eyes[i].set_angle(theta);

           velocity.set_angle(theta);
        }

        public void turn(double theta)
        {
            for (int i = 0; i < eyes.Length; ++i)
                eyes[i].turn(theta);

            velocity.set_angle(theta + velocity.θ);
        }

        public void update_eyes(Maze m)
        {
            for (int i = 0; i < eyes.Length; ++i)
                m.raycast(ref eyes[i].ray, 0.0);
        }

        public void draw(PaintEventArgs e)
        {
            if (goal)
                for (int i = 0; i < eyes.Length; ++i)
                    eyes[i].draw(e, Pens.PowderBlue);
            else if (alive)
                for (int i = 0; i < eyes.Length; ++i)
                    eyes[i].draw(e, Pens.GreenYellow);
            else
                for (int i = 0; i < eyes.Length; ++i)
                    eyes[i].draw(e, Pens.Red);

            velocity.draw(e, Pens.Purple, 10.0 * speed);
            e.Graphics.DrawEllipse(Pens.CornflowerBlue, (float)velocity.O.x - 6.0f, (float)velocity.O.y - 6.0f, 12.0f, 12.0f);
        }
    }*/
}

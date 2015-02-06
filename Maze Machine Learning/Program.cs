using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Drawing;

namespace Maze_Machine_Learning
{
    /// <summary>
    /// A <c>Wall_grid</c> based Maze.
    /// </summary>
    class Maze_Old
    {
        public WallGridMaze walls;
        public CoordI start;
        public CoordI goal;
        public CoordD cell_size;
        public Rectangle draw_rect;
        private StringFormat font_format;
        private static Random rand = new Random();
        private static Font font = new Font("Deja vu Sans", 12);

        public Maze(int width, int height, int window_w, int window_h)
        {
            walls = new WallGridMaze(width, height);

            draw_rect = new Rectangle(0, 0, window_w, window_h);
            cell_size = new CoordD((double)window_w / (double)width, (double)window_h / (double)height);

            font_format = new StringFormat();
            font_format.Alignment = StringAlignment.Center;
            font_format.LineAlignment = StringAlignment.Center;

            generate();
        }

        public void scale(int width, int height)
        {
            draw_rect = new Rectangle(0, 0, width, height);
            cell_size = new CoordD((double)width / (double)walls.Size.x, (double)height / (double)walls.Size.y);
        }

        public void resize(int width, int height)
        {
            walls.resize(width, height);
        }

        public void generate()
        {
            goal = new CoordI(rand.Next(0, 2) * (walls.Size.x - 1), rand.Next(0, 2) * (walls.Size.y - 1));
            start = walls.generate(goal);
        }

        public void draw(PaintEventArgs e)
        {
            for (int x = 0; x < walls.Size.x; ++x)
            {
                for (int y = 0; y < walls.Size.y; ++y)
                {
                    for (int d = 0; d < 2; ++d)
                    {
                        if(walls.test_wall(x, y, d))
                        {
                            float x1 = x * (float)cell_size.x;
                            float y1 = y * (float)cell_size.y;

                            e.Graphics.DrawLine(Pens.White, x1, y1,
                                x1 + (d == 0 ? 0 : (float)cell_size.x),
                                y1 + (d == 1 ? 0 : (float)cell_size.y)
                            );
                        }
                    }
                }
            }

            e.Graphics.DrawLine(Pens.White, (float)cell_size.x * (float)walls.Size.x, 0, (float)cell_size.x * (float)walls.Size.x, (float)cell_size.y * (float)walls.Size.y);
            e.Graphics.DrawLine(Pens.White, 0, (float)cell_size.y * (float)walls.Size.y, (float)cell_size.x * (float)walls.Size.x, (float)cell_size.y * (float)walls.Size.y);

            Rectangle r1 = new Rectangle((int)(start.x * cell_size.x), (int)(start.y * cell_size.y), (int)cell_size.x, (int)cell_size.y);
            e.Graphics.DrawString("S", font, Brushes.Red, r1, font_format);

            Rectangle r2 = new Rectangle((int)(goal.x * cell_size.x), (int)(goal.y * cell_size.y), (int)cell_size.x, (int)cell_size.y);
            e.Graphics.DrawString("G", font, Brushes.Green, r2, font_format);
        }

        /// <summary>
        /// Raycasts a given <c>Ray</c> in this maze.
        /// </summary>
        /// <param name="ray">The <c>Ray</c> to cast.</param>
        /// <param name="radius">The radius of the thing casting the ray.</param>
        public void raycast(ref Ray ray, double radius)
        {
            if (ray.r == 0)
            {
                ray.v = 0;
                return;
            }

            CoordI grid_coords = new CoordI(ray.O / cell_size);
            int dir = -1;
            double len = 0;
            double Ax;
            double Ay;

            bool walled = false;
            bool south = ray.going_south();
            bool east = ray.going_east();

            if ((south && !east) || (!south && east))
            {
                Ax = 1.0 / Math.Cos(Consts.η - ray.θ % Consts.η);
                Ay = 1.0 / Math.Cos(ray.θ % Consts.η);
            }
            else
            {
                Ax = 1.0 / Math.Cos(ray.θ % Consts.η);
                Ay = 1.0 / Math.Cos(Consts.η - ray.θ % Consts.η);
            }

            CoordD test_coord = ray.O - grid_coords * cell_size;
            CoordD test_space = cell_size - test_coord;

                 if (walls.test_wall(grid_coords, 2) && test_space.x < radius)
                ray.O.x -= radius - test_space.x;
            else if (walls.test_wall(grid_coords, 3) && test_space.y < radius)
                ray.O.y -= radius - test_space.y;
            else if (walls.test_wall(grid_coords, 0) && test_coord.x < radius)
                ray.O.x += radius - test_coord.x;
            else if (walls.test_wall(grid_coords, 1) && test_coord.y < radius)
                ray.O.y += radius - test_coord.y;

            while (len < ray.r && !walled)
            {
                CoordD grid_space = ray.O.by_angle(ray.θ, len) - grid_coords * cell_size;
                CoordD cell_space = cell_size - grid_space;
                grid_space.abs();
                cell_space.abs();

                double hx = (east ? cell_space.x : grid_space.x) * Ax;
                double hy = (south ? cell_space.y : grid_space.y) * Ay;

                if (hx < hy)
                    len += (east ? Math.Max(0, cell_space.x - radius) : Math.Max(0, grid_space.x - radius)) * Ax;
                else
                    len += (south ? Math.Max(0, cell_space.y - radius) : Math.Max(0, grid_space.y - radius)) * Ay;

                dir = hx < hy ? 0 : 1;
                if ((hx < hy && east) || (hx >= hy && south)) dir += 2;

                walled = !walls.in_bounds(grid_coords, dir) || walls.test_wall(grid_coords, dir);

                if (!walled)
                {
                    if (dir == 0) grid_coords.x -= 1;
                    else if (dir == 1) grid_coords.y -= 1;
                    else if (dir == 2) grid_coords.x += 1;
                    else if (dir == 3) grid_coords.y += 1;
                }
            }

            ray.v = Math.Min(len, ray.r) / ray.r;
        }
    }

    /// <summary>
    /// An offset <c>Ray</c> and radius to be used as an eye in a maze.
    /// </summary>
    class Eye
    {
        public Ray ray;
        public double offset;
        public double radius;

        public Eye(CoordD origin, double radius, double theta, double offset, double magnitude)
        {
            this.radius = radius;
            ray = new Ray(origin.by_angle(theta + offset, radius), theta, magnitude);

            this.offset = offset;
            while (this.offset < 0) this.offset += Consts.τ;
            while (this.offset >= Consts.τ) this.offset -= Consts.τ;
        }

        public void draw(PaintEventArgs e)
        {
            ray.draw(e);
        }

        public void draw(PaintEventArgs e, Pen p)
        {
            ray.draw(e, p);
        }

        public void set_angle(double theta)
        {
            ray.set_angle(theta + offset);
        }

        public void turn(double theta)
        {
            ray.set_angle(ray.θ + theta);
        }

        public void move(CoordD pos)
        {
            ray.O = pos.by_angle(ray.θ, radius);
        }
    }

    /// <summary>
    /// A feed forward neural network based AI.
    /// </summary>
    /// <field name="weights">An array of synapse weights between neurons.</field>
    /// <field name="layers">The working layers used to calculate the output.</field>
    /// <field name="inputs_outputs">The inputs and outputs of the network.</field>
    /// <field name="net_size">The size of each layer.</field>
    /// <field name="largest_layer">The size of the largest layer.</field>
    class Brain
    {
        private float[,] weights;
        private float[,] layers;
        private float[,] inputs_outputs;

        private int[] net_size;
        private int largest_layer;

        private static Random rand = new Random();

        public Brain(int[] network_params)
        {
            this.net_size = network_params;
            build();
        }

        public Brain(int num_inputs, int num_outputs)
        {
            this.net_size = new int[] { num_inputs, num_outputs };
            build();
        }

        public Brain(int num_inputs, int layer_1, int num_outputs)
        {
            this.net_size = new int[] { num_inputs, layer_1, num_outputs };
            build();
        }

        public Brain(int num_inputs, int layer_1, int layer_2, int num_outputs)
        {
            this.net_size = new int[] { num_inputs, layer_1, layer_2, num_outputs };
            build();
        }

        public float get_weight(int i, int j)
        {
            return weights[i, j];
        }

        public float generate_weight()
        {
            return (float)(rand.NextDouble() * 2.0 - 1.0);
        }

        public void build()
        {
            largest_layer = net_size.Max();

            weights = new float[net_size.Length, largest_layer * largest_layer];
            layers = new float[2, largest_layer];
            inputs_outputs = new float[2, largest_layer];

            for (int i = 0; i < net_size.Length; ++i)
                for (int j = 0; j < largest_layer * largest_layer; ++j)
                    weights[i, j] = generate_weight();
        }

        /// <summary>
        /// Breeds two parent brains creating this as the new child.
        /// </summary>
        /// <param name="parent_a">A parent brain.</param>
        /// <param name="parent_b">A parent brain.</param>
        /// <param name="mutation_rate">The rate of weights that are randomized instead of inherited.</param>
        public void breed(Brain parent_a, Brain parent_b, double mutation_rate)
        {
            for (int i = 0; i < net_size.Length; ++i)
            {
                for (int j = 0; j < largest_layer * largest_layer; ++j)
                {
                    if (rand.NextDouble() <= mutation_rate)
                    {
                        weights[i, j] = generate_weight();
                    }
                    else if (rand.NextDouble() >= 0.5)
                    {
                        weights[i, j] = parent_a.get_weight(i, j);
                    }
                    else
                    {
                        weights[i, j] = parent_b.get_weight(i, j);
                    }
                }
            }
        }

        public float get_input(int index)
        {
            return inputs_outputs[0, index];
        }

        public float get_output(int index)
        {
            return inputs_outputs[1, index];
        }

        public void set_input(int index, float input)
        {
            inputs_outputs[0, index] = input;
        }

        public void set_inputs(float[] inputs)
        {
            for (int i = 0; i < inputs.Length; ++i)
            {
                inputs_outputs[0, i] = inputs[i];
            }
        }

        public float sigmoid(float a)
        {
            // This is a Log Sigmoid.
            return (float)(1.0 / (1.0 + Math.Exp(-a * 4.0)));
        }

        /// <summary>
        /// Use the two working layers to calculate the outputs of the network from given inputs.
        /// The working layer is switched after each network layer to conserve memory, and improve performance.
        /// </summary>
        public void think()
        {
            for (int n = 0; n < net_size[0]; ++n)
                layers[0, n] = inputs_outputs[0, n];

            for (int layer = 1; layer < net_size.Length; ++layer)
            {
                for (int neuron = 0; neuron < net_size[layer]; ++neuron)
                {
                    layers[layer % 2, neuron] = 0;

                    for (int synapse = 0; synapse < net_size[layer - 1]; ++synapse)
                        layers[layer % 2, neuron] += layers[1 - (layer % 2), synapse] * weights[layer - 1, (neuron + 1) * synapse];

                    layers[layer % 2, neuron] = sigmoid(layers[layer % 2, neuron]);
                }
            }

            for (int n = 0; n < net_size[net_size.Length - 1]; ++n)
                inputs_outputs[1, n] = layers[(net_size.Length - 1) % 2, n];
        }
    }

    /// <summary>
    /// A robot with <c>Brain</c> and <c>Eyes</c> for navigating a <c>Maze</c>.
    /// </summary>
    class Bot
    {
        public Ray velocity;
        public CoordI grid_pos;
        public double energy;
        public bool alive;
        public bool goal;

        private double radius;
        private double speed;
        private double max_energy;
        private double energy_step;
        public Brain brain;
        private Eye[] eyes;
        private int num_eyes;

        private Maze m;

        public Bot(ref Maze m)
        {
            this.m = m;
            num_eyes = Program.settings.bot_eyes;
            energy_step = 1.0;

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
            this.energy = Math.Max(0.0, energy - energy_step);

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
    }

    /// <summary>
    /// A controller for a <c>Maze</c> and a population of <c>Bot</c>s that attempt to path find.
    /// This "Genetic Algorithm" uses concepts of artificial selection and evolution to train the AIs.
    /// </summary>
    class GeneticAlg
    {
        private Bot[] pool;
        private int[] scores;
        private int[] order;
        public Maze maze;
        public int generation;

        public GeneticAlg(int win_w, int win_h)
        {
            generation = 0;
            maze = new Maze(Program.settings.maze_width, Program.settings.maze_height, win_w, win_h);
            pool = new Bot[Program.settings.bot_number];
            scores = new int[Program.settings.bot_number];
            order = new int[Program.settings.bot_number];

            for (int i = 0; i < Program.settings.bot_number; ++i)
                pool[i] = new Bot(ref maze);
        }

        /// <summary>
        /// Move on to the next generation.
        /// Scores the bots and then kills the worst third of the population,
        /// breeding from the upper two thirds to refill the population.
        /// Regenerates the maze and moves the bots to the start.
        /// </summary>
        public void new_generation()
        {
            for (int i = 0; i < pool.Length; ++i)
            {
                scores[i] = 0;

                if (Program.settings.learn_distance && maze.walls.in_bounds(pool[i].grid_pos))
                    scores[i] += maze.walls.get_path_value(pool[i].grid_pos);
                else if (Program.settings.learn_distance)
                    scores[i] += 1000;

                scores[i] -= (int)pool[i].energy;
                order[i] = i;
            }

            Array.Sort(scores, order);
            int sample = (pool.Length - 1) / 3;

            for (int i = 0; i < sample; ++i)
                pool[order[pool.Length - 1 - i]].brain.breed(pool[order[i * 2]].brain, pool[order[i * 2 + 1]].brain, 0.1);

            ++generation;
            maze.generate();

            for (int i = 0; i < pool.Length; ++i)
                pool[i].init(maze.start * maze.cell_size + (maze.cell_size / 2.0));
        }

        public void update(PaintEventArgs e)
        {
            maze.draw(e);

            if (pool.Length == 0) return;

            bool done = true;

            for (int i = 0; i < pool.Length; ++i)
            {
                done &= !pool[i].alive; // check if all dead
                pool[i].update_eyes(maze);
                pool[i].think();
                pool[i].update_eyes(maze);
                pool[i].draw(e);
            }

            if (done) new_generation();
        }
    }

    class Eye_Tester
    {
        public CoordD position;
        public Eye[] eyes;
        public int num_eyes;
        public bool visible;
        public double radius;

        public Eye_Tester(int number_of_eyes, double magnitude)
        {
            radius = 0.0;
            visible = false;
            num_eyes = number_of_eyes;
            eyes = new Eye[num_eyes];
            for (int i = 0; i < num_eyes; ++i)
            {
                eyes[i] = new Eye(position, 0.0, (double)i / (double)num_eyes * Consts.τ, 0.0, magnitude);
            }
        }

        public void adjust(double magnitude)
        {
            for (int i = 0; i < eyes.Length; ++i)
                eyes[i].ray.r = Math.Max(0.1, eyes[i].ray.r + magnitude);
        }

        public void move(int x, int y)
        {
            position.x = x;
            position.y = y;

            for (int i = 0; i < eyes.Length; ++i)
                eyes[i].ray.O.copy(position);
        }

        public void update_eyes(Maze m)
        {
            if (!visible) return;
            for (int i = 0; i < eyes.Length; ++i)
                m.raycast(ref eyes[i].ray, radius);
        }

        public void draw(PaintEventArgs e)
        {
            if (!visible) return;
            for (int i = 0; i < eyes.Length; ++i)
                eyes[i].draw(e);
        }
    }

    static class Program
    {
        public static Eye_Tester mouse_tracker;
        public static GeneticAlg genalg;
        public static Settings settings;
        public static Menu menu_form;
        public static Maze_Form maze_form;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            menu_form = new Menu();
            Application.Run(menu_form);
        }
    }
}

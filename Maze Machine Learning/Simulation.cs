using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Maze_Machine_Learning.ABUtil;
using Maze_Machine_Learning.Bot;
using Maze_Machine_Learning.Maze;
using Maze_Machine_Learning.Neural;

namespace Maze_Machine_Learning
{
    public class Simulation
    {
        public PaintMaze Maze;
        public Genetics Genetics;

        public double UpdateTimeStop;
        public double UpdatePeriod;
        private Stopwatch Timer;

        public Simulation()
        {
            Maze = new PaintMaze(new Coordinate(Program.settings.maze_width, Program.settings.maze_height),
                                 new Coordinate(Program.settings.win_width, Program.settings.win_height));

            Genetics = new Genetics(Program.settings.bot_number, Program.settings.bot_eyes, Program.settings.maze_time, Program.settings.bot_radius, 100, 0.485 * ExtraMath.Tau);

            Timer.Start();
            UpdateTimeStop = Timer.ElapsedMilliseconds;
        }

        public void Update()
        {
            var timeStop = Timer.ElapsedMilliseconds;
            UpdatePeriod = UpdateTimeStop - timeStop;
            UpdateTimeStop = timeStop;

            foreach (var r in Genetics.Pool.Where(r => r.Alive))
            {
                if ((r.Position/Maze.CellSize).Coord == Maze.Goal)
                {
                    r.Goal = true;
                    r.Alive = false;
                    continue;
                }

                r.Update(UpdatePeriod);
                foreach (var e in r.Eyes) RaySolver.CastRayInMaze(ref e.Ray, Maze);
            }
        }

        public void Draw(PaintEventArgs e)
        {
            Maze.Draw(e);
            foreach (var r in Genetics.Pool) r.Draw(e);
        }

        public void NextCycle()
        {
            Genetics.NextGeneration(Maze, Program.settings.learn_distance);
            Maze.GenerateRandom();

            foreach (var r in Genetics.Pool) r.Position = Maze.Start + Maze.CellSize / 2.0;
        }
    }
}

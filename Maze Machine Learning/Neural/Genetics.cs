using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maze_Machine_Learning.ABUtil;
using Maze_Machine_Learning.Bot;
using Maze_Machine_Learning.Maze;

namespace Maze_Machine_Learning.Neural
{
    public class Genetics
    {
        public int[] Scores;
        public int[] Order;
        public Robot[] Pool;
        public int Generation;

        public Genetics(int poolSize, int numBotEyes, double maxEnergy, double botRadius, double visionRange, double FoV)
        {
            Generation = 0;
            Scores = new int[poolSize];
            Order = new int[poolSize];
            Pool = new Robot[poolSize];

            for (var i = 0; i < Program.settings.bot_number; ++i)
                Pool[i] = new Robot(numBotEyes, maxEnergy, botRadius, visionRange, FoV);
        }

        public void NextGeneration(PaintMaze maze, bool learnDistance)
        {
            for (var i = 0; i < Pool.Length; ++i)
            {
                Order[i] = i;
                Scores[i] = (int)(Pool[i].MaxEnergy - Pool[i].Energy);

                if (!learnDistance) continue;

                var pos = Pool[i].MazeCoordinates(ref maze);
                if (maze.InBounds(pos))
                    Scores[i] += maze[pos];
            }

            Array.Sort(Scores, Order);

            var sample = Pool.Length/3;

            for (var i = 0; i < sample; ++i)
                Pool[Order[Pool.Length - 1 - i]].Brain.breed(Pool[Order[i * 2]].Brain, Pool[Order[i * 2 + 1]].Brain, 0.1);

            ++Generation;
        }
    }
}

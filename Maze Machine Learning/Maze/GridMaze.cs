using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using Maze_Machine_Learning.ABUtil;

namespace Maze_Machine_Learning.Maze
{
    public class GridMaze : Cells
    {
        public Coordinate Start;
        public Coordinate Goal;

        private static readonly Random Rand = new Random();

        public static readonly WallP[] AllWalls =
        {
            new WallP(new Coordinate(-1, 0), Direction.West),
            new WallP(new Coordinate(0, -1), Direction.North),
            new WallP(new Coordinate(+1, 0), Direction.East),
            new WallP(new Coordinate(0, +1), Direction.South)
        };

        public GridMaze(Coordinate size)
        {
            Size = size;
        }

        public void Resize(Coordinate size)
        {
            Size = size;
        }

        public IEnumerable<WallP> Rows()
        {
            for (var p = new WallP(new Coordinate(0, 0), Direction.None); p.C.Y < Size.Y; ++p.C.Y)
                for (p.C.X = 0; p.C.X < Size.Y; ++p.C.X)
                {
                    p.D = Direction.West;
                    yield return p;
                    p.D = Direction.North;
                    yield return p;
                }
        }

        private Coordinate SuitableStart()
        {
            var largest = 0;
            var c = new Coordinate(0, 0);

            foreach (var p in Rows().Where(p => largest < this[p.C]))
            {
                largest = this[p.C];
                c = p.C;
            }

            return c;
        }

        private void RGenerate(Coordinate c, int f = 1)
        {
            if (this[c] != 0) return;
            this[c] = f;

            foreach (var p in AllWalls.OrderBy(x => Rand.Next()).Select(x => x + c).Where(InBounds))
            {
                this[p] = true;
                if (this[p.C] != 0) continue;
                RGenerate(p.C, f + 1);
                this[p] = false;
            }
        }

        private Coordinate Regnerate()
        {
            Reset();
            RGenerate(Goal);
            Start = SuitableStart();
            return Start;
        }

        public Coordinate GenerateFromGoal(Coordinate goal)
        {
            Goal = goal;
            return Regnerate();
        }

        public void GenerateRandom()
        {
            GenerateFromGoal(new Coordinate(Rand.Next(0, Size.X), Rand.Next(0, Size.Y)));
        }
    }
}
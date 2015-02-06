using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maze_Machine_Learning.ABUtil;

namespace Maze_Machine_Learning.Maze
{
    public struct WallP
    {
        public Coordinate C { get; set; }
        public Direction D { get; set; }

        public WallP(Coordinate c, Direction d)
            : this()
        {
            C = c;
            D = d;
        }

        public static WallP operator +(WallP p, Coordinate c) { return new WallP(p.C + c, p.D); }
        public static WallP operator -(WallP p, Coordinate c) { return new WallP(p.C - c, p.D); }
        public static WallP operator *(WallP p, Coordinate c) { return new WallP(p.C * c, p.D); }
        public static WallP operator /(WallP p, Coordinate c) { return new WallP(p.C / c, p.D); }
    }
}

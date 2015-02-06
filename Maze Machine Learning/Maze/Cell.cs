using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maze_Machine_Learning.ABUtil;

namespace Maze_Machine_Learning.Maze
{
    public struct Cell
    {
        public bool TopWall { get; set; }
        public bool LeftWall { get; set; }
        public int FloodValue { get; set; }

        public bool this[Direction index]
        {
            get
            {
                switch (index)
                {
                    case Direction.West:
                        return LeftWall;
                    case Direction.North:
                        return TopWall;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
            set
            {
                switch (index)
                {
                    case Direction.West:
                        LeftWall = value;
                        break;
                    case Direction.North:
                        TopWall = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }
    }

    public class Cells
    {
        public Cell[,] Grid { get; set; }
        public Coordinate Size { get; set; }

        public void Reset()
        {
            Grid = new Cell[Size.X, Size.Y];
        }

        public bool InBounds(Coordinate c)
        {
            return c.X >= 0 && c.Y >= 0 && c.X < Size.X && c.Y < Size.Y;
        }

        public bool InBounds(Direction d)
        {
            return d == Direction.West || d == Direction.North || d == Direction.East || d == Direction.South;
        }

        public bool InBounds(WallP p)
        {
            return InBounds(p.C) && InBounds(p.D) &&
                !(p.C.X == Size.X - 1 && p.D == Direction.East) &&
                !(p.C.Y == Size.Y - 1 && p.D == Direction.South);
        }

        public int this[Coordinate index]
        {
            get
            {
                return Grid[index.X, index.Y].FloodValue;
            }
            set
            {
                Grid[index.X, index.Y].FloodValue = value;
            }
        }

        public bool this[WallP index]
        {
            get
            {
                switch (index.D)
                {
                    case Direction.East:
                        return Grid[index.C.X + 1, index.C.Y][Direction.West];
                    case Direction.South:
                        return Grid[index.C.X, index.C.Y + 1][Direction.North];
                    default:
                        return Grid[index.C.X, index.C.Y][index.D];
                }
            }
            set
            {
                switch (index.D)
                {
                    case Direction.East:
                        Grid[index.C.X + 1, index.C.Y][Direction.West] = value;
                        break;
                    case Direction.South:
                        Grid[index.C.X, index.C.Y + 1][Direction.North] = value;
                        break;
                    default:
                        Grid[index.C.X, index.C.Y][index.D] = value;
                        break;
                }
            }
        }
    }
}

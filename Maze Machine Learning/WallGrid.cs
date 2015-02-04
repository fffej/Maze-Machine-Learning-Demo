using System;
using System.Collections.Generic;
using System.Linq;
using ABUtil;

namespace Maze_Machine_Learning
{
    /// <summary>
    /// A Maze generator and storage.
    /// </summary>
    /// <field name="walls">An array of true/false representing walls on/off.</field>
    /// <field name="flood">Flood fill from the goal of the maze for path finding.</field>
    /// <field name="order">The order of cardinal directions.</field>
    public struct WallGrid
    {
        public CoordI size;
        private bool[] walls;
        private int[,] flood;

        private static Random rand = new Random();

        public static CoordI[] order = new CoordI[] {
            new CoordI(-1, +0), // west
            new CoordI(+0, -1), // north
            new CoordI(+1, +0), // east
            new CoordI(+0, +1)  // south
        };

        public WallGrid(int x, int y)
        {
            size = new CoordI(x, y);
            walls = null;
            flood = null;
        }

        /// <summary>
        /// Gets the size of the walls array, not the size of the maze.
        /// </summary>
        /// <returns>Size of walls array.</returns>
        /// <seealso cref="size"/>
        public int get_size()
        {
            return walls.Length;
        }

        public void resize(int x, int y)
        {
            size.x = x;
            size.y = y;
        }

        public int get_path_value(int x, int y)
        {
            return flood[x, y];
        }

        public int get_path_value(CoordI c)
        {
            return flood[c.x, c.y];
        }

        /// <summary>
        /// Obtains the one dimensional index from a CoordI for a cell and direction for a wall.
        /// </summary>
        /// <param name="c">The coordinates of the cell.</param>
        /// <param name="dir">The direction of the wall.</param>
        /// <returns>The corresponding index of the walls array.</returns>
        public int get_wall(CoordI c, int dir)
        {
            if (dir < 3)
            {
                return 2 * (c.x + c.y * size.x) + dir; // west, north, east
            }
            return 2 * (c.x + (c.y + 1) * size.x) + 1; // south
        }

        /// <summary>
        /// Obtains the one dimensional index from coordinates for a cell and direction for a wall.
        /// </summary>
        /// <param name="x">The x coordinate of the cell.</param>
        /// <param name="y">The y coordinate of the cell.</param>
        /// <param name="dir">The direction of the wall.</param>
        /// <returns>The corresponding index of the walls array.</returns>
        public int get_wall(int x, int y, int dir)
        {
            if (dir < 3)
            {
                return 2 * (x + y * size.x) + dir; // west, north, east
            }
            return 2 * (x + (y + 1) * size.x) + 1; // south
        }

        public bool in_bounds(int dir)
        {
            return dir >= 0 && dir < 4;
        }

        public bool in_bounds(CoordI c)
        {
            return c.x >= 0 && c.x < size.x && c.y >= 0 && c.y < size.y;
        }

        public bool in_bounds(int x, int y)
        {
            return x >= 0 && x < size.x && y >= 0 && y < size.y;
        }

        public bool in_bounds(CoordI c, int dir)
        {
            return in_bounds(c) && in_bounds(dir) &&
                   !(dir == 2 && c.x == size.x - 1) &&
                   !(dir == 3 && c.y == size.y - 1);
        }

        public bool in_bounds(int x, int y, int dir)
        {
            return in_bounds(x, y) && in_bounds(dir) &&
                   !(dir == 2 && x == size.x - 1) &&
                   !(dir == 3 && y == size.y - 1);
        }

        /// <summary>
        /// Tests if a cell and wall are out of bounds, or if it is set.
        /// </summary>
        /// <param name="c">The coordinates of the cell.</param>
        /// <param name="dir">The direction of the wall to test.</param>
        /// <returns>True if the wall is out of bounds, or if the wall is set.</returns>
        public bool test_wall(CoordI c, int dir)
        {
            return !in_bounds(c, dir) || walls[get_wall(c, dir)];
        }

        /// <summary>
        /// Tests if a cell and wall are out of bounds, or if it is set.
        /// </summary>
        /// <param name="x">The x coordinate of the cell.</param>
        /// <param name="y">The y coordinate of the cell.</param>
        /// <param name="dir">The direction of the wall to test.</param>
        /// <returns>True if the wall is out of bounds, or if the wall is set.</returns>
        public bool test_wall(int x, int y, int dir)
        {
            return !in_bounds(x, y, dir) || walls[get_wall(x, y, dir)];
        }

        /// <summary>
        /// Recursive maze generation based on the "Growing tree" algorithm.
        /// Should be invoked with the goal coordinates of the maze, and no value for f.
        /// </summary>
        /// <param name="c">Coordinates generate from.</param>
        /// <param name="f">Flood fill value for this cell.</param>
        private void r_generate(CoordI c, int f = 1)
        {
            if (flood[c.x, c.y] == 0)
            {
                flood[c.x, c.y] = f;

                walls[get_wall(c, 0)] = true;
                walls[get_wall(c, 1)] = true;
                if (c.x < size.x - 1) walls[get_wall(c, 2)] = true;
                if (c.y < size.y - 1) walls[get_wall(c, 3)] = true;

                Stack<int> items = new Stack<int>(new int[] { 0, 1, 2, 3 }.OrderBy(n => rand.Next()).ToArray());

                while (items.Count > 0)
                {
                    int active = items.Pop();
                    CoordI new_c = c + order[active];

                    if (in_bounds(new_c) && flood[new_c.x, new_c.y] == 0)
                    {
                        r_generate(new_c, f + 1);
                        walls[get_wall(c, active)] = false;
                    }
                }
            }
        }

        /// <summary>
        /// Finds a point in the maze that is as far or further than any other point from the goal.
        /// </summary>
        /// <returns>The furthest point in the maze from the goal.</returns>
        public CoordI furthest_from_goal()
        {
            int largest = 0;
            CoordI coord = new CoordI(0, 0);

            for (int x = 0; x < size.x; ++x)
            {
                for (int y = 0; y < size.y; ++y)
                {
                    if (largest < flood[x, y])
                    {
                        largest = flood[x, y];
                        coord.x = x;
                        coord.y = y;
                    }
                }
            }

            return coord;
        }

        /// <summary>
        /// Generates a new maze.
        /// </summary>
        /// <param name="goal">The intended goal of the maze.</param>
        /// <returns>The intended start of the maze.</returns>
        public CoordI generate(CoordI goal)
        {
            walls = new bool[size.x * size.y * 2];
            flood = new int[size.x, size.y];
            r_generate(goal);
            return furthest_from_goal();
        }
    }
}
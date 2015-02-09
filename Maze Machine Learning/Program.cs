using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Drawing;

namespace Maze_Machine_Learning
{
/*        public void raycast(ref Ray ray, double radius)
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
    }*/



    /// <summary>
    /// A robot with <c>Brain</c> and <c>Eyes</c> for navigating a <c>Maze</c>.
    /// </summary>
    


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

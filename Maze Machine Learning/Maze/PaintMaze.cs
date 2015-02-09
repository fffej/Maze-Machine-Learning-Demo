using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Maze_Machine_Learning.ABUtil;

namespace Maze_Machine_Learning.Maze
{
    public class PaintMaze : GridMaze
    {
        public Vector CellSize;
        public Rectangle DrawBox;
        private readonly StringFormat FontFormat;
        private static readonly Font SansFont = new Font("Deja vu Sans", 12);

        public PaintMaze(Coordinate dimensions, Coordinate window)
            : base(dimensions)
        {
            FontFormat = new StringFormat {Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center};
            Scale(window);
        }

        public void Scale(Coordinate window)
        {
            DrawBox = new Rectangle(0, 0, window.X, window.Y);
            CellSize = new Vector((double)window.X / Size.X, (double)window.Y / Size.Y);
        }

        public void Draw(PaintEventArgs e)
        {
            foreach (var p in Rows().Where(x => this[x]))
            {
                e.Graphics.DrawLine(Pens.White, p.C.X * (float)CellSize.X, p.C.Y * (float)CellSize.Y,
                    p.C.X * (float)CellSize.X + (p.D == Direction.West ? 0 : (float)CellSize.X),
                    p.C.Y * (float)CellSize.Y + (p.D == Direction.North ? 0 : (float)CellSize.Y)
                );
            }

            e.Graphics.DrawLine(Pens.White, (float)CellSize.X * Size.X, 0, (float)CellSize.X * Size.X, (float)CellSize.Y * Size.Y);
            e.Graphics.DrawLine(Pens.White, 0, (float)CellSize.Y * Size.Y, (float)CellSize.X * Size.X, (float)CellSize.Y * Size.Y);

            var r1 = new Rectangle((int)(Start.X * CellSize.X), (int)(Start.Y * CellSize.Y), (int)CellSize.X, (int)CellSize.Y);
            var r2 = new Rectangle((int)(Goal.X * CellSize.X), (int)(Goal.Y * CellSize.Y), (int)CellSize.X, (int)CellSize.Y);
            e.Graphics.DrawString("S", SansFont, Brushes.Red, r1, FontFormat);
            e.Graphics.DrawString("G", SansFont, Brushes.Green, r2, FontFormat);
        }
    }
}

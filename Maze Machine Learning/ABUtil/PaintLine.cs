using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Maze_Machine_Learning.ABUtil
{
    public class PaintLine : Line
    {
        public PaintLine()
        {
        }

        public PaintLine(Vector origin, Angle angle)
            : base(origin, angle)
        {
        }

        public PaintLine(Vector origin, Vector end)
            : base(origin, end)
        {
        }

        public void Draw(PaintEventArgs e, Pen p)
        {
            e.Graphics.DrawLine(p, Origin.PointF, CastPoint().PointF);
        }
    }
}

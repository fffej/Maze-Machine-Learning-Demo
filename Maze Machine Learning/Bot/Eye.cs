using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Maze_Machine_Learning.ABUtil;

namespace Maze_Machine_Learning.Bot
{
    public class Eye
    {
        public PaintLine Ray;
        public double Offset;

        public Eye()
            : this(new Vector(), new Angle(), 0)
        {
        }

        public Eye(Vector origin, Angle theta, double offset)
        {
            Ray = new PaintLine(origin, theta);
            Ray.Angle.Theta += offset;
            Offset = offset;
        }

        public void Draw(PaintEventArgs e)
        {
            Ray.Draw(e, Pens.GreenYellow);
        }

        public void Draw(PaintEventArgs e, Pen p)
        {
            Ray.Draw(e, p);
        }

        public void TurnTo(double theta)
        {
            Ray.Angle.Theta = theta + Offset;
        }

        public void MoveTo(Vector pos)
        {
            Ray.Origin = pos;
        }

        public void Turn(double theta)
        {
            Ray.Angle.Theta += theta;
        }

        public void Move(Vector pos)
        {
            Ray.Origin += pos;
        }
    }
}

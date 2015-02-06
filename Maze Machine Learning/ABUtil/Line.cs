using System;
using System.Drawing;
using System.Windows.Forms;
using Maze_Machine_Learning;

namespace Maze_Machine_Learning.ABUtil
{
    public class Line
    {
        private double _cast;
        public Vector Origin { get; set; }
        public Angle Theta { get; set; }

        public double Cast
        {
            get { return _cast; }
            set { _cast = Math.Max(0.0, Math.Min(1.0, value)); }
        }

        Line(Vector origin, Angle theta)
        {
            Origin = origin;
            Theta = theta;
            Cast = 1;
        }

        Line(Vector origin, Vector end)
        {
            Origin = origin;
            Theta = new Angle { Vec = end };
            Cast = 1;
        }

        public Vector EndPoint()
        {
            return Origin + Theta;
        }

        public Vector CastPoint()
        {
            return Origin + (Theta * Cast);
        }
    }
}
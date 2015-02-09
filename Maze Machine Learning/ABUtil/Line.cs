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
        public Angle Angle { get; set; }

        public double Cast
        {
            get { return _cast; }
            set { _cast = Math.Max(0.0, Math.Min(1.0, value)); }
        }

        public Line()
            : this(new Vector(0, 0), new Angle(0, 0))
        {
        }

        public Line(Vector origin, Angle theta)
        {
            Origin = origin;
            Angle = theta;
            Cast = 1;
        }

        public Line(Vector origin, Vector end)
        {
            Origin = origin;
            Angle = new Angle { Vec = end };
            Cast = 1;
        }

        public Vector EndPoint()
        {
            return Origin + Angle;
        }

        public Vector CastPoint()
        {
            return Origin + (Angle * Cast);
        }
    }
}
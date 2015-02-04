using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze_Machine_Learning.ABUtil
{
    class Angle
    {
        private double _theta;
        public double Magnitude { get; set; }

        public double Theta
        {
            get { return _theta; }
            set { _theta = value % Constants.Circles.Tau; }
        }

        public double X
        {
            get { return Magnitude * Math.Cos(_theta); }
            set { Theta = Math.Atan(Y / value); Magnitude = Math.Sqrt(value * value + Y * Y); }
        }

        public double Y
        {
            get { return Magnitude * Math.Sin(_theta); }
            set { Theta = Math.Atan(value / X); Magnitude = Math.Sqrt(X * X + value * value); }
        }

        public Vector Vec
        {
            get { return new Vector(X, Y); }
            set { X = value.X; Y = value.Y; }
        }

        public Angle()
            : this(0, 0)
        { }

        public Angle(double theta, double magnitude)
        {
            Theta = theta;
            Magnitude = magnitude;
        }
    }
}

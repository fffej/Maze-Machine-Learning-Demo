using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze_Machine_Learning.ABUtil
{
    public enum Direction { None, West, North, East, South }

    public class Angle
    {
        private static readonly double[] Cardinals = Enumerable.Range(0, 4).Select(x => ExtraMath.Tau / (4 - x)).ToArray();
        private static readonly double[] Quads = Enumerable.Range(0, 3).Select(x => ExtraMath.Tau / (4 - x) - 1.0 / 8.0).ToArray();

        private double _theta;
        public double Magnitude { get; set; }

        public double Theta
        {
            get { return _theta; }
            set { _theta = value % ExtraMath.Tau; }
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
            get { return new Vector {X = X, Y = Y}; }
            set { X = value.X; Y = value.Y; }
        }

        public Angle()
            : this(0, 0)
        {}

        public Angle(double theta, double magnitude)
        {
            Theta = theta;
            Magnitude = magnitude;
        }

        public bool InRange(double a1, double a2)
        {
            return a1 < a2 ? Theta >= a1 && Theta < a2 : Theta >= a1 || Theta < a2;
        }

        public bool InCardinal(Direction dir)
        {
            return InRange(Cardinals[((int)dir - 1) % 4], Cardinals[((int)dir + 1) % 5]);
        }

        public bool InQuad(Direction dir)
        {
            return InRange(Quads[((int)dir - 1) % 4], Quads[(int)dir]);
        }

        public static Angle operator +(Angle a, double b) { return new Angle(a.Theta, a.Magnitude + b); }
        public static Angle operator -(Angle a, double b) { return new Angle(a.Theta, a.Magnitude - b); }
        public static Angle operator *(Angle a, double b) { return new Angle(a.Theta, a.Magnitude * b); }
        public static Angle operator /(Angle a, double b) { return new Angle(a.Theta, a.Magnitude / b); }

        public static bool operator ==(Angle a, Angle b) { return a.Equals(b); }
        public static bool operator ==(Angle a, Vector b) { return a.Equals(b); }
        public static bool operator ==(Vector a, Angle b) { return b.Equals(a); }
        public static bool operator !=(Angle a, Angle b) { return !a.Equals(b); }
        public static bool operator !=(Angle a, Vector b) { return !a.Equals(b); }
        public static bool operator !=(Vector a, Angle b) { return !b.Equals(a); }

        public bool Equals(Angle obj)
        {
            return ExtraMath.Compare(Theta, obj.Theta) && ExtraMath.Compare(Magnitude, obj.Magnitude);
        }

        public bool Equals(Vector obj)
        {
            return ExtraMath.Compare(X, obj.X) && ExtraMath.Compare(Y, obj.Y);
        }

        public override bool Equals(object obj) { return false; }
        public override int GetHashCode() { return (int)Theta ^ (int)Magnitude; }
    }
}

using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

/*
namespace Maze_Machine_Learning.ABUtil
{
    class Coordinate
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Coordinate()
            : this(0, 0)
        {
        }

        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int[] Array
        {
            get { return new int[] { X, Y }; }
            set
            {
                try
                {
                    X = value[0];
                    Y = value[1];
                }
                catch (IndexOutOfRangeException exception)
                {
                    throw new ArgumentException(Constants.Strings.ArrayToSmall2);
                }
            }
        }

        public int this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return X;
                    case 1:
                        return Y;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        X = value;
                        break;
                    case 1:
                        Y = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }

        public Vector Vector
        {
            get { return new Vector(X, Y); }
            set
            {
                X = (int)value.X;
                Y = (int)value.Y;
            }
        }

        public static Coordinate operator +(Coordinate a, Coordinate b) { return new Coordinate(a.X + b.X, a.Y + b.Y); }
        public static Coordinate operator -(Coordinate a, Coordinate b) { return new Coordinate(a.X - b.X, a.Y + b.Y); }
        public static Coordinate operator *(Coordinate a, Coordinate b) { return new Coordinate(a.X * b.X, a.Y + b.Y); }
        public static Coordinate operator /(Coordinate a, Coordinate b) { return new Coordinate(a.X / b.X, a.Y + b.Y); }
    }

    class Vector
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Vector()
            : this(0.0, 0.0)
        {
        }

        public Vector(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double[] Array
        {
            get { return new double[] { X, Y }; }
            set
            {
                try
                {
                    X = value[0];
                    Y = value[1];
                }
                catch (IndexOutOfRangeException exception)
                {
                    throw new ArgumentException(Constants.Strings.ArrayToSmall2);
                }
            }
        }

        public double this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return X;
                    case 1:
                        return Y;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        X = value;
                        break;
                    case 1:
                        Y = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }

        public Coordinate Coordinate
        {
            get { return new Coordinate((int)X, (int)Y); }
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }
    }

    /// <summary>
    ///     Double precision coordinate system.
    /// </summary>
    /// <remarks>
    ///     Contains various operator overloads for CoordI, CoordD, int, and double types.
    ///     Many of the operators relating CoordD to CoordI are in the CoordI struct.
    ///     In cases where integers and doubles are combined via operations, integers are promoted.
    /// </remarks>
    /// <seealso cref="CoordI" />
    struct CoordD
    {
        public double x;
        public double y;

        public CoordD(CoordI other)
        {
            x = other.x;
            y = other.y;
        }

        public CoordD(CoordD other)
        {
            x = other.x;
            y = other.y;
        }

        public CoordD(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public CoordD(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public void copy(CoordI other)
        {
            x = other.x;
            y = other.y;
        }

        public void copy(CoordD other)
        {
            x = other.x;
            y = other.y;
        }

        public static bool operator ==(CoordD c1, CoordD c2)
        {
            return (c1.x == c2.x) && (c1.y == c2.y);
        }

        public static bool operator !=(CoordD c1, CoordD c2)
        {
            return (c1.x != c2.x) || (c1.y != c2.y);
        }

        public static CoordD operator +(CoordD c1, CoordD c2)
        {
            return new CoordD(c1.x + c2.x, c1.y + c2.y);
        }

        public static CoordD operator +(CoordD c1, double s)
        {
            return new CoordD(c1.x + s, c1.y + s);
        }

        public static CoordD operator -(CoordD c1, CoordD c2)
        {
            return new CoordD(c1.x - c2.x, c1.y - c2.y);
        }

        public static CoordD operator -(CoordD c1, double s)
        {
            return new CoordD(c1.x - s, c1.y - s);
        }

        public static CoordD operator *(CoordD c1, CoordD c2)
        {
            return new CoordD(c1.x*c2.x, c1.y*c2.y);
        }

        public static CoordD operator *(CoordD c1, double s)
        {
            return new CoordD(c1.x*s, c1.y*s);
        }

        public static CoordD operator /(CoordD c1, CoordD c2)
        {
            return new CoordD(c1.x/c2.x, c1.y/c2.y);
        }

        public static CoordD operator /(CoordD c1, double s)
        {
            return new CoordD(c1.x/s, c1.y/s);
        }

        /// <summary>
        ///     Calculates the magnitude of a vector from origin (0, 0) to this (x, y).
        /// </summary>
        /// <returns>Length of a vector from origin to this.</returns>
        public double length()
        {
            return Math.Sqrt(x*x + y*y);
        }

        /// <summary>
        ///     Calculates the location of a point <c>magnitude</c> down a line starting at this, in the direction of <c>theta</c>.
        /// </summary>
        /// <param name="theta">The angle to calculate by.</param>
        /// <param name="magnitude">The length to calculate for.</param>
        /// <returns>The CoordD of the calculated point.</returns>
        public CoordD by_angle(double theta, double magnitude)
        {
            return new CoordD(x + magnitude*Math.Cos(theta), y + magnitude*Math.Sin(theta));
        }

        /// <summary>
        ///     Ensures both x and y are positive.
        /// </summary>
        public void abs()
        {
            x = Math.Abs(x);
            y = Math.Abs(y);
        }
    }

    /// <summary>
    ///     Integer coordinate system.
    /// </summary>
    /// <remarks>
    ///     Contains various operator overloads for CoordI, CoordD, int, and double types.
    ///     In cases where doubles are assigned or compared to integers, doubles are truncated.
    ///     In cases where integers and doubles are combined via operations, integers are promoted.
    /// </remarks>
    /// <seealso cref="CoordD" />
    struct CoordI
    {
        public int x;
        public int y;

        public CoordI(CoordI other)
        {
            x = other.x;
            y = other.y;
        }

        public CoordI(CoordD other)
        {
            x = (int) other.x;
            y = (int) other.y;
        }

        public CoordI(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public CoordI(double x, double y)
        {
            this.x = (int) x;
            this.y = (int) y;
        }

        public void copy(CoordI other)
        {
            x = other.x;
            y = other.y;
        }

        public void copy(CoordD other)
        {
            x = (int) other.x;
            y = (int) other.y;
        }

        public static bool operator ==(CoordI c1, CoordI c2)
        {
            return (c1.x == c2.x) && (c1.y == c2.y);
        }

        public static bool operator ==(CoordI c1, CoordD c2)
        {
            return (c1.x == (int) c2.x) && (c1.y == (int) c2.y);
        }

        public static bool operator ==(CoordD c1, CoordI c2)
        {
            return ((int) c1.x == c2.x) && ((int) c1.y == c2.y);
        }

        public static bool operator !=(CoordI c1, CoordI c2)
        {
            return (c1.x != c2.x) || (c1.y != c2.y);
        }

        public static bool operator !=(CoordI c1, CoordD c2)
        {
            return (c1.x != c2.x) || (c1.y != c2.y);
        }

        public static bool operator !=(CoordD c1, CoordI c2)
        {
            return (c1.x != c2.x) || (c1.y != c2.y);
        }

        public static CoordI operator +(CoordI c1, CoordI c2)
        {
            return new CoordI(c1.x + c2.x, c1.y + c2.y);
        }

        public static CoordI operator +(CoordI c1, int a)
        {
            return new CoordI(c1.x + a, c1.y + a);
        }

        public static CoordD operator +(CoordI c1, CoordD c2)
        {
            return new CoordD(c1.x + c2.x, c1.y + c2.y);
        }

        public static CoordD operator +(CoordD c1, CoordI c2)
        {
            return new CoordD(c1.x + c2.x, c1.y + c2.y);
        }

        public static CoordI operator -(CoordI c1, CoordI c2)
        {
            return new CoordI(c1.x - c2.x, c1.y - c2.y);
        }

        public static CoordI operator -(CoordI c1, int a)
        {
            return new CoordI(c1.x - a, c1.y - a);
        }

        public static CoordD operator -(CoordI c1, CoordD c2)
        {
            return new CoordD(c1.x - c2.x, c1.y - c2.y);
        }

        public static CoordD operator -(CoordD c1, CoordI c2)
        {
            return new CoordD(c1.x - c2.x, c1.y - c2.y);
        }

        public static CoordI operator *(CoordI c1, CoordI c2)
        {
            return new CoordI(c1.x*c2.x, c1.y*c2.y);
        }

        public static CoordI operator *(CoordI c1, int a)
        {
            return new CoordI(c1.x*a, c1.y*a);
        }

        public static CoordD operator *(CoordI c1, CoordD c2)
        {
            return new CoordD(c1.x*c2.x, c1.y*c2.y);
        }

        public static CoordD operator *(CoordD c1, CoordI c2)
        {
            return new CoordD(c1.x*c2.x, c1.y*c2.y);
        }

        public static CoordI operator /(CoordI c1, CoordI c2)
        {
            return new CoordI(c1.x/c2.x, c1.y/c2.y);
        }

        public static CoordI operator /(CoordI c1, int a)
        {
            return new CoordI(c1.x/a, c1.y/a);
        }

        public static CoordD operator /(CoordI c1, CoordD c2)
        {
            return new CoordD(c1.x/c2.x, c1.y/c2.y);
        }

        public static CoordD operator /(CoordD c1, CoordI c2)
        {
            return new CoordD(c1.x/c2.x, c1.y/c2.y);
        }
    }
}*/
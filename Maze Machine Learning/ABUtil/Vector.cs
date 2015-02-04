using System;
using System.Drawing;

namespace Maze_Machine_Learning.ABUtil
{
	class Coordinate
	{
		public int X { get; set; }
		public int Y { get; set; }
		
		public Vector Vec
		{
			get { return new Vector(X, Y); }
			set { X = (int)value.X; Y = (int)value.Y; }
		}

		public double Magnitude
		{
			get { return Math.Sqrt(X * X + Y * Y); }
			set { Angle = new Angle {Vec = Vec, Magnitude = value - Magnitude}; }
		}

		public Angle Angle
		{
			get { return new Angle {Vec = Vec}; }
			set { X = (int)value.X; Y = (int)value.Y; }
		}

		public PointF PointF
		{
			get { return new PointF(X, Y); }
			set { X = (int)value.X; Y = (int)value.Y; }
		}

		public int[] Array
		{
			get { return new[] {X, Y}; }
			set
			{
				try
				{
					X = value[0];
					Y = value[1];
				}
				catch (IndexOutOfRangeException)
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
	
		public Coordinate()
			: this(0, 0)
		{}
	
		public Coordinate(int x, int y)
		{
			X = x;
			Y = y;
		}

		public Coordinate(double x, double y)
		{
			X = (int)x;
			Y = (int)y;
		}

		public Coordinate(Coordinate other)
		{
			X = other.X;
			Y = other.Y;
		}

		public Coordinate(Vector other)
		{
			X = (int)other.X;
			Y = (int)other.Y;
		}

		public static Coordinate operator +(Coordinate a, Coordinate b) { return new Coordinate(a.X + b.X, a.Y + b.X); }
		public static Coordinate operator +(Coordinate a, int b) { return new Coordinate(a.X + b, a.Y + b); }
		public static Coordinate operator +(int a, Coordinate b) { return new Coordinate(a + b.X, a + b.Y); }
		public static Coordinate operator +(Coordinate a, double b) { return new Coordinate(a.X + b, a.Y + b); }
		public static Coordinate operator +(double a, Coordinate b) { return new Coordinate(a + b.X, a + b.Y); }

		public static Coordinate operator -(Coordinate a, Coordinate b) { return new Coordinate(a.X - b.X, a.Y - b.X); }
		public static Coordinate operator -(Coordinate a, int b) { return new Coordinate(a.X - b, a.Y - b); }
		public static Coordinate operator -(int a, Coordinate b) { return new Coordinate(a - b.X, a - b.Y); }
		public static Coordinate operator -(Coordinate a, double b) { return new Coordinate(a.X - b, a.Y - b); }
		public static Coordinate operator -(double a, Coordinate b) { return new Coordinate(a - b.X, a - b.Y); }

		public static Coordinate operator *(Coordinate a, Coordinate b) { return new Coordinate(a.X * b.X, a.Y * b.X); }
		public static Coordinate operator *(Coordinate a, int b) { return new Coordinate(a.X * b, a.Y * b); }
		public static Coordinate operator *(int a, Coordinate b) { return new Coordinate(a * b.X, a * b.Y); }
		public static Coordinate operator *(Coordinate a, double b) { return new Coordinate(a.X * b, a.Y * b); }
		public static Coordinate operator *(double a, Coordinate b) { return new Coordinate(a * b.X, a * b.Y); }

		public static Coordinate operator /(Coordinate a, Coordinate b) { return new Coordinate(a.X / b.X, a.Y / b.X); }
		public static Coordinate operator /(Coordinate a, int b) { return new Coordinate(a.X / b, a.Y / b); }
		public static Coordinate operator /(int a, Coordinate b) { return new Coordinate(a / b.X, a / b.Y); }
		public static Coordinate operator /(Coordinate a, double b) { return new Coordinate(a.X / b, a.Y / b); }
		public static Coordinate operator /(double a, Coordinate b) { return new Coordinate(a / b.X, a / b.Y); }

		public static Vector operator +(Coordinate v, Angle a) { return new Vector(v.X + a.X, v.Y + a.Y); }

		public int Length()
		{
			return 2;
		}
	}

	class Vector
	{
		public double X { get; set; }
		public double Y { get; set; }
		
		public Coordinate Coord
		{
			get { return new Coordinate(X, Y); }
			set { X = value.X; Y = value.Y; }
		}

		public double Magnitude
		{
			get { return Math.Sqrt(X * X + Y * Y); }
			set { Angle = new Angle {Vec = this, Magnitude = value - Magnitude}; }
		}

		public Angle Angle
		{
			get { return new Angle {Vec = this}; }
			set { X = value.X; Y = value.Y; }
		}

		public PointF PointF
		{
			get { return new PointF((float)X, (float)Y); }
			set { X = value.X; Y = value.Y; }
		}

		public double[] Array
		{
			get { return new[] {X, Y}; }
			set
			{
				try
				{
					X = value[0];
					Y = value[1];
				}
				catch (IndexOutOfRangeException)
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
	
		public Vector()
			: this(0, 0)
		{}
	
		public Vector(int x, int y)
		{
			X = x;
			Y = y;
		}

		public Vector(double x, double y)
		{
			X = x;
			Y = y;
		}

		public Vector(Coordinate other)
		{
			X = other.X;
			Y = other.Y;
		}

		public Vector(Vector other)
		{
			X = other.X;
			Y = other.Y;
		}

		public static Vector operator +(Vector a, Coordinate b) { return new Vector(a.X + b.X, a.Y + b.X); }
		public static Vector operator +(Coordinate a, Vector b) { return new Vector(a.X + b.X, a.X + b.Y); }
		public static Vector operator +(Vector a, Vector b) { return new Vector(a.X + b.X, a.Y + b.X); }
		public static Vector operator +(Vector a, int b) { return new Vector(a.X + b, a.Y + b); }
		public static Vector operator +(int a, Vector b) { return new Vector(a + b.X, a + b.Y); }
		public static Vector operator +(Vector a, double b) { return new Vector(a.X + b, a.Y + b); }
		public static Vector operator +(double a, Vector b) { return new Vector(a + b.X, a + b.Y); }

		public static Vector operator -(Vector a, Coordinate b) { return new Vector(a.X - b.X, a.Y - b.X); }
		public static Vector operator -(Coordinate a, Vector b) { return new Vector(a.X - b.X, a.X - b.Y); }
		public static Vector operator -(Vector a, Vector b) { return new Vector(a.X - b.X, a.Y - b.X); }
		public static Vector operator -(Vector a, int b) { return new Vector(a.X - b, a.Y - b); }
		public static Vector operator -(int a, Vector b) { return new Vector(a - b.X, a - b.Y); }
		public static Vector operator -(Vector a, double b) { return new Vector(a.X - b, a.Y - b); }
		public static Vector operator -(double a, Vector b) { return new Vector(a - b.X, a - b.Y); }

		public static Vector operator *(Vector a, Coordinate b) { return new Vector(a.X * b.X, a.Y * b.X); }
		public static Vector operator *(Coordinate a, Vector b) { return new Vector(a.X * b.X, a.X * b.Y); }
		public static Vector operator *(Vector a, Vector b) { return new Vector(a.X * b.X, a.Y * b.X); }
		public static Vector operator *(Vector a, int b) { return new Vector(a.X * b, a.Y * b); }
		public static Vector operator *(int a, Vector b) { return new Vector(a * b.X, a * b.Y); }
		public static Vector operator *(Vector a, double b) { return new Vector(a.X * b, a.Y * b); }
		public static Vector operator *(double a, Vector b) { return new Vector(a * b.X, a * b.Y); }

		public static Vector operator /(Vector a, Coordinate b) { return new Vector(a.X / b.X, a.Y / b.X); }
		public static Vector operator /(Coordinate a, Vector b) { return new Vector(a.X / b.X, a.X / b.Y); }
		public static Vector operator /(Vector a, Vector b) { return new Vector(a.X / b.X, a.Y / b.X); }
		public static Vector operator /(Vector a, int b) { return new Vector(a.X / b, a.Y / b); }
		public static Vector operator /(int a, Vector b) { return new Vector(a / b.X, a / b.Y); }
		public static Vector operator /(Vector a, double b) { return new Vector(a.X / b, a.Y / b); }
		public static Vector operator /(double a, Vector b) { return new Vector(a / b.X, a / b.Y); }

		public static Vector operator +(Vector v, Angle a) { return new Vector(v.X + a.X, v.Y + a.Y); }

		public int Length()
		{
			return 2;
		}
	}

}
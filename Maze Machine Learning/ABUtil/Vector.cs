﻿using System;
using System.Collections;
using System.Drawing;

namespace Maze_Machine_Learning.ABUtil
{
	class VecEnumerable<T> : IEnumerable
	{
		public const int Length = 2;
		public T X { get; set; }
		public T Y { get; set; }

		public IEnumerator GetEnumerator()
		{
			yield return X;
			yield return Y;
		}

		public T[] Array
		{
			get { return new[] { X, Y }; }
			set { X = value[0]; Y = value[1]; }
		}

		public T this[int index]
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
	}

	class Coordinate : VecEnumerable<int>
	{
		public Vector Vec
		{
			get { return new Vector {X = X, Y = Y}; }
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

		public static Coordinate operator +(Coordinate a, Coordinate b) { return new Coordinate {X = a.X + b.X, Y = a.Y + b.Y}; }
		public static Coordinate operator +(Coordinate a, int b) { return new Coordinate {X = a.X + b, Y = a.Y + b}; }
		public static Coordinate operator +(int a, Coordinate b) { return new Coordinate {X = a + b.X, Y = a + b.Y}; }
		public static Coordinate operator +(Coordinate a, double b) { return new Coordinate {X = a.X + (int)b, Y = a.Y + (int)b}; }
		public static Coordinate operator +(double a, Coordinate b) { return new Coordinate {X = (int)a + b.X, Y = (int)a + b.Y}; }

		public static Coordinate operator -(Coordinate a, Coordinate b) { return new Coordinate {X = a.X - b.X, Y = a.Y - b.Y}; }
		public static Coordinate operator -(Coordinate a, int b) { return new Coordinate {X = a.X - b, Y = a.Y - b}; }
		public static Coordinate operator -(int a, Coordinate b) { return new Coordinate {X = a - b.X, Y = a - b.Y}; }
		public static Coordinate operator -(Coordinate a, double b) { return new Coordinate {X = a.X - (int)b, Y = a.Y - (int)b}; }
		public static Coordinate operator -(double a, Coordinate b) { return new Coordinate {X = (int)a - b.X, Y = (int)a - b.Y}; }

		public static Coordinate operator *(Coordinate a, Coordinate b) { return new Coordinate {X = a.X * b.X, Y = a.Y * b.Y}; }
		public static Coordinate operator *(Coordinate a, int b) { return new Coordinate {X = a.X * b, Y = a.Y * b}; }
		public static Coordinate operator *(int a, Coordinate b) { return new Coordinate {X = a * b.X, Y = a * b.Y}; }
		public static Coordinate operator *(Coordinate a, double b) { return new Coordinate {X = a.X * (int)b, Y = a.Y * (int)b}; }
		public static Coordinate operator *(double a, Coordinate b) { return new Coordinate {X = (int)a * b.X, Y = (int)a * b.Y}; }

		public static Coordinate operator /(Coordinate a, Coordinate b) { return new Coordinate {X = a.X / b.X, Y = a.Y / b.Y}; }
		public static Coordinate operator /(Coordinate a, int b) { return new Coordinate {X = a.X / b, Y = a.Y / b}; }
		public static Coordinate operator /(int a, Coordinate b) { return new Coordinate {X = a / b.X, Y = a / b.Y}; }
		public static Coordinate operator /(Coordinate a, double b) { return new Coordinate {X = a.X / (int)b, Y = a.Y / (int)b}; }
		public static Coordinate operator /(double a, Coordinate b) { return new Coordinate {X = (int)a / b.X, Y = (int)a / b.Y}; }

		public static Vector operator +(Coordinate v, Angle a) { return new Vector {X = v.X + a.X, Y = v.Y + a.Y}; }
		
		public static bool operator ==(Coordinate t, Vector v) { return t.Equals(v); }
		public static bool operator ==(Coordinate t, Coordinate c) { return t.Equals(c); }
		public static bool operator ==(Coordinate t, Angle a) { return t.Equals(a); }
		public static bool operator !=(Coordinate t, Vector v) { return !t.Equals(v); }
		public static bool operator !=(Coordinate t, Coordinate c) { return !t.Equals(c); }
		public static bool operator !=(Coordinate t, Angle a) { return !t.Equals(a); }

		public bool Equals(Coordinate obj) { return X == obj.X && Y == obj.Y; }
		public bool Equals(Vector obj) { return X == (int)obj.X && Y == (int)obj.Y; }
		public bool Equals(Angle obj) { return X == (int)obj.X && Y == (int)obj.Y; }
		override public bool Equals(object obj) { return false; }

		override public int GetHashCode() { return X ^ Y; }
	}


	class Vector : VecEnumerable<double>
	{
		public Coordinate Coord
		{
			get { return new Coordinate {X = (int)X, Y = (int)Y}; }
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

		public static Vector operator +(Vector a, Coordinate b) { return new Vector {X = a.X + b.X, Y = a.Y + b.Y}; }
		public static Vector operator +(Coordinate a, Vector b) { return new Vector {X = a.X + b.X, Y = a.Y + b.Y}; }
		public static Vector operator +(Vector a, Vector b) { return new Vector {X = a.X + b.X, Y = a.Y + b.Y}; }
		public static Vector operator +(Vector a, int b) { return new Vector {X = a.X + b, Y = a.Y + b}; }
		public static Vector operator +(int a, Vector b) { return new Vector {X = a + b.X, Y = a + b.Y}; }
		public static Vector operator +(Vector a, double b) { return new Vector {X = a.X + b, Y = a.Y + b}; }
		public static Vector operator +(double a, Vector b) { return new Vector {X = a + b.X, Y = a + b.Y}; }

		public static Vector operator -(Vector a, Coordinate b) { return new Vector {X = a.X - b.X, Y = a.Y - b.Y}; }
		public static Vector operator -(Coordinate a, Vector b) { return new Vector {X = a.X - b.X, Y = a.Y - b.Y}; }
		public static Vector operator -(Vector a, Vector b) { return new Vector {X = a.X - b.X, Y = a.Y - b.Y}; }
		public static Vector operator -(Vector a, int b) { return new Vector {X = a.X - b, Y = a.Y - b}; }
		public static Vector operator -(int a, Vector b) { return new Vector {X = a - b.X, Y = a - b.Y}; }
		public static Vector operator -(Vector a, double b) { return new Vector {X = a.X - b, Y = a.Y - b}; }
		public static Vector operator -(double a, Vector b) { return new Vector {X = a - b.X, Y = a - b.Y}; }

		public static Vector operator *(Vector a, Coordinate b) { return new Vector {X = a.X * b.X, Y = a.Y * b.Y}; }
		public static Vector operator *(Coordinate a, Vector b) { return new Vector {X = a.X * b.X, Y = a.Y * b.Y}; }
		public static Vector operator *(Vector a, Vector b) { return new Vector {X = a.X * b.X, Y = a.Y * b.Y}; }
		public static Vector operator *(Vector a, int b) { return new Vector {X = a.X * b, Y = a.Y * b}; }
		public static Vector operator *(int a, Vector b) { return new Vector {X = a * b.X, Y = a * b.Y}; }
		public static Vector operator *(Vector a, double b) { return new Vector {X = a.X * b, Y = a.Y * b}; }
		public static Vector operator *(double a, Vector b) { return new Vector {X = a * b.X, Y = a * b.Y}; }

		public static Vector operator /(Vector a, Coordinate b) { return new Vector {X = a.X / b.X, Y = a.Y / b.Y}; }
		public static Vector operator /(Coordinate a, Vector b) { return new Vector {X = a.X / b.X, Y = a.Y / b.Y}; }
		public static Vector operator /(Vector a, Vector b) { return new Vector {X = a.X / b.X, Y = a.Y / b.Y}; }
		public static Vector operator /(Vector a, int b) { return new Vector {X = a.X / b, Y = a.Y / b}; }
		public static Vector operator /(int a, Vector b) { return new Vector {X = a / b.X, Y = a / b.Y}; }
		public static Vector operator /(Vector a, double b) { return new Vector {X = a.X / b, Y = a.Y / b}; }
		public static Vector operator /(double a, Vector b) { return new Vector {X = a / b.X, Y = a / b.Y}; }

		public static Vector operator +(Vector v, Angle a) { return new Vector {X = v.X + a.X, Y = v.Y + a.Y}; }
		
		public static bool operator ==(Vector t, Vector v) { return t.Equals(v); }
		public static bool operator ==(Vector t, Coordinate c) { return t.Equals(c); }
		public static bool operator ==(Vector t, Angle a) { return t.Equals(a); }
		public static bool operator !=(Vector t, Vector v) { return !t.Equals(v); }
		public static bool operator !=(Vector t, Coordinate c) { return !t.Equals(c); }
		public static bool operator !=(Vector t, Angle a) { return !t.Equals(a); }

		public bool Equals(Coordinate obj) { return (int)X == obj.X && (int)Y == obj.Y; }
		public bool Equals(Vector obj) { return ExtraMath.Compare(X, obj.X) && ExtraMath.Compare(Y, obj.Y); }
		public bool Equals(Angle obj) { return ExtraMath.Compare(X, obj.X) && ExtraMath.Compare(Y, obj.Y); }
		override public bool Equals(object obj) { return false; }

		override public int GetHashCode() { return (int)X ^ (int)Y; }
	}

}
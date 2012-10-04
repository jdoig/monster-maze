using System;

namespace MonsterMaze
{
	public struct Point
	{
		public int X, Y, Z;
		
		public Point(int x, int y, int z)
		{
			X = x;
			Y = y;
			Z = z;
		}
		public Point(int x, int y): this(x, y, 0){}

		public static Point operator +(Point a, Point b)
		{
			return new Point(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
		}
		public static Point operator -(Point a, Point b)
		{
			return new Point(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
		}

		public override string ToString()
		{
			return String.Format("{0},{1}", X, Y);
		}

		public bool Equals(Point other)
		{
			return other.X == X && other.Y == Y && other.Z == Z;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (obj.GetType() != typeof (Point)) return false;
			return Equals((Point) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int result = X;
				result = (result*397) ^ Y;
				result = (result*397) ^ Z;
				return result;
			}
		}

		public static bool operator ==(Point left, Point right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Point left, Point right)
		{
			return !left.Equals(right);
		}
	}
}
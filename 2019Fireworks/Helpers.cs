using System.Collections.Generic;
using System.Drawing;

namespace _2019Fireworks
{
	public static class Helpers
	{
		public static IEnumerable<Point> RemoveExtraneousPoints(this IEnumerable<Point> points)
		{
			foreach (var point in points)
			{
				if (point.X < 0 || point.X > 79 ||
					point.Y < 0 || point.Y > 24)
				{
					continue;
				}

				yield return point;
			}
		}
	}
}
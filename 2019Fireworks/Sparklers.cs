using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace _2019Fireworks
{
	public class Sparklers : IEntity
	{
		public Sparklers(Random rng, ConsoleColor color, int x, int y)
		{
			SparkleLifetime = rng.Next(3, 10);
			_rng = rng;
			_color = color;
			_x = x;
			_y = y;
		}

		public int SparkleLifetime { get; }

		private int _sparkles;
		private List<Point> _thisScene = new List<Point>();
		private List<Point> _previousScene = new List<Point>();
		private readonly Random _rng;
		private readonly ConsoleColor _color;
		private readonly int _x;
		private readonly int _y;

		public void Draw()
		{
			foreach (var i in _previousScene)
			{
				Console.SetCursorPosition(i.X, i.Y);
				Console.Write(' ');
			}

			Console.ForegroundColor = _color;
			foreach (var i in _thisScene)
			{
				Console.SetCursorPosition(i.X, i.Y);
				Console.Write('*');
			}
		}

		public Action RequestDeathWish(IEntityManager entityManager) => () =>
		{
			entityManager.Spawn(new Firework(_rng));
		};

		public UpdateResult Update()
		{
			_previousScene = new List<Point>(_thisScene);

			_thisScene = new List<Point>();

			_sparkles++;

			if (_sparkles == SparkleLifetime)
			{
				// clears out the previously rendered scene
				return UpdateResult.Draw;
			}

			if (_sparkles >= SparkleLifetime)
			{
				return UpdateResult.Kill;
			}

			var max = _rng.Next(4, 10);

			for (int i = 0; i < max; i++)
			{
				_thisScene.Add(new Point(_rng.Next(_x - 2, _x + 2), _rng.Next(_y - 2, _y + 2)));
			}

			_thisScene = _thisScene.RemoveExtraneousPoints().Distinct().ToList();

			return UpdateResult.Draw;
		}

		public void Dispose()
		{
		}
	}
}
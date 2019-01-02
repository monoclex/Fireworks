using System;

namespace _2019Fireworks
{
	public class Firework : IEntity
	{
		public Firework(Random rng)
		{
			X = rng.Next(0, Console.WindowWidth - 1);
			Y = rng.Next(Console.WindowHeight - 5, Console.WindowHeight);
			MaxY = rng.Next(2, 9);
			UpdatesUntilDraw = rng.Next(0, 7);
			Color = (ConsoleColor)rng.Next(0, 16);
			_rng = rng;
		}

		public int X { get; }
		public int Y { get; private set; }
		public int MaxY { get; }
		public int UpdatesUntilDraw { get; }
		public ConsoleColor Color { get; }

		private int _updates;
		private readonly Random _rng;

		public void Draw()
		{
			Console.ForegroundColor = Color;

			Console.SetCursorPosition(X, Y);
			Console.Write('|');

			if (Y + 1 <= 25)
			{
				Console.SetCursorPosition(X, Y + 1);
				Console.Write(' ');
			}
		}

		public UpdateResult Update()
		{
			if (Y <= MaxY)
			{
				return UpdateResult.Kill;
			}

			if (_updates++ >= UpdatesUntilDraw)
			{
				_updates = 0;
				Y--;
				return UpdateResult.Draw;
			}

			return UpdateResult.Standby;
		}

		public Action RequestDeathWish(IEntityManager entityManager) => () => { entityManager.Spawn(new Sparklers(_rng, Color, X, Y)); };

		public void Dispose()
		{
			Console.SetCursorPosition(X, Y);
			Console.Write(' ');
		}
	}
}
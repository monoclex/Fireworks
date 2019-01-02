using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace _2019Fireworks
{
	public class EntityManager : IEntityManager
	{
		private List<IEntity> _entities = new List<IEntity>();
		private readonly object _entityLock = new object();

		public async Task Run(int msDelay)
		{
			var wasDelay = false;
			var lastDelay = 0L;

			while (true)
			{
				var requests = new List<Action>();
				var kill = new List<IEntity>();
				var draw = new List<IEntity>();

				lock (_entityLock)
				{
					foreach (var i in _entities)
					{
						switch (i.Update())
						{
							case UpdateResult.Draw:
							{
								draw.Add(i);
							}
							break;

							case UpdateResult.Kill:
							{
								kill.Add(i);
							}
							break;

							default: break;
						}
					}

					foreach (var entity in kill)
					{
						requests.Add(entity.RequestDeathWish(this));
						entity.Dispose();
						_entities.Remove(entity);
					}
				}

				var drawingTask = Task.Run(() =>
				{
					foreach (var entity in draw)
					{
						entity.Draw();
					}
				});

				var fufillRequests = Task.Run(() =>
				{
					foreach (var request in requests)
					{
						request();
					}
				});

				await Task.Delay(msDelay);

				// TODO: time if these tasks are late
				var delay = await TimeTask(Task.WhenAll(drawingTask, fufillRequests));

				if (delay >= 10)
				{
					wasDelay = true;

					Console.SetCursorPosition(0, 0);

					Console.ForegroundColor = ConsoleColor.DarkRed;
					Console.BackgroundColor = ConsoleColor.White;

					Console.Write($@"/!\ DELAY /!\ {delay}ms");

					Console.BackgroundColor = ConsoleColor.Black;
				}
				else if (wasDelay)
				{
					Console.SetCursorPosition(0, 0);

					// i'm lazy ok :(
					// "/!\ DELAY /!\ ms" is the 16, + length of delay
					Console.Write(new string(' ', 16 + $"{lastDelay}".Length));

					wasDelay = false;
				}

				lastDelay = delay;
			}
		}

		private async Task<long> TimeTask(Task task)
		{
			var stopwatch = Stopwatch.StartNew();

			await task;

			stopwatch.Stop();

			return stopwatch.ElapsedMilliseconds;
		}

		public void Spawn(IEntity entity)
		{
			lock (_entityLock)
			{
				_entities.Add(entity);
			}
		}
	}
}
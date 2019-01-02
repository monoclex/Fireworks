using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _2019Fireworks
{
	public class EntityManager : IEntityManager
	{
		private List<IEntity> _entities = new List<IEntity>();
		private readonly object _entityLock = new object();

		public async Task Run(int msDelay)
		{
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
				await Task.WhenAll(drawingTask, fufillRequests);
			}
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
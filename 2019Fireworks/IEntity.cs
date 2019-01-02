using System;

namespace _2019Fireworks
{
	public interface IEntity : IDisposable
	{
		void Draw();

		UpdateResult Update();

		Action RequestDeathWish(IEntityManager entityManager);
	}
}
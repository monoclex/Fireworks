using System.Threading.Tasks;

namespace _2019Fireworks
{
	public interface IEntityManager
	{
		Task Run(int msDelay);

		void Spawn(IEntity entity);
	}
}
using System;
using System.Threading.Tasks;

namespace _2019Fireworks
{
	public class Program
	{
		public static Task Main(string[] args)
		{
			Console.CursorVisible = false;

			var em = new EntityManager();

			var rng = new Random();

			for (int i = 0; i < 75; i++)
			{
				em.Spawn(new Firework(rng));
			}

			return em.Run(100);
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Console.AsyncAwait
{
	class AsyncCounter
	{
		public async Task<int> StartCounterAsync(int size)
		{
			//int result = await CountAsync(size);
			//return result;

			return await CountAsync(size);
		}

		private Task<int> CountAsync(int upperBound)
		{
			return Task<int>.Factory.StartNew(() =>
			{
				int counter = 0;
				for (int i = 0; i < upperBound; i++)
				{
					System.Console.Clear();
					System.Console.WriteLine(i);
					Thread.Sleep(1000);

					counter += i;
				}

				return counter;
			});
		}
	}
}

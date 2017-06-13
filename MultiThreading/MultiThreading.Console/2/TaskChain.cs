using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreading.Console._2
{
	class TaskChain
	{
		public static Task<T> Start<T>(Func<T> initialAction)
		{
			return Task<T>.Factory.StartNew(initialAction);
		}

		public static int[] GenerateArray(int arrayLength, int upperBound = 0, int lowerBound = 10)
		{
			Random rnd = new Random(Environment.TickCount);

			System.Console.WriteLine("Method");
			return Enumerable.Range(0, arrayLength).Select(v => rnd.Next(upperBound, lowerBound)).ToArray();
		}

		public static int[] MultiplyBy(int[] array, int multiplier)
		{
			System.Console.WriteLine("Method");
			return array.Select(v => v*multiplier).ToArray();
		}

		public static int[] SortAscending(int[] array)
		{
			System.Console.WriteLine("Method");
			return array.OrderBy(val => val).ToArray();
		}

		public static double Average(int[] array)
		{
			System.Console.WriteLine("Method");
			return array.Average();
		}
	}
}

using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Console._2
{
	/// <summary>
	/// Task chain methods helper.
	/// </summary>
	class TaskChain
	{
		private static ThreadLocal<Random> rnd;

		static TaskChain()
		{
			rnd = new ThreadLocal<Random>(() => new Random(Environment.TickCount));
		}

		public static int[] GenerateArray(int arrayLength, int upperBound = 0, int lowerBound = 10)
		{
			int[] result = Enumerable.Range(0, arrayLength).Select(v => rnd.Value.Next(upperBound, lowerBound)).ToArray();
			return result;
		}

		public static int[] MultiplyBy(int[] array)
		{
			int[] result = array.Select(v => v * rnd.Value.Next(0, 10)).ToArray();
			return result;
		}

		public static int[] SortAscending(int[] array)
		{
			int[] result = array.OrderBy(val => val).ToArray();
			return result;
		}

		public static double Average(int[] array)
		{
			double result = array.Average();
			return result;
		}
	}

	public static class ContinuationExt
	{
		public static Task<T[]> LogAndContinueArray<T>(this Task<T[]> task, Func<T[], T[]> nextFunc)
		{
			T[] result = task.Result;
			result.CustomLog();
			return task.ContinueWith(ant => nextFunc(result));
		}

		public static Task<double> LogAndContinue<T>(this Task<T[]> task, Func<T[], double> nextFunc)
		{
			T[] result = task.Result;
			result.CustomLog();
			return task.ContinueWith(ant => nextFunc(result));
		}

		public static Task LogAndContinue<T>(this Task<T> task, Action nextAction)
		{
			T result = task.Result;
			result.CustomLog();
			return task.ContinueWith(ant => nextAction());
		}

	}

	public static class LogingExt
	{
		public static void CustomLog<T>(this T[] result)
		{
			StringBuilder builder = new StringBuilder();
			foreach (T value in result)
			{
				builder.Append($"{value} ");
			}

			System.Console.WriteLine(builder.ToString());
		}

		public static void CustomLog<T>(this T result)
		{
			System.Console.WriteLine(result);
		}
	}
}

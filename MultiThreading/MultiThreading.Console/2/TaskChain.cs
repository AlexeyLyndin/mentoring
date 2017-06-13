using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreading.Console._2
{
	class TaskChain
	{
		public static int[] GenerateArray(int arrayLength, int upperBound = 0, int lowerBound = 10)
		{
			Random rnd = new Random(Environment.TickCount);
			int[] result = Enumerable.Range(0, arrayLength).Select(v => rnd.Next(upperBound, lowerBound)).ToArray();
			return result;
		}

		public static int[] MultiplyBy(int[] array)
		{
			int[] result = array.Select(v => v * 2).ToArray();
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

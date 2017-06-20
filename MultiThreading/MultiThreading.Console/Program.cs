using System;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Running;
using MultiThreading.Console._1;
using MultiThreading.Console._2;
using MultiThreading.Console._3;
using MultiThreading.Console._4;
using MultiThreading.Console._6;
using MultiThreading.Console._7;

namespace MultiThreading.Console
{
	class Program
	{
		static void Main(string[] args)
		{
			//Check1();
			//Check2();
			//Check3();
			//Check4_5();
			//Check6();
			Check7();
		}

		public static void Check1()
		{
			new TasksArray(100).InitializeTasks(() =>
			{
				for (int i = 1; i <= 1000; i++)
				{
					System.Console.WriteLine($"Task #{Task.CurrentId} - {i}");
				}
			}).Start().Wait();
		}

		public static void Check2()
		{
			Task<int[]>.Factory
				.StartNew(() => TaskChain.GenerateArray(10))
				.LogAndContinueArray(TaskChain.MultiplyBy)
				.LogAndContinueArray(TaskChain.SortAscending)
				.LogAndContinue(TaskChain.Average)
				.LogAndContinue(() => System.Console.WriteLine("Finish"))
				.Wait();
		}

		public static void Check3()
		{
			BenchmarkRunner.Run<MatrixBenchmark>();
		}

		public static void Check4_5()
		{
			//RecursiveThread.CreateThread(100);
			RecursiveThread.CreateThreadPool(100);

		}

		public static void Check6()
		{
			//BenchmarkRunner.Run<BenchmarkCRW>();
			ConcurrentReadWrite.StartMRES();
			ConcurrentReadWrite.StartARE();
		}

		public static void Check7()
		{
			TaskContinuations.Do(ParentTaskOpt.B);
		}
	}
}
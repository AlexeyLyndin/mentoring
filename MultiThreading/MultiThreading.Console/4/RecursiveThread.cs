using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace MultiThreading.Console._4
{
	public class RecursiveThread
	{
		private static SemaphoreSlim ss;

		static RecursiveThread()
		{
			ss = new SemaphoreSlim(Environment.ProcessorCount);
		}

		public static void CreateThread(int state)
		{
			InitializeThread(state);
		}

		public static void CreateThreadPool(int state)
		{
			int minWorker, minIOC;
			int maxWorker, maxIOC;
			ThreadPool.GetMaxThreads(out maxWorker, out maxIOC);
			ThreadPool.GetMinThreads(out minWorker, out minIOC);

			bool isSetMin = ThreadPool.SetMinThreads(minIOC, minIOC);
			bool isSetMax = ThreadPool.SetMaxThreads(maxWorker, maxIOC);

			ThreadPoolAction(state);
		}

		public static void CreateThreadPoolNoOpts(int state)
		{
			ThreadPoolAction(state);
		}

		private static void InitializeThread(int initialState)
		{
			Thread thread = new Thread(ThreadAction);
			thread.Start(initialState);
			thread.Join();
		}

		private static void ThreadPoolAction(int initialState)
		{
			ThreadPool.QueueUserWorkItem(act =>
			{
				int currentState = (int)act;
				ss.Wait();
				System.Console.WriteLine($"Current thread id = {Thread.CurrentThread.ManagedThreadId}, State is {--currentState}");
				if (currentState > 0)
				{
					ThreadPoolAction(currentState);
				}
				else
				{
					System.Console.WriteLine("Finish");
				}
				ss.Release();
			}, initialState);
		}

		private static void ThreadAction(object state)
		{
			int currentState = (int)state;
			System.Console.WriteLine($"Current thread id = {Thread.CurrentThread.ManagedThreadId}, State is {--currentState}");
			if (currentState > 0)
			{
				InitializeThread(currentState);
			}
			else
			{
				System.Console.WriteLine("Finish");
			}
		}
	}

	public class BenchmarThreadWorks
	{
		[Benchmark]
		public void SimpleThread() => RecursiveThread.CreateThread(2);

		[Benchmark]
		public void ThreadPoolNoOpts() => RecursiveThread.CreateThreadPoolNoOpts(2);

		[Benchmark]
		public void ThreadPool() => RecursiveThread.CreateThreadPool(2);
	}
}

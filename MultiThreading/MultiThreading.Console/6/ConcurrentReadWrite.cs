using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace MultiThreading.Console._6
{
	public class ConcurrentReadWrite
	{
		private static IList<int> _sharedList;

		private static ManualResetEventSlim writeEvent;
		private static ManualResetEventSlim readEvent;

		private static EventWaitHandle eWriteHandle;
		private static EventWaitHandle eReadHandle;

		private static ThreadLocal<Random> rnd;

		private static int N = 10;

		static ConcurrentReadWrite()
		{
			_sharedList = new List<int>(N);

			writeEvent = new ManualResetEventSlim(true);
			readEvent = new ManualResetEventSlim(false);

			eWriteHandle = new AutoResetEvent(true);
			eReadHandle = new AutoResetEvent(false);

			rnd = new ThreadLocal<Random>(() => new Random(Environment.TickCount));
		}
		public static void StartMRES()
		{
			_sharedList.Clear();
			Task.WaitAll(StartWritingMRES(N), StartReadingMRES(N));
		}

		public static void StartARE()
		{
			_sharedList.Clear();
			Task.WaitAll(StartWritingEWH(N), StartReadingEWH(N));
		}

		private static Task StartWritingMRES(int size)
		{
			return Task.Factory.StartNew(() =>
			{

				while (_sharedList.Count < size)
				{
					writeEvent.Wait();
					writeEvent.Reset();
					_sharedList.Add(rnd.Value.Next(0, 10));
					readEvent.Set();
				}
			});
		}

		private static Task StartReadingMRES(int size)
		{
			return Task.Factory.StartNew(() =>
			{
				while (_sharedList.Count < size)
				{
					readEvent.Wait();
					readEvent.Reset();
					StringBuilder sb = new StringBuilder();
					foreach (int element in _sharedList)
					{
						sb.Append($"{element} ");
					}
					System.Console.WriteLine(sb.ToString());
					writeEvent.Set();
				}

			});
		}

		private static Task StartWritingEWH(int size)
		{
			return Task.Factory.StartNew(() =>
			{
				while (_sharedList.Count < size)
				{
					eWriteHandle.WaitOne();
					_sharedList.Add(rnd.Value.Next(0, 10));
					eReadHandle.Set();
				}
			});
		}

		private static Task StartReadingEWH(int size)
		{
			return Task.Factory.StartNew(() =>
			{
				while (_sharedList.Count < size)
				{
					eReadHandle.WaitOne();
					StringBuilder sb = new StringBuilder();
					foreach (int element in _sharedList)
					{
						sb.Append($"{element} ");
					}
					System.Console.WriteLine(sb.ToString());
					eWriteHandle.Set();
				}

			});
		}
	}
}

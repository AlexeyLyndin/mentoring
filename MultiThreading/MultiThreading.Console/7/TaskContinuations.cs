using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Console._7
{
	public class TaskContinuations
	{
		private static CancellationTokenSource cts = new CancellationTokenSource();
		private static CancellationToken token = cts.Token;

		public static void Do(ChildTaskOpt opt)
		{
			System.Console.WriteLine($"Main thread ID - {Thread.CurrentThread.ManagedThreadId}");
			Task task = Task.Factory.StartNew(() => ParentAction(opt), token);


			if (ChildTaskOpt.D == opt)
			{
				cts.Cancel();
			}
			task.ContinueWith(ant =>
			{
				System.Console.WriteLine("Continuation A");
			}).Wait();

			task.ContinueWith(ant =>
			{
				System.Console.WriteLine("Continuation B");
			}, TaskContinuationOptions.OnlyOnFaulted);

			task.ContinueWith(ant =>
			{
				System.Console.WriteLine("Continuation C");
				System.Console.WriteLine($"Continuation thread ID - {Thread.CurrentThread.ManagedThreadId}");
			}, TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnFaulted);

			task.ContinueWith(ant =>
			{
				System.Console.WriteLine("Continuation D");
				System.Console.WriteLine($"Is Continuation on TP - {Thread.CurrentThread.IsThreadPoolThread}");
			}, TaskContinuationOptions.OnlyOnCanceled | TaskContinuationOptions.LongRunning);

			
		}

		private static void ParentAction(object opt)
		{
			TaskCreationOptions atp = TaskCreationOptions.AttachedToParent;
			ChildTaskOpt option = (ChildTaskOpt)opt;

			switch (option)
			{
				case ChildTaskOpt.A:
					Task.Factory.StartNew(() =>
					{
						System.Console.WriteLine("Case A");
					}, atp);
					break;
				case ChildTaskOpt.B:
					Task.Factory.StartNew(() => { throw null; }, atp);
					break;
				case ChildTaskOpt.C:
					System.Console.WriteLine($"Parent task thread ID - {Thread.CurrentThread.ManagedThreadId}");
					Task.Factory.StartNew(() =>
					{
						System.Console.WriteLine($"Child task thread ID - {Thread.CurrentThread.ManagedThreadId}");
						throw null;
					}, atp);
					break;
				case ChildTaskOpt.D:
					System.Console.WriteLine("Canceling...");
					token.ThrowIfCancellationRequested();
					break;
				default:
					System.Console.WriteLine("Default");
					break;
			}
		}
	}

	public enum ChildTaskOpt
	{
		A,
		B,
		C,
		D
	}
}
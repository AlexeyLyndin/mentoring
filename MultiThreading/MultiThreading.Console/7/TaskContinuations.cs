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
		public static void Do(ParentTaskOpt opt)
		{
			IList<Task> continuationList = new List<Task>();
			Task task = Task.Factory.StartNew(() => ParentAction(opt));
			Task next;
			switch (opt)
			{
				case ParentTaskOpt.A:
					next = task.ContinueWith(ant =>
					{
						Thread.Sleep(1000);
						System.Console.WriteLine("Simple continuation");
					});
					break;
				case ParentTaskOpt.B:
					next = task.ContinueWith(ant =>
					{
						System.Console.WriteLine(ant.Exception?.Message);
					}, TaskContinuationOptions.OnlyOnFaulted);
					break;
				default:
					next = new Task(() => {});
					break;
			}

			next.ContinueWith(ant => System.Console.WriteLine("Stop")).Wait();
		}

		private static void ParentAction(object opt)
		{
			TaskCreationOptions atp = TaskCreationOptions.AttachedToParent;
			ParentTaskOpt option = (ParentTaskOpt)opt;

			switch (option)
			{
				case ParentTaskOpt.A:
					Task.Factory.StartNew(() =>
					{
						Thread.Sleep(1000);
						System.Console.WriteLine("Case A");
					});
					break;
				case ParentTaskOpt.B:
					Task.Factory.StartNew(() => { throw null; }, atp);
					break;
				default:
					System.Console.WriteLine("Default");
					break;
			}
		}
	}

	public enum ParentTaskOpt
	{
		A,
		B,
		C,
		D
	}
}

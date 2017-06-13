using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MultiThreading.Console._1;
using MultiThreading.Console._2;

namespace MultiThreading.Console
{
	class Program
	{
		static void Main(string[] args)
		{
			//Check1();
			Check2();
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
			//TaskChain.Start(() => TaskChain.GenerateArray(10))
			//	.ContinueWith(ant => TaskChain.MultiplyBy(ant.Result))
			//	.ContinueWith(ant => TaskChain.SortAscending(ant.Result))
			//	.ContinueWith(ant => TaskChain.Average(ant.Result))
			//	.Wait();

			TaskChain
				.Start(() => TaskChain.GenerateArray(10))
				.LogAndContinueArray(TaskChain.MultiplyBy)
				.LogAndContinueArray(TaskChain.SortAscending)
				.LogAndContinue(TaskChain.Average)
				.ContinueWith(ant => System.Console.WriteLine(ant.Result))
				.Wait();
		}

	}
}
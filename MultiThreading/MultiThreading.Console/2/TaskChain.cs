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
	}
}

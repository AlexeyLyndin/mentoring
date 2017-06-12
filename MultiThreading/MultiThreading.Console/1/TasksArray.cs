using System;
using System.Threading.Tasks;

namespace MultiThreading.Console._1
{
	/// <summary>
	/// Class that represents array of tasks with the appropriate action per each task.
	/// </summary>
	public class TasksArray
	{
		private readonly Task[] _tasks;
		private const int N = 100;

		/// <summary>
		/// Initialize instance of <see cref="TasksArray"/>.
		/// </summary>
		public TasksArray()
		{
			this._tasks = new Task[N];
		}

		/// <summary>
		/// Initialize instance of <see cref="TasksArray"/>.
		/// </summary>
		/// <param name="arrayLength"></param>
		public TasksArray(int arrayLength)
		{
			this._tasks = new Task[arrayLength];
		}

		/// <summary>
		/// Initialize each array element as an Task.
		/// </summary>
		/// <param name="taskAction">Action to be executed per each task.</param>
		/// <returns>Returns an instance of <see cref="TasksArray"/>.</returns>
		public TasksArray InitializeTasks(Action taskAction)
		{
			for (int i = 0; i < this._tasks.Length; i++)
			{
				this._tasks[i] = new Task(taskAction);
			}

			return this;
		}

		/// <summary>
		/// Starts each task of internal array.
		/// </summary>
		/// <returns>Returns an instance of <see cref="TasksArray"/>.</returns>
		public TasksArray Start()
		{
			foreach (Task task in this._tasks)
			{
				task.Start();
			}

			return this;
		}

		/// <summary>
		/// Waits untill all tasks will be executed.
		/// </summary>
		public void Wait()
		{
			Task.WaitAll(this._tasks);
		}
	}
}

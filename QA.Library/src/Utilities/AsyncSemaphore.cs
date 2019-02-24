namespace Clix.Utilities
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Threading;
	using System.Threading.Tasks;

	/// <summary>
	/// 	An <c>async</c>/<c>await</c>-compatible implementation of <see cref="Semaphore"/>.
	/// </summary>
	[DebuggerStepThrough]
	[DebuggerNonUserCode]
	public class AsyncSemaphore
	{
		/// <summary>
		/// 	A cached, pre-completed task.
		/// </summary>
		private static readonly Task PreCompleted = Task.FromResult(true);

		/// <summary>
		/// 	Task-completion sources representing pending wait operations.
		/// </summary>
		private readonly Queue<TaskCompletionSource<bool>> waitOperations = new Queue<TaskCompletionSource<bool>>();

		/// <summary>
		/// 	The semaphore's current count.
		/// </summary>
		/// <remarks>
		/// 	Wait operations will block when the count is 0 (until it becomes non-zero).
		/// </remarks>
		private int currentCount;

		/// <summary>
		/// Initializes a new instance of the <see cref="AsyncSemaphore"/> class.
		/// </summary>
		/// <param name="initialCount">
		/// 	The initial count for the semaphore.
		/// </param>
		public AsyncSemaphore(int initialCount)
		{
			if (initialCount <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(initialCount));
			}

			this.currentCount = initialCount;
		}

		/// <summary>
		/// 	Asynchronously wait for the semaphore's count to be non-zero.
		/// </summary>
		/// <returns>
		/// 	A <see cref="Task"/> representing the wait operation.
		/// </returns>
		public Task WaitAsync()
		{
			lock (this.waitOperations)
			{
				if (this.currentCount > 0)
				{
					--this.currentCount;

					return AsyncSemaphore.PreCompleted;
				}

				TaskCompletionSource<bool> waitOperation = new TaskCompletionSource<bool>();
				this.waitOperations.Enqueue(waitOperation);

				return waitOperation.Task;
			}
		}

		/// <summary>
		/// 	Increment the semaphore's count.
		/// </summary>
		public void Release()
		{
			TaskCompletionSource<bool> toRelease = null;
			lock (this.waitOperations)
			{
				if (this.waitOperations.Count > 0)
				{
					toRelease = this.waitOperations.Dequeue();
				}
				else
				{
					++this.currentCount;
				}
			}

			if (toRelease != null)
			{
				toRelease.SetResult(true);
			}
		}
	}
}
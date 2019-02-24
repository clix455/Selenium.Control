namespace Clix.Utilities
{
	using System;
	using System.Diagnostics;
	using System.Threading;
	using System.Threading.Tasks;

	/// <summary>
	/// 	An <c>async</c>/<c>await</c>-compatible lock primitive.
	/// </summary>
	[DebuggerStepThrough]
	[DebuggerNonUserCode]
	public class AsyncLock
	{
		/// <summary>
		/// 	The current releaser.
		/// </summary>
		private readonly Task<IDisposable> releaser;

		/// <summary>
		/// 	The semaphore used to provide wait semantics.
		/// </summary>
		private readonly AsyncSemaphore semaphore = new AsyncSemaphore(1);

		/// <summary>
		/// Initializes a new instance of the <see cref="AsyncLock"/> class.
		/// </summary>
		public AsyncLock()
		{
			this.releaser = Task.FromResult((IDisposable)new Releaser(this));
		}

		/// <summary>
		/// 	Asynchronously acquire the lock.
		/// </summary>
		/// <returns>
		/// 	An <see cref="IDisposable"/> that releases the lock, when disposed.
		/// </returns>
		public Task<IDisposable> LockAsync()
		{
			Task waitOperation = this.semaphore.WaitAsync();
			return waitOperation.IsCompleted
						? this.releaser
						: waitOperation.ContinueWith(
							(_, state) => (IDisposable)new Releaser((AsyncLock)state),
							this,
							CancellationToken.None,
							TaskContinuationOptions.ExecuteSynchronously,
							TaskScheduler.Default);
		}

		#region Releaser

		/// <summary>
		/// 	An <see cref="IDisposable"/> implementation that releases the lock when disposed.
		/// </summary>
		private struct Releaser : IDisposable
		{
			/// <summary>
			/// 	The <see cref="AsyncLock"/> to release.
			/// </summary>
			private AsyncLock lockToRelease;

			/// <summary>
			/// Initializes a new instance of the <see cref="Releaser"/> struct.
			/// </summary>
			/// <param name="lockToRelease">
			/// 	The <see cref="AsyncLock"/> to release.
			/// </param>
			public Releaser(AsyncLock lockToRelease)
			{
				if (lockToRelease == null)
				{
					throw new ArgumentNullException(nameof(lockToRelease));
				}

				this.lockToRelease = lockToRelease;
			}

			/// <summary>
			/// 	Release the lock.
			/// </summary>
			public void Dispose()
			{
				this.lockToRelease.semaphore.Release();
			}
		}

		#endregion // Releaser
	}
}
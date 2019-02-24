// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThreadAffinitiveSynchronizationContext.cs" company="">
//  
// </copyright>
// 
// <summary>
//   A synchronisation context that runs all calls scheduled on it (via <see cref="SynchronizationContext.Post" />) on a
//   single thread.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Clix.Vsts.Powershell.Commands
{
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Runtime.ExceptionServices;
	using System.Threading;
	using System.Threading.Tasks;

	/// <summary>
	///     A synchronisation context that runs all calls scheduled on it (via <see cref="SynchronizationContext.Post" />) on a
	///     single thread.
	/// </summary>
	public sealed class ThreadAffinitiveSynchronizationContext
		: SynchronizationContext, IDisposable
	{
		/// <summary>
		///     A blocking collection (effectively a queue) of work items to execute, consisting of callback delegates and their
		///     callback state (if any).
		/// </summary>
		private BlockingCollection<KeyValuePair<SendOrPostCallback, object>> _workItemQueue = new BlockingCollection<KeyValuePair<SendOrPostCallback, object>>();

		/// <summary>
		/// Run an asynchronous operation using the current thread as its synchronisation context.
		/// </summary>
		/// <param name="asyncOperation">
		/// A <see cref="Func{TResult}"/> delegate representing the asynchronous operation to run.
		/// </param>
		public static void RunSynchronized(Func<Task> asyncOperation)
		{
			if (asyncOperation == null)
				throw new ArgumentNullException(nameof(asyncOperation));

			SynchronizationContext savedContext = Current;
			try
			{
				using (ThreadAffinitiveSynchronizationContext synchronizationContext = new ThreadAffinitiveSynchronizationContext())
				{
					SetSynchronizationContext(synchronizationContext);

					Task rootOperationTask = asyncOperation();
					if (rootOperationTask == null)
						throw new InvalidOperationException("The asynchronous operation delegate cannot return null.");

					rootOperationTask
						.ContinueWith(
							operationTask =>
							synchronizationContext.TerminateMessagePump(), TaskScheduler.Default
						);

					synchronizationContext.RunMessagePump();

					try
					{
						rootOperationTask
							.GetAwaiter()
							.GetResult();
					}
					catch (AggregateException eWaitForTask)
					{
						// The TPL will almost always wrap an AggregateException around any exception thrown by the async operation.
						// Is this just a wrapped exception?
						AggregateException flattenedAggregate = eWaitForTask.Flatten();
						if (flattenedAggregate.InnerExceptions.Count != 1)
							throw; // Nope, genuine aggregate.

						// Yep, so rethrow (preserving original stack-trace).
						ExceptionDispatchInfo
							.Capture(
								flattenedAggregate
									.InnerExceptions[0]
							)
							.Throw();
					}
				}
			}
			finally
			{
				SetSynchronizationContext(savedContext);
			}
		}

		/// <summary>
		/// Run an asynchronous operation using the current thread as its synchronisation context.
		/// </summary>
		/// <typeparam name="TResult">
		/// The operation result type.
		/// </typeparam>
		/// <param name="asyncOperation">
		/// A <see cref="Func{TResult}"/> delegate representing the asynchronous operation to run.
		/// </param>
		/// <returns>
		/// The operation result.
		/// </returns>
		public static TResult RunSynchronized<TResult>(Func<Task<TResult>> asyncOperation)
		{
			if (asyncOperation == null)
				throw new ArgumentNullException(nameof(asyncOperation));

			SynchronizationContext savedContext = Current;
			try
			{
				using (ThreadAffinitiveSynchronizationContext synchronizationContext = new ThreadAffinitiveSynchronizationContext())
				{
					SetSynchronizationContext(synchronizationContext);

					Task<TResult> rootOperationTask = asyncOperation();
					if (rootOperationTask == null)
						throw new InvalidOperationException("The asynchronous operation delegate cannot return null.");

					rootOperationTask
						.ContinueWith(
							operationTask =>
							synchronizationContext.TerminateMessagePump(), TaskScheduler.Default
						);

					synchronizationContext.RunMessagePump();

					try
					{
						return
							rootOperationTask
								.GetAwaiter()
								.GetResult();
					}
					catch (AggregateException eWaitForTask)
					{
						// The TPL will almost always wrap an AggregateException around any exception thrown by the async operation.
						// Is this just a wrapped exception?
						AggregateException flattenedAggregate = eWaitForTask.Flatten();
						if (flattenedAggregate.InnerExceptions.Count != 1)
							throw; // Nope, genuine aggregate.

						// Yep, so rethrow (preserving original stack-trace).
						ExceptionDispatchInfo
							.Capture(
								flattenedAggregate
									.InnerExceptions[0]
							)
							.Throw();

						throw; // Never reached.
					}
				}
			}
			finally
			{
				SetSynchronizationContext(savedContext);
			}
		}

		/// <summary>
		///     Dispose of resources being used by the synchronisation context.
		/// </summary>
		public void Dispose()
		{
			if (this._workItemQueue != null)
			{
				this._workItemQueue.Dispose();
				this._workItemQueue = null;
			}
		}

		/// <summary>
		/// Dispatch an asynchronous message to the synchronization context.
		/// </summary>
		/// <param name="callback">
		/// The <see cref="SendOrPostCallback"/> delegate to call in the synchronisation context.
		/// </param>
		/// <param name="callbackState">
		/// Optional state data passed to the callback.
		/// </param>
		/// <exception cref="InvalidOperationException">
		/// The message pump has already been started, and then terminated by calling <see cref="TerminateMessagePump"/>.
		/// </exception>
		public override void Post(SendOrPostCallback callback, object callbackState)
		{
			if (callback == null)
				throw new ArgumentNullException(nameof(callback));

			this.CheckDisposed();

			try
			{
				this._workItemQueue.Add(
					new KeyValuePair<SendOrPostCallback, object>(callback, callbackState
						)
					);
			}
			catch (InvalidOperationException eMessagePumpAlreadyTerminated)
			{
				throw new InvalidOperationException(
					"Cannot enqueue the specified callback because the synchronisation context's message pump has already been terminated.", 
					eMessagePumpAlreadyTerminated
					);
			}
		}

		/// <summary>
		///     Run the message pump for the callback queue on the current thread.
		/// </summary>
		public void RunMessagePump()
		{
			this.CheckDisposed();

			KeyValuePair<SendOrPostCallback, object> workItem;
			while (this._workItemQueue.TryTake(out workItem, Timeout.InfiniteTimeSpan))
			{
				workItem.Key(workItem.Value);

				// Has the synchronisation context been disposed?
				if (this._workItemQueue == null)
					break;
			}
		}

		/// <summary>
		///     Terminate the message pump once all callback have completed.
		/// </summary>
		public void TerminateMessagePump()
		{
			this.CheckDisposed();

			this._workItemQueue.CompleteAdding();
		}

		/// <summary>
		///     Check if the synchronisation context has been disposed.
		/// </summary>
		private void CheckDisposed()
		{
			if (this._workItemQueue == null)
				throw new ObjectDisposedException(this.GetType().Name);
		}
	}
}
namespace Clix.QA.Selenium
{
	using System;

	/// <summary>
	///     The FilterControl interface.
	/// </summary>
	public interface IFilterControl
	{
		/// <summary>
		///     Activates the current filter to allow filtering operations.
		/// </summary>
		void Activate();

		/// <summary>
		///     Clears the current filter.
		/// </summary>
		/// <returns>
		///     The <see cref="bool" />, true when successfully clears the filter.
		/// </returns>
		bool Clear();

		/// <summary>
		/// Clears the current filter.
		/// </summary>
		/// <param name="timeout">
		/// The timeout.
		/// </param>
		/// <returns>
		/// The <see cref="bool"/>, true when successfully clears the filter.
		/// </returns>
		bool Clear(TimeSpan timeout);

		/// <summary>
		/// Submit the filtering request.
		/// </summary>
		/// <param name="timeout">
		/// The timeout.
		/// </param>
		/// <returns>
		/// Return true when successfully perform the filtering request.
		/// </returns>
		bool Filter(TimeSpan timeout);

		/// <summary>
		/// Submit the filtering request, and wait for the timeout in milliseconds.
		/// </summary>
		/// <param name="timeout">
		/// The timeout in milliseconds.
		/// </param>
		/// <returns>
		/// Return true when successfully perform the filtering request.
		/// </returns>
		bool Filter(int timeout);
	}
}
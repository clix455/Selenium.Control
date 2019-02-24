namespace Clix.QA.Selenium
{
	using System;

	using OpenQA.Selenium;

	/// <summary>
	///     The filter control.
	/// </summary>
	public abstract class FilterControl : ElementControl, IFilterControl
	{
		/// <summary>
		///     The table header control.
		/// </summary>
		private readonly TableHeaderControl tableHeaderControl;

		/// <summary>
		/// Initializes a new instance of the <see cref="FilterControl"/> class.
		/// </summary>
		/// <param name="parent">
		/// The parent.
		/// </param>
		/// <param name="tableHeaderControl">
		/// The table header control.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// Thrown when <see cref="TableHeaderControl"/> not instantiated.
		/// </exception>
		protected FilterControl(IControl parent, TableHeaderControl tableHeaderControl)
			: base(parent)
		{
			if (tableHeaderControl == null)
				throw new ArgumentNullException(nameof(tableHeaderControl));

			this.tableHeaderControl = tableHeaderControl;
		}

		/// <summary>
		///     Gets the filter animation container.
		/// </summary>
		protected virtual IWebElement FilterContainer =>
			this.Driver.FindElement(By.CssSelector("body > div.k-animation-container > form[style*='display: block']"));

		/// <summary>
		///     Gets the cancel button.
		/// </summary>
		protected virtual IWebElement ResetButton =>
			this.FilterContainer.FindElement(By.CssSelector("button.k-button[type='reset']"));

		/// <summary>
		///     Gets the submit button.
		/// </summary>
		protected virtual IWebElement SubmitButton =>
			this.FilterContainer.FindElement(By.CssSelector("button.k-button[type='submit']"));

		/// <summary>
		///     Activates the current filter to allow filtering operations.
		/// </summary>
		public virtual void Activate()
		{
			this.tableHeaderControl.FilterStatus = FilterStatus.On;
		}

		/// <summary>
		///     Clears the current filter.
		/// </summary>
		/// <returns>
		///     The <see cref="bool" />, true when successfully clears the filter.
		/// </returns>
		public virtual bool Clear()
		{
			return this.Clear(TimeSpan.FromSeconds(1));
		}

		/// <inheritdoc/>
		public virtual bool Clear(TimeSpan timeout)
		{
			this.Activate();

			// Wait for the filter control to load.
			Playback.Wait(timeout);

			IWebElement clearButton = this.ResetButton;
			clearButton.WaitClickable();
			clearButton.Click();
			return true;
		}

		/// <summary>
		/// Submit the filtering request, and wait for the timeout.
		/// </summary>
		/// <param name="timeout">
		/// The timeout.
		/// </param>
		/// <returns>
		/// Return true when successfully perform the filtering request.
		/// </returns>
		public bool Filter(TimeSpan timeout)
		{
			IWebElement filterButton = this.SubmitButton;
			filterButton.WaitClickable();
			filterButton.Click();
			Playback.Wait(timeout);
			return true;
		}

		/// <summary>
		/// Submit the filtering request, and wait for the timeout in milliseconds.
		/// </summary>
		/// <param name="timeout">
		/// Wait for the timeout period to ensure grid reloaded after filtering.
		/// </param>
		/// <returns>
		/// The <see cref="bool"/>, true when successfully submits the filtering request.
		/// </returns>
		public virtual bool Filter(int timeout = 2000)
		{
			return this.Filter(TimeSpan.FromMilliseconds(timeout));
		}
	}
}
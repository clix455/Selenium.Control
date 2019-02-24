namespace Clix.QA.Selenium
{
	using System;

	using OpenQA.Selenium;

	/// <summary>
	///     The table header control.
	/// </summary>
	public class TableHeaderControl : ElementControl
	{
		/// <summary>
		///     The filter status.
		/// </summary>
		private FilterStatus filterStatus = FilterStatus.None;

		/// <summary>
		/// Initializes a new instance of the <see cref="TableHeaderControl"/> class.
		/// </summary>
		/// <param name="wrappedElement">
		/// The web element.
		/// </param>
		/// <param name="parent">
		/// The parent.
		/// </param>
		public TableHeaderControl(IWebElement wrappedElement, ElementControl parent)
			: base(wrappedElement, parent)
		{
		}

		/// <summary>
		///     Gets or sets the display name.
		/// </summary>
		public string DisplayName { get; set; }

		/// <summary>
		///     Gets or sets the filter status.
		/// </summary>
		/// <exception cref="InvalidOperationException">
		///     Thrown when trying to set <see cref="FilterStatus" /> to None or Unknown explicitly.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///     Thrown when <see cref="FilterStatus" /> is out of range.
		/// </exception>
		public virtual FilterStatus FilterStatus
		{
			get
			{
				if (this.filterStatus == FilterStatus.None)
					return this.filterStatus;

				IWebElement filterToggleElement = this.FindElement(By.CssSelector("a.k-grid-filter"));

				if (filterToggleElement == null)
					return FilterStatus.None;

				return filterToggleElement.GetAttribute("class").Contains("k-state-border-down")
							? FilterStatus.On
							: FilterStatus.Off;
			}

			set
			{
				if (value == this.FilterStatus)
					return;

				if (value == FilterStatus.Unknow || value == FilterStatus.None)
					throw new InvalidOperationException("It is not allowed to explicitly set filter status to either Unknow or None.");

				this.filterStatus = FilterStatus.Unknow;

				IWebElement filterToggleElement = this.FindElement(By.CssSelector("a.k-grid-filter"));

				switch (value)
				{
					case FilterStatus.On:
						{
							filterToggleElement.WaitClickable();
							filterToggleElement.Click();
							this.WaitForCondition(
								tableHeader =>
								tableHeader.FilterStatus == FilterStatus.On,
								PlaybackSettings.Instance.ExplicitlyWaitSecond * 1000);

							break;
						}

					case FilterStatus.Off:
						{
							filterToggleElement.WaitClickable();
							filterToggleElement.Click();
							this.WaitForCondition(
								tableHeader =>
								tableHeader.FilterStatus == FilterStatus.Off,
								PlaybackSettings.Instance.ExplicitlyWaitSecond * 1000);
							break;
						}

					default:
						throw new ArgumentOutOfRangeException(nameof(value), value, null);
				}

				// Set the filter status back field after successfully interacts with the UI control.
				this.filterStatus = value;
			}
		}

		/// <summary>
		/// Gets or sets the table header position.
		/// </summary>
		/// <remarks>
		/// The header position is 1-based.
		/// </remarks>
		public int Position { get; set; }
	}
}
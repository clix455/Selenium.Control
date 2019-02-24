namespace Clix.QA.Selenium
{
	using System;

	using OpenQA.Selenium;

	/// <summary>
	///     The free text filter.
	/// </summary>
	public class FreeTextFilter : FilterControl
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FreeTextFilter"/> class.
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
		public FreeTextFilter(IControl parent, TableHeaderControl tableHeaderControl)
			: base(parent, tableHeaderControl)
		{
		}

		/// <summary>
		///     Gets or sets the operator.
		/// </summary>
		public string Operator
		{
			get
			{
				IWebElement operatorElement = this.OperatorElement;
				IWebElement operatorValuElement = operatorElement.FindElement(By.CssSelector("span > span.k-input"));
				return operatorValuElement != null
							? operatorValuElement.Text
							: string.Empty;
			}

			set
			{
				var textDropDownControl = new TextDropDownControl
				{
					ToggleElement = this.OperatorElement,
					Expanded = true,
					ListElement = this.FilterContainer.FindElement(By.CssSelector("div.k-animation-container > div > div > ul"))
				};
				textDropDownControl.SelectByText(value);
			}
		}

		/// <summary>
		///     Gets or sets the value.
		/// </summary>
		public virtual string Value
		{
			get
			{
				IWebElement inputElement = this.InputElement;

				return inputElement.Text;
			}

			set
			{
				IWebElement inputElement = this.InputElement;
				inputElement.WaitClickable();
				inputElement.Clear();
				inputElement.SendKeys(value);
			}
		}

		/// <summary>
		///     Gets the input element.
		/// </summary>
		private IWebElement InputElement => this.FilterContainer.FindElement(By.CssSelector("input.k-textbox"));

		/// <summary>
		///     Gets the operator element.
		/// </summary>
		private IWebElement OperatorElement => this.FilterContainer.FindElement(By.CssSelector("span.k-dropdown"));
	}
}
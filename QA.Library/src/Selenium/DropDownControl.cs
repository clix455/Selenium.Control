namespace Clix.QA.Selenium
{
	using System;
	using System.Collections.ObjectModel;
	using System.Linq;

	using OpenQA.Selenium;

	/// <summary>
	/// The drop down control.
	/// </summary>
	public abstract class DropDownControl : IDropDownControl
	{
		/// <summary>
		/// Gets or sets the list element.
		/// </summary>
		public virtual IWebElement ListElement { get; set; }

		/// <summary>
		/// Gets or sets the toggle element.
		/// </summary>
		public virtual IWebElement ToggleElement { get; set; }

		/// <inheritdoc />
		public virtual bool Expanded
		{
			get
			{
				string expandingAttribute = this.ToggleElement.GetAttribute("aria-expanded");
				return expandingAttribute != null
					&& expandingAttribute.Equals("true", StringComparison.CurrentCultureIgnoreCase);
			}

			set
			{
				// Nothing to do
				if (value == this.Expanded)
					return;

				this.ToggleElement.WaitClickable().Click();
				WaitConditions.WaitForCondition(
					() => this.Expanded.Equals(value),
					PlaybackSettings.Instance.ExplicitlyWaitSecond * 1000);
			}
		}

		/// <summary>
		/// Gets the options.
		/// </summary>
		protected virtual ReadOnlyCollection<IWebElement> Options => this.ListElement.FindElements(By.CssSelector("li"));

		/// <inheritdoc />
		public virtual void SelectByIndex(int index)
		{
			this.Expanded = true;
			IWebElement matchingElement = this.Options[index];
			matchingElement.WaitClickable();
			matchingElement.Click();
		}

		/// <inheritdoc />
		public virtual void SelectByText(string text)
		{
			this.Expanded = true;

			IWebElement matchingElement = null;
			WaitConditions.WaitForCondition(
				() =>
				{
					matchingElement =
						this.Options.FirstOrDefault(
							webElement =>
							webElement.Text.Trim().Equals(text, StringComparison.CurrentCultureIgnoreCase));

					return matchingElement != null;
				},
				5000);

			if (matchingElement == null)
				throw new NoSuchElementException($"Cannot locate option with text: {text}");

			matchingElement.WaitClickable();
			matchingElement.Click();
		}
	}
}
namespace Clix.QA.Selenium
{
	using System;
	using System.Collections.ObjectModel;
	using System.Linq;

	using OpenQA.Selenium;

	/// <summary>
	///     The radio button group control.
	/// </summary>
	public class RadioButtonGroupControl : ElementControl
	{
		/// <summary>
		/// The radio button elements.
		/// </summary>
		private readonly ReadOnlyCollection<IWebElement> radioButtonElements;

		/// <summary>
		/// Initializes a new instance of the <see cref="RadioButtonGroupControl"/> class.
		/// </summary>
		/// <param name="radioButtonElements">
		/// The radio Button Elements.
		/// </param>
		/// <param name="parent">
		/// The parent.
		/// </param>
		public RadioButtonGroupControl(ReadOnlyCollection<IWebElement> radioButtonElements, IControl parent)
			: base(parent)
		{
			if (radioButtonElements == null)
				throw new ArgumentNullException(nameof(radioButtonElements));
			this.radioButtonElements = radioButtonElements;
		}

		/// <summary>
		/// Selects radio button by value.
		/// </summary>
		/// <param name="value">
		/// The value to be selected.
		/// </param>
		/// <param name="valueAttribute">
		/// The value attribute.
		/// </param>
		public void Select(string value, string valueAttribute = "value")
		{
			if (string.IsNullOrEmpty(value))
				throw new ArgumentException("value cannot be null or empty");

			IWebElement selectedElement = this.radioButtonElements.FirstOrDefault(item => item.Selected);

			if (selectedElement != null)
			{
				string currentValue = selectedElement.GetAttribute(valueAttribute);
				if (currentValue.Equals(value, StringComparison.CurrentCultureIgnoreCase))
					return;
			}

			IWebElement candidateElement =
				this.radioButtonElements.FirstOrDefault(
					item =>
					{
						string itemValue = item.GetAttribute(valueAttribute);
						return itemValue.Equals(value, StringComparison.CurrentCultureIgnoreCase);
					});

			if (candidateElement == null)
				return;

			candidateElement.WaitClickable();
			candidateElement.Click();
		}

		/// <summary>
		/// Selects radio button by index.
		/// </summary>
		/// <param name="index">
		/// The zero based index.
		/// </param>
		public void Select(int index)
		{
			IWebElement candidateElement = this.radioButtonElements[index];

			candidateElement.WaitClickable();
			candidateElement.Click();
		}
	}
}
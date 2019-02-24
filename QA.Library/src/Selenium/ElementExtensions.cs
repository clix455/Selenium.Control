namespace Clix.QA.Selenium
{
	using System;

	using OpenQA.Selenium;
	using OpenQA.Selenium.Support.UI;

	/// <summary>
	///     Define extension methods for web elements.
	/// </summary>
	public static class ElementExtensions
	{
		/// <summary>
		/// Select the option by the position.
		/// </summary>
		/// <param name="selectElement">
		/// The select element.
		/// </param>
		/// <param name="position">
		/// The position.
		/// </param>
		public static void SelectByPosition(this SelectElement selectElement, int position)
		{
			if (selectElement == null)
				throw new ArgumentException("The select element cannot be null", nameof(selectElement));
			if (position > selectElement.Options.Count)
				throw new IndexOutOfRangeException("The option position is out of the options range.");
			IWebElement option = selectElement.Options[position - 1]; // Convert to zero-based collection index.

			if (option.Selected)
				return;
			option.Click();
		}
	}
}
namespace Clix.QA.Selenium
{
	using OpenQA.Selenium;

	using Validation;

	/// <summary>
	///     The web control extensions.
	/// </summary>
	public static class WebControlExtensions
	{
		/// <summary>
		/// Clear and fill out input.
		/// </summary>
		/// <param name="control">
		/// The web control, can be a page control, or an element control.
		/// </param>
		/// <param name="locator">
		/// The web element locator.
		/// </param>
		/// <param name="input">
		/// The input.
		/// </param>
		public static void ClearAndFilloutInput(this IControl control, By locator, string input)
		{
			Requires.NotNull(locator, nameof(control));
			Requires.NotNull(locator, nameof(locator));
			IWebElement element = control.FindElement(locator).WaitClickable();

			element.ClearAndSendKeys(input);
		}

		/// <summary>
		/// Clicks on the element found via the <see cref="By"/> locator.
		/// </summary>
		/// <param name="control">
		/// The web control that contains the element.
		/// </param>
		/// <param name="locator">
		/// The locator.
		/// </param>
		public static void Click(this IControl control, By locator)
		{
			Requires.NotNull(locator, nameof(control));
			Requires.NotNull(locator, nameof(locator));
			IWebElement element = control.FindElement(locator).WaitClickable();
			element.Click();
		}

		/// <summary>
		/// Gets the trimmed text from element identified by the <see cref="By"/> locator.
		/// </summary>
		/// <param name="control">
		/// The web control, can be a page control, or an element control.
		/// </param>
		/// <param name="locator">
		/// The web element locator.
		/// </param>
		/// <returns>
		/// The text of the specified element.
		/// </returns>
		public static string GetText(this IControl control, By locator)
		{
			Requires.NotNull(locator, nameof(control));
			Requires.NotNull(locator, nameof(locator));
			IWebElement element = control.FindElement(locator).WaitClickable();

			return element.Text.Trim();
		}
	}
}

namespace Clix.QA.Selenium
{
	using OpenQA.Selenium;

	using Validation;

	/// <summary>
	/// Define extesnsion methods for <see cref="IWebElement"/>
	/// </summary>
	public static class WebElementExtensions
	{
		/// <summary>
		/// Clear existing content, and type in the new text specified by <paramref name="text"/>.
		/// </summary>
		/// <param name="webElement">
		/// The <see cref="IWebElement"/> represents the text field or text box.
		/// </param>
		/// <param name="text">The text to type into the element.</param>
		/// <remarks>
		/// The text to be typed may include special characters like arrow keys,
		/// backspaces, function keys, and so on. Valid special keys are defined in
		/// <see cref="T:OpenQA.Selenium.Keys"/>.
		/// </remarks>
		public static void ClearAndSendKeys(this IWebElement webElement, string text)
		{
			Requires.NotNull(webElement, nameof(webElement));
			webElement.Clear();
			if (string.IsNullOrEmpty(text))
			{
				webElement.SendKeys("-");
				webElement.SendKeys(Keys.Backspace);
				return;
			}

			webElement.SendKeys(text);
		}
	}
}

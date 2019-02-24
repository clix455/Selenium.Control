namespace Clix.QA.Selenium
{
	using System.Collections.ObjectModel;

	using OpenQA.Selenium;
	using OpenQA.Selenium.Support.UI;

	/// <summary>
	/// The Control interface.
	/// </summary>
	public interface IControl
	{
		/// <summary>
		/// Gets the driver.
		/// </summary>
		IWebDriver Driver { get; }

		/// <summary>
		/// Finds the first <see cref="T:OpenQA.Selenium.IWebElement"/> using the given method.
		/// </summary>
		/// <param name="by">
		/// The locating mechanism to use.
		/// </param>
		/// <returns>
		/// The first matching <see cref="T:OpenQA.Selenium.IWebElement"/> on the current context.
		/// </returns>
		/// <exception cref="T:OpenQA.Selenium.NoSuchElementException">
		/// If no element matches the criteria.
		/// </exception>
		IWebElement FindElement(By by);

		/// <summary>
		/// Finds all <see cref="T:OpenQA.Selenium.IWebElement">IWebElements</see> within the current control using the given
		///     mechanism.
		/// </summary>
		/// <param name="by">
		/// The locating mechanism to use.
		/// </param>
		/// <returns>
		/// A <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1"/> of all
		///     <see cref="T:OpenQA.Selenium.IWebElement">WebElements</see>
		///     matching the current criteria, or an empty list if nothing matches.
		/// </returns>
		ReadOnlyCollection<IWebElement> FindElements(By by);

		/// <summary>
		/// Wait until the current control completes loading.
		/// </summary>
		/// <remarks>
		/// The web control can be either a page, or an element control.
		/// </remarks>
		void WaitLoading();
	}
}
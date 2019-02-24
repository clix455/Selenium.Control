namespace Clix.QA.Selenium
{
	using OpenQA.Selenium;
	using OpenQA.Selenium.Edge;

	/// <summary>
	/// The web driver factory for <see cref="EdgeDriver"/>.
	/// </summary>
	internal class WebDriverFactoryEdge : WebDriverFactory
	{
		/// <summary>
		/// Create an instance of <see cref="EdgeDriver"/>.
		/// </summary>
		/// <returns>
		/// An instance of <see cref="EdgeDriver"/>.
		/// </returns>
		protected override IWebDriver CreateWebDriver()
		{
			EdgeOptions options = new EdgeOptions();
			var service = EdgeDriverService.CreateDefaultService();
			var driver = new EdgeDriver(service, options);
			return driver;
		}
	}
}

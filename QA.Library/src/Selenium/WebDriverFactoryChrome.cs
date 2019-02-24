namespace Clix.QA.Selenium
{
	using OpenQA.Selenium;
	using OpenQA.Selenium.Chrome;

	/// <summary>
	/// The web driver factory chrome.
	/// </summary>
	internal class WebDriverFactoryChrome : WebDriverFactory
	{
		/// <summary>
		/// Create a chrome web driver.
		/// </summary>
		/// <returns>
		/// The <see cref="IWebDriver"/>.
		/// </returns>
		protected override IWebDriver CreateWebDriver()
		{
			var options = new ChromeOptions();
			options.AddArguments("test-type");
			options.AddArgument("--start-maximized");

			options.AddArguments("--incognito");
			return new ChromeDriver(options);
		}
	}
}

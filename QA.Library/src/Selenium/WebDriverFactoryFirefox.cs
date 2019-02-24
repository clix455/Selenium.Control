namespace Clix.QA.Selenium
{
	using OpenQA.Selenium;
	using OpenQA.Selenium.Firefox;

	/// <summary>
	/// The web driver factory firefox.
	/// </summary>
	internal class WebDriverFactoryFirefox : WebDriverFactory
	{
		/// <summary>
		/// Create a firefox web driver.
		/// </summary>
		/// <returns>
		/// The <see cref="IWebDriver"/>.
		/// </returns>
		protected override IWebDriver CreateWebDriver()
		{
			FirefoxOptions options = new FirefoxOptions();
			options.AddArgument("--start-maximized");
			options.SetPreference("browser.privatebrowsing.autostart", true);
			IWebDriver driver = new FirefoxDriver(options);
			driver.Manage().Window.FullScreen();
			return driver;
		}
	}
}

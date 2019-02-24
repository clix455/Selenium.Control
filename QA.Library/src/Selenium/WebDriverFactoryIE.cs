namespace Clix.QA.Selenium
{
	using OpenQA.Selenium;
	using OpenQA.Selenium.IE;

	/// <summary>
	/// The web driver factory ie.
	/// </summary>
	public class WebDriverFactoryIE : WebDriverFactory
	{
		/// <summary>
		/// Create an IE web driver.
		/// </summary>
		/// <returns>
		/// The <see cref="IWebDriver"/>.
		/// </returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Reviewed.")]
		protected override IWebDriver CreateWebDriver()
		{
			InternetExplorerOptions options = new InternetExplorerOptions();

			options.BrowserCommandLineArguments += " -private";
            options.AddAdditionalCapability("ignoreZoomSetting", true);
            return	new InternetExplorerDriver(
					InternetExplorerDriverService.CreateDefaultService(),
					options);
		}
	}
}

namespace Clix.QA.Selenium
{
	using System.Collections.Specialized;
	using System.Configuration;

	using Utilities;

	/// <summary>
	///     The playback settings.
	/// </summary>
	public class PlaybackSettings
	{
		/// <summary>
		///     Prevents a default instance of the <see cref="PlaybackSettings" /> class from being created.
		/// </summary>
		private PlaybackSettings()
		{
			this.Initialize();
		}

		/// <summary>
		///     Gets the instance.
		/// </summary>
		public static PlaybackSettings Instance { get; } = new PlaybackSettings();

		/// <summary>
		/// Gets or sets the base url.
		/// </summary>
		public string BaseUrl { get; set; }

		/// <summary>
		///     Gets or sets the browser type.
		/// </summary>
		public string BrowserType { get; set; }

		/// <summary>
		///     Gets or sets the explicitly waiting timeout in second.
		/// </summary>
		public int ExplicitlyWaitSecond { get; set; }

		/// <summary>
		///     Gets or sets the implicitly wait second.
		/// </summary>
		public int ImplicitlyWaitSecond { get; set; }

		/// <summary>
		///     Gets or sets the page load timeout second.
		/// </summary>
		public int PageLoadTimeoutSecond { get; set; }

		/// <summary>
		///     Gets or sets the polling interval.
		/// </summary>
		/// <remarks> Polling interval is in millisecond. </remarks>
		public int PollingInterval { get; set; }

		/// <summary>
		///     Gets or sets the Selenium Grid Hub url.
		/// </summary>
		/// <remarks> Selenium Grid Hub url. </remarks>
		public string SeleniumGridUrl { get; set; }

		/// <summary>
		/// Gets or sets the web driver configuration file name.
		/// </summary>
		public string WebDriverConfigurationFile { get; set; }

		/// <summary>
		///     Gets or sets wait time multiplier.
		/// </summary>
		/// <remarks>
		///     Wait time multiplier default value to 1. During Playback.Wait it will wait for the ThinkTime Interval multiplied by
		///     the wait time multiplier.
		/// </remarks>
		public double WaitTimeMultiplier { get; set; } = 1;

		/// <summary>
		///     The initialize.
		/// </summary>
		public void Initialize()
		{
			this.AppSettings(ConfigurationManager.AppSettings);
		}

		/// <summary>
		/// Set up application settings.
		/// </summary>
		/// <param name="appSettings">
		/// The app settings.
		/// </param>
		private void AppSettings(NameValueCollection appSettings)
		{
			this.BaseUrl =
				SettingsHelper.GetConfigOptionValue(
					"BaseUrl",
					"http://localhost",
					appSettings);

			this.WebDriverConfigurationFile
				= SettingsHelper.GetConfigOptionValue(
					"WebDriverConfigurationFile",
					"webDriver.json",
					appSettings);

			this.BrowserType =
				SettingsHelper.GetConfigOptionValue(
					"BrowserType",
					"chrome",
					appSettings);

			this.ImplicitlyWaitSecond =
				SettingsHelper.GetConfigOptionValueInt(
					"ImplicitlyWaitSecond",
					30,
					appSettings);

			this.ExplicitlyWaitSecond =
				SettingsHelper.GetConfigOptionValueInt(
					"ExplicitlyWaitSecond",
					10,
					appSettings);

			this.PageLoadTimeoutSecond =
				SettingsHelper.GetConfigOptionValueInt(
					"PageLoadTimeoutSecond",
					60,
					appSettings);

			this.PollingInterval =
				SettingsHelper.GetConfigOptionValueInt(
					"PollingInterval",
					250,
					appSettings);

			this.WaitTimeMultiplier =
				SettingsHelper.GetConfigOptionValueDouble(
					"WaitTimeMultiplier",
					1d,
					appSettings);

			this.SeleniumGridUrl =
				SettingsHelper.GetConfigOptionValue(
					"SeleniumGridUrl",
					"http://localhost:4444/wd/hub/",
					appSettings);
		}
	}
}
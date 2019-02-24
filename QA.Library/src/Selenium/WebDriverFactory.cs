namespace Clix.QA.Selenium
{
	using System;
	using System.Collections.Generic;

	using OpenQA.Selenium;

	/// <summary>
	///     The web driver factory.
	/// </summary>
	public abstract class WebDriverFactory
	{
		/// <summary>
		///     The default driver token.
		/// </summary>
		public static readonly Guid DefaultDriverToken = new Guid("C100EE9B-60D3-4776-AA94-6F4E59F424E2");

		/// <summary>
		///     The drivers.
		/// </summary>
		private readonly Dictionary<Guid, IWebDriver> drivers = new Dictionary<Guid, IWebDriver>();

		/// <summary>
		/// Indicating whether the specified driver token exists.
		/// </summary>
		/// <param name="driverToken">
		/// The web driver token identifies the driver.
		/// </param>
		/// <returns>
		/// True if the current driver factor has the specified driver; otherwise false.
		/// </returns>
		public bool Has(Guid driverToken)
		{
			return this.drivers.ContainsKey(driverToken);
		}

		/// <summary>
		/// Quits a web driver via its driver token.
		/// </summary>
		/// <param name="driverToken">
		/// The driver token.
		/// </param>
		public void QuitDriver(Guid driverToken)
		{
			if (!this.Has(driverToken))
			{
				return;
			}

			IWebDriver driver = this.drivers[driverToken];

			driver.Quit();
			this.drivers.Remove(driverToken);
		}

		/// <summary>
		/// Close all the driver instances.
		/// </summary>
		public void QuitAllDrivers()
		{
			foreach (var driverRecord in this.drivers)
			{
				driverRecord.Value.Quit();
			}

			this.drivers.Clear();
		}

		/// <summary>
		///     Get the default web driver.
		/// </summary>
		/// <returns>
		///     The <see cref="IWebDriver" />.
		/// </returns>
		public IWebDriver GetWebDriver()
		{
			return this.GetWebDriver(DefaultDriverToken);
		}

		/// <summary>
		///     Get the default event firing web driver.
		/// </summary>
		/// <param name="driverToken">
		///     The driver Token.
		/// </param>
		/// <returns>
		///     The <see cref="IWebDriver" />.
		/// </returns>
		public IWebDriver GetWebDriver(Guid driverToken)
		{
			if (this.Has(driverToken))
				return this.drivers[driverToken];

			IWebDriver webDriver = this.CreateWebDriver(driverToken);
			ConfigureWebDriver(webDriver);
			return webDriver;
		}

		/// <summary>
		///     Get web driver.
		/// </summary>
		/// <returns>
		///     The <see cref="IWebDriver" />.
		/// </returns>
		protected abstract IWebDriver CreateWebDriver();

		/// <summary>
		///     Get web driver.
		/// </summary>
		/// <param name="driverToken">
		///     The driver Token.
		/// </param>
		/// <returns>
		///     The <see cref="IWebDriver" />.
		/// </returns>
		protected IWebDriver CreateWebDriver(Guid driverToken)
		{
			IWebDriver webDriver = this.CreateWebDriver();
			this.drivers.Add(driverToken, webDriver);
			return webDriver;
		}

		/// <summary>
		///     Configure web driver.
		/// </summary>
		/// <param name="driver">
		///     The driver.
		/// </param>
		private static void ConfigureWebDriver(IWebDriver driver)
		{
			if (driver == null)
				throw new ArgumentNullException(nameof(driver));

			driver
				.SetImplicitWait(TimeSpan.FromSeconds(PlaybackSettings.Instance.ImplicitlyWaitSecond));

			driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(PlaybackSettings.Instance.PageLoadTimeoutSecond);
		}
	}
}

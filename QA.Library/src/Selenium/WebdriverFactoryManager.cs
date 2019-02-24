namespace Clix.QA.Selenium
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Autofac.Features.Indexed;

	/// <summary>
	///     Manage the web driver factory instances per browser type.
	/// </summary>
	public class WebdriverFactoryManager
	{
		/// <summary>
		///     Manage the in use browser types.
		/// </summary>
		private readonly IList<string> browserTypesInuse = new List<string>();

		/// <summary>
		///     The indexed web driver factories.
		/// </summary>
		private readonly IIndex<string, WebDriverFactory> factories;

		/// <summary>
		/// Initializes a new instance of the <see cref="WebdriverFactoryManager"/> class.
		/// </summary>
		/// <param name="factories">
		/// The registered factory list.
		/// </param>
		public WebdriverFactoryManager(IIndex<string, WebDriverFactory> factories)
		{
			this.factories = factories;
		}

		/// <summary>
		/// Quits all managed web driver instances.
		/// </summary>
		public void QuitAllDrivers()
		{
			foreach (string browserType in this.browserTypesInuse)
			{
				WebDriverFactory webDriverFactory = this.factories[browserType];
				webDriverFactory.QuitAllDrivers();
			}
		}

		/// <summary>
		/// Gets web driver factory per browser type.
		/// </summary>
		/// <returns>
		/// Web driver factory for the browser type.
		/// </returns>
		public WebDriverFactory ResolveWebDriverFactory()
		{
			return this.ResolveWebDriverFactory(null);
		}

		/// <summary>
		/// Gets web driver factory per browser type.
		/// </summary>
		/// <param name="browserType">
		/// Browser type.
		/// </param>
		/// <returns>
		/// Web driver factory for the browser type.
		/// </returns>
		public WebDriverFactory ResolveWebDriverFactory(string browserType)
		{
			if (string.IsNullOrEmpty(browserType))
			{
				browserType = PlaybackSettings.Instance.BrowserType;
			}

			WebDriverFactory webDriverFactory = this.factories[browserType];

			this.SetBrowserTypesInUse(browserType);

			return webDriverFactory;
		}

		/// <summary>
		/// Sets browser types in use.
		/// </summary>
		/// <param name="browserType">
		/// The browser type.
		/// </param>
		private void SetBrowserTypesInUse(string browserType)
		{
			if (!this.browserTypesInuse.Contains(browserType, StringComparer.OrdinalIgnoreCase))
				this.browserTypesInuse.Add(browserType);
		}
	}
}
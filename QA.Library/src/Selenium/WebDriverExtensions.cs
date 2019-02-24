namespace Clix.QA.Selenium
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Net;

	using Autofac;

	using OpenQA.Selenium;

	using Cookie = OpenQA.Selenium.Cookie;

	/// <summary>
	///     The web driver extensions.
	/// </summary>
	public static class WebDriverExtensions
	{
		/// <summary>
		/// Gets auth cookies from current <see cref="IWebDriver"/>.
		/// </summary>
		/// <param name="driver">
		/// The driver.
		/// </param>
		/// <returns>
		/// The <see cref="CookieCollection"/> containing all the FedAuth cookies.
		/// </returns>
		public static CookieCollection GetAuthCookies(this IWebDriver driver)
		{
			if (driver == null)
				throw new ArgumentNullException(nameof(driver));
			var authCookies = new CookieCollection();
			ReadOnlyCollection<Cookie> cookies = driver.Manage().Cookies.AllCookies;
			IEnumerable<Cookie> fedAuthCookies =
				cookies.Where(cookie =>
							cookie.Name.StartsWith("FedAuth", StringComparison.CurrentCultureIgnoreCase));

			foreach (var cookie in fedAuthCookies)
			{
				authCookies.Add(new System.Net.Cookie(cookie.Name, cookie.Value, cookie.Path, cookie.Domain));
			}

			return authCookies;
		}

		/// <summary>
		/// Load a new web page in the current browser window.
		/// </summary>
		/// <param name="driver">
		/// The driver.
		/// </param>
		/// <param name="url">
		/// The URL to load. It is best to use a fully qualified URL.
		/// </param>
		/// <param name="uriKind">
		/// The uri kind.
		/// </param>
		/// <param name="ensureNavigation">
		/// The ensure Navigation.
		/// </param>
		public static void GoToUrl(this IWebDriver driver, string url, UriKind uriKind = UriKind.Relative, bool ensureNavigation = true)
		{
			if (driver == null)
			{
				throw new ArgumentNullException(nameof(driver));
			}

			if (string.IsNullOrEmpty(url))
			{
				throw new ArgumentException("url cannot be null or empty");
			}

			Uri absoluteUri = null;
			switch (uriKind)
			{
				case UriKind.Absolute:
					Uri.TryCreate(url, UriKind.Absolute, out absoluteUri);
					break;
				case UriKind.Relative:
					{
						if (!Uri.IsWellFormedUriString(PlaybackSettings.Instance.BaseUrl, UriKind.Absolute))
						{
							throw new InvalidOperationException("The web site base uri is not well formatted");
						}

						var baseUri = new Uri(PlaybackSettings.Instance.BaseUrl);
						Uri.TryCreate(baseUri, url, out absoluteUri);
						break;
					}
			}

			driver.GoToUrl(absoluteUri, ensureNavigation);
		}

		/// <summary>
		/// Load a new web page in the current browser window.
		/// </summary>
		/// <param name="driver">
		/// The driver.
		/// </param>
		/// <param name="uri">
		/// The URL to load.
		/// </param>
		/// <param name="ensureNavigation">
		/// The ensure Navigation. When true, wait for the driver url to starts with the specified uri.
		/// </param>
		public static void GoToUrl(this IWebDriver driver, Uri uri, bool ensureNavigation)
		{
			if (driver == null)
			{
				throw new ArgumentNullException(nameof(driver));
			}

			if (uri == null)
			{
				throw new ArgumentNullException(nameof(uri));
			}

			if (driver.Url.StartsWith(uri.AbsoluteUri, StringComparison.InvariantCultureIgnoreCase))
				return;

			WaitConditions.WaitForCondition(
				() =>
				{
					driver.Navigate().GoToUrl(uri);
					return !ensureNavigation
							|| driver.Url.StartsWith(uri.AbsoluteUri, StringComparison.InvariantCultureIgnoreCase);
				});
		}

		/// <summary>
		/// The launch portal.
		/// </summary>
		/// <param name="driver">
		/// The driver.
		/// </param>
		public static void LaunchPortal(this IWebDriver driver)
		{
			if (driver == null)
			{
				throw new ArgumentNullException(nameof(driver));
			}

			driver.GoToUrl(PlaybackSettings.Instance.BaseUrl, UriKind.Absolute, false);
		}

		/// <summary>
		/// Get an instance of <see cref="IWebDriver"/> by its token.
		/// </summary>
		/// <param name="componentContext">
		/// The component context.
		/// </param>
		/// <param name="driverToken">
		/// The driver token.
		/// </param>
		/// <param name="browserType">
		/// The browser type.
		/// </param>
		/// <returns>
		/// The <see cref="IWebDriver"/>.
		/// </returns>
		public static IWebDriver ResolveWebDriver(
			this IComponentContext componentContext,
			Guid driverToken = default(Guid),
			string browserType = null)
		{
			if (componentContext == null)
			{
				throw new ArgumentNullException(nameof(componentContext));
			}

			if (driverToken.Equals(default(Guid)))
			{
				driverToken = WebDriverFactory.DefaultDriverToken;
			}

			if (browserType == null)
			{
				browserType = PlaybackSettings.Instance.BrowserType;
			}

			WebdriverFactoryManager factoryManager = componentContext.Resolve<WebdriverFactoryManager>();
			WebDriverFactory factory = factoryManager.ResolveWebDriverFactory(browserType);

			IWebDriver driver = factory.GetWebDriver(driverToken);

			return driver;
		}
	}
}
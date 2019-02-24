namespace Clix.QA.Selenium
{
	using System;
	using System.Collections.ObjectModel;

	using OpenQA.Selenium;
	using OpenQA.Selenium.Support.PageObjects;
	using OpenQA.Selenium.Support.UI;

	/// <summary>
	///     The base class for page objects.
	/// </summary>
	public abstract class PageBase : IControl
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PageBase"/> class.
		/// </summary>
		/// <param name="driver">
		/// The driver.
		/// </param>
		protected PageBase(IWebDriver driver)
		{
			this.Driver = driver;

			PageFactory.InitElements(driver, this);
		}

		/// <summary>
		///     Gets or sets the driver.
		/// </summary>
		public IWebDriver Driver { get; set; }

		/// <summary>
		/// Gets a value indicating whether indicating if the current loaded page matches this page object definition.
		/// </summary>
		public bool Displaying => this.Driver.Title.Equals(this.Title, StringComparison.CurrentCultureIgnoreCase);

		/// <summary>
		///     Gets the page title.
		/// </summary>
		protected virtual string Title => "default title";

		/// <inheritdoc/>
		public IWebElement FindElement(By @by)
		{
			if (@by == null)
				throw new ArgumentNullException(nameof(@by));
			this.WaitLoading();
			return this.Driver?.FindElement(@by);
		}

		/// <inheritdoc/>
		public ReadOnlyCollection<IWebElement> FindElements(By @by)
		{
			if (@by == null)
				throw new ArgumentNullException(nameof(@by));
			this.WaitLoading();
			return this.Driver?.FindElements(@by);
		}

		/// <summary>
		/// Wait until the current control completes loading.
		/// </summary>
		/// <remarks>
		/// The web control can be either a page, or an element control.
		/// </remarks>
		public virtual void WaitLoading()
		{
			By loadingElementLocator = By.CssSelector(".k-loading-mask:not(#modal-progress)");
			try
			{
				this.Driver.WaitUntil(ExpectedConditions.ElementExists(loadingElementLocator), TimeSpan.FromMilliseconds(100));
			}
			catch (Exception)
			{
				// Go ahead if loading already completed.
			}

			this.Driver.WaitUntil(ExpectedConditions.InvisibilityOfElementLocated(loadingElementLocator));
		}

		/// <summary>
		/// Load a new web page in the current browser window.
		/// </summary>
		/// <param name="url">
		/// The URL to load. It is best to use a fully qualified URL.
		/// </param>
		/// <param name="uriKind">
		/// The uri kind.
		/// </param>
		public void GoToUrl(string url, UriKind uriKind = UriKind.Relative)
		{
			Uri absoluteUri = null;
			switch (uriKind)
			{
				case UriKind.Absolute:
					Uri.TryCreate(url, UriKind.Absolute, out absoluteUri);
					break;
				case UriKind.Relative:
					{
						if (!Uri.IsWellFormedUriString(PlaybackSettings.Instance.BaseUrl, UriKind.Absolute))
							throw new InvalidOperationException("The web site base uri is not well formatted");
						Uri baseUri = new Uri(PlaybackSettings.Instance.BaseUrl);
						Uri.TryCreate(baseUri, url, out absoluteUri);
						break;
					}
			}

			this.GoToUrl(absoluteUri);
		}

		/// <summary>
		/// Load a new web page in the current browser window.
		/// </summary>
		/// <param name="uri">
		/// The URL to load.
		/// </param>
		public void GoToUrl(Uri uri)
		{
			this.Driver.Navigate().GoToUrl(uri);
		}

		/// <summary>
		///     The launch portal.
		/// </summary>
		public void LaunchPortal()
		{
			this.GoToUrl(PlaybackSettings.Instance.BaseUrl, UriKind.Absolute);
		}
	}
}
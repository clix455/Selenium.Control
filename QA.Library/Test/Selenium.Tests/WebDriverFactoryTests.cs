namespace Clix.QA.Selenium.Tests
{
	using System;

	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Autofac;

	using OpenQA.Selenium;

	[TestClass]
	public class WebDriverFactoryTests
	{
		[TestMethod]
		public void SingleInstanceOfWebdriverFactoryForSameBrowser()
		{
			const string BrowserType = "chrome";
			ILifetimeScope scope = Registrar.NewLifetimeScope;
			WebdriverFactoryManager factoryManager = scope.Resolve<WebdriverFactoryManager>();
			WebDriverFactory factory = factoryManager.ResolveWebDriverFactory(BrowserType);

			ILifetimeScope scope2 = Registrar.NewLifetimeScope;
			WebdriverFactoryManager factoryManager2 = scope2.Resolve<WebdriverFactoryManager>();
			WebDriverFactory factory2 = factoryManager2.ResolveWebDriverFactory(BrowserType);

			Assert.IsInstanceOfType(factory, typeof(WebDriverFactory));
			Assert.IsInstanceOfType(factory2, typeof(WebDriverFactory));
			Assert.AreSame(factory2, factory);
		}

		[TestMethod]
		public void MultipleWebdriverFactoryPerBrowserType()
		{
			const string ChromeType = "chrome";
			const string FireFoxType = "firefox";

			ContainerBuilder builder = new ContainerBuilder();
			builder.RegisterModule(new WebdriverFactoryModule());
			IContainer container = builder.Build();

			using (ILifetimeScope scope = container.BeginLifetimeScope())
			{
				WebdriverFactoryManager factoryManager = scope.Resolve<WebdriverFactoryManager>();
				WebDriverFactory factory = factoryManager.ResolveWebDriverFactory(ChromeType);
				WebDriverFactory factory2 = factoryManager.ResolveWebDriverFactory(FireFoxType);

				Assert.IsInstanceOfType(factory, typeof(WebDriverFactory));
				Assert.IsInstanceOfType(factory2, typeof(WebDriverFactory));
				Assert.AreNotSame(factory2, factory);
			}
		}

		[TestMethod]
		public void WebdriverFactoryDefaultWebDriver()
		{
			ContainerBuilder builder = new ContainerBuilder();
			builder.RegisterModule(new WebdriverFactoryModule());
			IContainer container = builder.Build();

			using (ILifetimeScope scope = container.BeginLifetimeScope())
			{
				WebdriverFactoryManager factoryManager = scope.Resolve<WebdriverFactoryManager>();
				WebDriverFactory factory = factoryManager.ResolveWebDriverFactory(PlaybackSettings.Instance.BrowserType);

				Assert.IsInstanceOfType(factory, typeof(WebDriverFactory));
				IWebDriver driver = factory.GetWebDriver();
				Assert.IsNotNull(driver);
				factory.QuitAllDrivers();
			}
		}

		[TestMethod]
		public void TestWebDirverFactoryOverride()
		{
			const string ChromeType = "chrome";

			ContainerBuilder builder = new ContainerBuilder();
			builder.RegisterModule(new WebdriverFactoryModule());

			builder.RegisterType<WebDriverFactoryChromeDemo>()
				.Keyed<WebDriverFactory>(ChromeType)
				.SingleInstance();

			IContainer container = builder.Build();

			using (ILifetimeScope scope = container.BeginLifetimeScope())
			{
				WebdriverFactoryManager factoryManager = scope.Resolve<WebdriverFactoryManager>();
				WebDriverFactory factory = factoryManager.ResolveWebDriverFactory(ChromeType);

				Assert.IsInstanceOfType(factory, typeof(WebDriverFactory));
				Assert.IsInstanceOfType(factory, typeof(WebDriverFactoryChromeDemo));
			}
		}

		[TestMethod]
		public void CreateWebDriverWithToken()
		{
			ContainerBuilder builder = new ContainerBuilder();
			builder.RegisterModule(new WebdriverFactoryModule());
			IContainer container = builder.Build();

			using (ILifetimeScope scope = container.BeginLifetimeScope())
			{
				WebdriverFactoryManager factoryManager = scope.Resolve<WebdriverFactoryManager>();
				WebDriverFactory factory = factoryManager.ResolveWebDriverFactory(PlaybackSettings.Instance.BrowserType);

				Assert.IsInstanceOfType(factory, typeof(WebDriverFactory));
				Guid token = Guid.NewGuid();
				IWebDriver driver = factory.GetWebDriver(token);
				Assert.IsNotNull(driver);
				factory.QuitDriver(token);
			}
		}
	}
}

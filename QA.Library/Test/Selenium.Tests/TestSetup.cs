namespace Clix.QA.Selenium.Tests
{
	using Autofac;

	using Clix.QA.Selenium;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class TestSetup
	{
		[AssemblyInitialize]
		public static void AssemblyInitialize(TestContext testContext)
		{
			Registrar.Instance
				.RegisterModule(new PlaybackModule())
				.RegisterModule(new WebdriverFactoryModule());

			// Register a new chrome web driver factory to override default implementation.
			Registrar.Instance.RegisterType(
				builder =>
				{
					builder.RegisterType<WebDriverFactoryChromeDemo>()
						.Keyed<WebDriverFactory>("chrome")
						.SingleInstance();
				});
		}

		[AssemblyCleanup]
		public static void AssemblyCleanup()
		{
			WebdriverFactoryManager manager = Registrar.NewLifetimeScope.Resolve<WebdriverFactoryManager>();
			manager.QuitAllDrivers();
		}
	}
}

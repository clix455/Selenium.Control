using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Demo.Selenium
{
	using Clix.QA.Selenium;

	using OpenQA.Selenium;

	using pages;

	[TestClass]
	public class PackagePageTests
	{
		private static readonly Guid DriverToken = Guid.NewGuid();

		private static IWebDriver driver;

		[ClassInitialize]
		public static void Setup(TestContext context)
		{
			driver = Registrar.NewLifetimeScope.ResolveWebDriver(DriverToken);
			driver.LaunchPortal();
		}

		[TestMethod]
		public void TestPackageTitle()
		{
			var page = Registrar.NewLifetimeScope.ResolvePage<PackagePage>(driver);
			page.GoToUrl("packages/Clix.QA.Selenium/");
			Assert.IsTrue(
				page.PackageTitle.IndexOf("Selenium", StringComparison.InvariantCultureIgnoreCase) != -1,
				"page.PackageTitle.IndexOf('Selenium', StringComparison.InvariantCultureIgnoreCase) != -1");
		}

		[TestMethod]
		public void TestThisVersionAtTop()
		{
			const string PackageId = "Clix.QA.Selenium";
			var page = Registrar.NewLifetimeScope.ResolvePage<PackagePage>(driver);
			page.GoToUrl($"packages/{PackageId}/");

			IWebElement element = page.VersionHistory[1, 1];
			string version = element.Text.Trim();

			Assert.IsTrue(
				version.IndexOf("(current version)", StringComparison.InvariantCultureIgnoreCase) != -1,
				"Package should be marked as current version.");

		}
	}
}

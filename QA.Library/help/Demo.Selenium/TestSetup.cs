using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Selenium
{
	using Autofac;

	using Clix.QA.Selenium;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using pages;

	[TestClass]
	public class TestSetup
	{
		[AssemblyInitializeAttribute]
		public static void AssemblyInitialize(TestContext testContext)
		{
			Registrar.Instance
				.RegisterModule(new PlaybackModule())
				.RegisterModule(new WebdriverFactoryModule())
				.RegisterModule(new DemoPageModule());
		}

		[AssemblyCleanup]
		public static void AssemblyCleanup()
		{
			WebdriverFactoryManager manager = Registrar.NewLifetimeScope.Resolve<WebdriverFactoryManager>();
			manager.QuitAllDrivers();
		}
	}
}

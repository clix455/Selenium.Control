using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Clix.QA.Selenium.Tests
{
    /// <summary>
    /// https://datatables.net/
    /// </summary>
    [TestClass]
    public class AngularUiTest
    {
        private static readonly Guid DriverToken = Guid.NewGuid();

        private static IWebDriver driver;
        

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            driver = Registrar.NewLifetimeScope.ResolveWebDriver(DriverToken);
            driver.GoToUrl("https://datatables.net/examples/index");
        }
    }
}

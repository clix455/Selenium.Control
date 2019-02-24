using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Autofac;
using FrontendBuster;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Clix.QA.Selenium.Tests
{

    public class LandingPage : PageBase
    {
        public LandingPage(IWebDriver driver) : base(driver)
        {
            var codePenIframe = driver.FindElement(By.Id("result"));
            driver.SwitchTo().Frame(codePenIframe);
        }
        public override void WaitLoading()
        {
            By loadingElementLocator = By.Id("clix_wrapper");
            this.Driver.WaitUntil(ExpectedConditions.ElementExists(loadingElementLocator), TimeSpan.FromMilliseconds(5000));
        }
    }

    /// <summary>
    /// this test class is to check if the JqueryDatatable https://datatables.net/ automation class is working
    /// I created a codepen here https://codepen.io/newcools/full/pWpGaO/
    /// </summary>
    [TestClass]
    public class JqueryDatatableTest
    {
        private static readonly Guid DriverToken = Guid.NewGuid();

        private static IWebDriver driver;

        /// <summary>
        /// search something in the table
        /// </summary>
        [TestMethod]
        public void JqueryDatatableSearch()
        {
            driver.GoToUrl("https://codepen.io/newcools/full/pWpGaO/");
            var landingPage = new LandingPage(driver);
            var tableWrapper = landingPage.FindElement(By.Id("clix_wrapper"));
            JqueryDatatable datatable = new JqueryDatatable(tableWrapper, landingPage);
            datatable.SearchTable("chengwei");
            Assert.IsTrue(datatable.Rows.Count > 0 );
            Assert.IsTrue(datatable[1, 1].Text == "Chengwei Li");
            var targetRow = datatable.GetRow(1);
            var chengweiOffice = datatable.GetDataCellElement(targetRow, "Office");
            var chengweiSalary = datatable.GetDataCellElement(targetRow, "Salary");
            var chengweiExtn = datatable.GetDataCellElement(targetRow, "Extn.");
            Assert.IsTrue(chengweiOffice.Text == "Sydney");
            Assert.IsTrue(chengweiSalary.Text == "$8,888,888");
            Assert.IsTrue(chengweiExtn.Text == "8888");
        }

        /// <summary>
        /// to test if JqueryDatatable can handle the empty table scenario well
        /// </summary>
        [TestMethod]
        public void JqueryDatatableNoData()
        {
            driver.GoToUrl("https://codepen.io/newcools/full/Oxzqwr/");
            var landingPage = new LandingPage(driver);
            var tableWrapper = landingPage.FindElement(By.Id("clix_wrapper"));
            JqueryDatatable datatable = new JqueryDatatable(tableWrapper, landingPage);
            Assert.AreEqual(0, datatable.Rows.Count);
        }

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            Registrar.Instance.RegisterModule(new FrontendBusterModule());
            Registrar.Instance.RegisterType(cb => cb.RegisterType<LandingPage>());
            driver = Registrar.NewLifetimeScope.ResolveWebDriver(DriverToken);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Selenium.pages
{
	using Clix.QA.Selenium;
	using OpenQA.Selenium;

	public class PackagePage : PageBase
	{
		/// <summary>
		/// Initialises a new instance of the <see cref="PackagePage"/> class.
		/// </summary>
		/// <param name="driver">
		/// The web driver wrapped with the page.
		/// </param>
		public PackagePage(IWebDriver driver) : base(driver)
		{
		}

		public string PackageTitle
		{
			get
			{
				By locator = By.CssSelector("div.package-title h1");
				return this.GetText(locator);
			}
		}

		public VersionHistoryTable VersionHistory
		{
			get
			{
				IWebElement tableElement = FindElement(By.CssSelector("#version-history table"));
				return new VersionHistoryTable(tableElement, this);
			}
		}


		public class VersionHistoryTable : TableControl
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="VersionHistoryTable"/> class.
			/// </summary>
			/// <param name="wrappedElement">The wrapped element.
			/// </param><param name="parent">The parent.
			/// </param>
			public VersionHistoryTable(IWebElement wrappedElement, IControl parent)
				: base(wrappedElement, parent)
			{
			}
		}
	}
}

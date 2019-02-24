using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Clix.QA.Selenium;
using OpenQA.Selenium;

namespace FrontendBuster
{
    public class JqueryDatatable : TableControl
    {
        public JqueryDatatable(IWebElement wrappedElement, IControl parent) : base(wrappedElement, parent)
        {
           
        }

        protected IWebElement SearchBox = null;

        /// <summary>
        /// Gets the header name from the header element.
        /// </summary>
        /// <param name="element">
        /// The header element.
        /// </param>
        /// <returns>
        /// The table header name.
        /// </returns>
        protected override string GetHeaderName(IWebElement element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            return element.Text;
        }

        /// <summary>
        ///     Gets the table rows.
        /// </summary>
        public override ReadOnlyCollection<IWebElement> Rows
        {
            get
            {
                var rows = this.TableContents.FindElements(By.CssSelector("tr"));
                if (rows.Count == 1)
                {
                    var waitOrig = Driver.GetCurrentImplicitWait();
                    Driver.SetImplicitWait(TimeSpan.Zero);
                    try
                    {
                        rows[0].FindElement(By.CssSelector("td.dataTables_empty"));

                        // if empty, return empty
                        return new ReadOnlyCollection<IWebElement>(new List<IWebElement>());
                    }
                    catch (NoSuchElementException)
                    {
                        // ignored
                    }
                    Driver.SetImplicitWait(waitOrig);
                }
                return rows;
            }
        }

        public bool Searchable()
        {
            if (SearchBox == null)
            {
                try
                {
                    SearchBox = WrappedElement.FindElement(By.CssSelector("div.dataTables_filter > label > input"));
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            }
            return true;
        }

        public void SearchTable(string keywords)
        {
            if (Searchable())
            {
                SearchBox.SendKeys(keywords);
            }
            else
            {
                throw new InvalidOperationException("This datatable does not support search");
            }
        }

    }
}

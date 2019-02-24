namespace Clix.QA.Selenium
{
	using System;

	using OpenQA.Selenium;
	using OpenQA.Selenium.Support.UI;

	/// <summary>
	///     The web element wait.
	/// </summary>
	public class WebElementWait : DefaultWait<IWebElement>
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="WebElementWait"/> class.
		/// </summary>
		/// <param name="webElement">The input value to pass to the evaluated conditions.</param>
		public WebElementWait(IWebElement webElement)
			: this(webElement, new SystemClock())
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="WebElementWait" /> class.
		/// </summary>
		/// <param name="webElement">
		///     The web element.
		/// </param>
		/// <param name="timeout">
		///     The timeout.
		/// </param>
		public WebElementWait(IWebElement webElement, TimeSpan timeout)
			: this(webElement, new SystemClock())
		{
			this.Timeout = timeout;
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="WebElementWait"/> class.
		/// </summary>
		/// <param name="webElement">The input value to pass to the evaluated conditions.</param>
		/// <param name="clock">The clock to use when measuring the timeout.</param>
		public WebElementWait(IWebElement webElement, IClock clock)
			: base(webElement, clock)
		{
			this.IgnoreExceptionTypes(typeof(NotFoundException));
		}
	}
}

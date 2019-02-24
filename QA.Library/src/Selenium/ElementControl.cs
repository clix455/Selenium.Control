namespace Clix.QA.Selenium
{
	using System;
	using System.Collections.ObjectModel;

	using OpenQA.Selenium;
	using OpenQA.Selenium.Support.PageObjects;
	using OpenQA.Selenium.Support.UI;

	/// <summary>
	///     The element control.
	/// </summary>
	public abstract class ElementControl : IControl
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ElementControl"/> class.
		/// </summary>
		/// <param name="wrappedElement">
		/// The wrapped web element.
		/// </param>
		/// <param name="parent">
		/// The parent.
		/// </param>
		/// <remarks>
		/// The wrapped element is the root for the new element control. Any children controls can be directly found/searched
		///     from it.
		/// </remarks>
		protected ElementControl(IWebElement wrappedElement, IControl parent)
		{
			if (parent == null)
				throw new ArgumentNullException(nameof(parent), "Element must have a page.");

			this.Parent = parent;

			if (wrappedElement != null)
			{
				this.WrappedElement = wrappedElement;
				PageFactory.InitElements(wrappedElement, this);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ElementControl"/> class.
		/// </summary>
		/// <param name="parent">
		/// The parent.
		/// </param>
		protected ElementControl(IControl parent)
			: this(null, parent)
		{
		}

		/// <summary>
		///     Gets the driver.
		/// </summary>
		public IWebDriver Driver => this.Parent.Driver;

		/// <summary>
		///     Gets the parent control.
		/// </summary>
		public IControl Parent { get; }

		/// <summary>
		///     Gets or sets the wait.
		/// </summary>
		public IWait<IWebDriver> Wait { get; set; }

		/// <summary>
		///     Gets or sets gets the wrapped element.
		/// </summary>
		public virtual IWebElement WrappedElement { get; protected set; }

		/// <summary>
		/// Finds the first <see cref="T:OpenQA.Selenium.IWebElement"/> using the given method.
		/// </summary>
		/// <param name="by">
		/// The locating mechanism to use.
		/// </param>
		/// <returns>
		/// The first matching <see cref="T:OpenQA.Selenium.IWebElement"/> on the current context.
		/// </returns>
		/// <exception cref="T:OpenQA.Selenium.NoSuchElementException">
		/// If no element matches the criteria.
		/// </exception>
		public IWebElement FindElement(By by)
		{
			if (@by == null)
				throw new ArgumentNullException(nameof(@by));

			if (this.WrappedElement == null)
				return this.Parent?.FindElement(@by);

			this.WaitLoading();
			return this.WrappedElement.FindElement(@by);
		}

		/// <summary>
		/// Finds all <see cref="T:OpenQA.Selenium.IWebElement">IWebElements</see> within the current control using the given
		///     mechanism.
		/// </summary>
		/// <param name="by">
		/// The locating mechanism to use.
		/// </param>
		/// <returns>
		/// A <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1"/> of all
		///     <see cref="T:OpenQA.Selenium.IWebElement">WebElements</see>
		///     matching the current criteria, or an empty list if nothing matches.
		/// </returns>
		public ReadOnlyCollection<IWebElement> FindElements(By by)
		{
			if (@by == null)
				throw new ArgumentNullException(nameof(@by));

			if (this.WrappedElement == null)
				return this.Parent?.FindElements(@by);

			this.WaitLoading();
			return this.WrappedElement.FindElements(@by);
		}

		/// <summary>
		/// Wait until the current control completes loading.
		/// </summary>
		/// <remarks>
		/// The web control can be either a page, or an element control.
		/// </remarks>
		public virtual void WaitLoading()
		{
		}
	}
}
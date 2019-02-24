namespace Clix.QA.Selenium
{
	using System;
	using System.Collections.ObjectModel;
	using System.Linq;

	using OpenQA.Selenium;

	using Validation;

	/// <summary>
	/// The collection control.
	/// </summary>
	public abstract class CollectionControl : ICollectionControl
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CollectionControl"/> class.
		/// </summary>
		/// <param name="parentElement">
		/// The parent element.
		/// </param>
		protected CollectionControl(IWebElement parentElement)
		{
			this.ParentElement = parentElement;
		}

		/// <summary>
		///     Gets the options.
		/// </summary>
		protected virtual ReadOnlyCollection<IWebElement> Options =>
			this.OptionsContainer.FindElements(By.CssSelector("ul > li"));

		/// <summary>
		///     Gets the options container element.
		/// </summary>
		protected abstract IWebElement OptionsContainer { get; }

		/// <summary>
		///     Gets the container element.
		/// </summary>
		protected IWebElement ParentElement { get; }

		/// <summary>
		/// Selects element by its zero-based index.
		/// </summary>
		/// <param name="index">
		/// The element index.
		/// </param>
		public virtual void SelectByIndex(int index)
		{
			IWebElement matchingElement = this.Options[index];
			matchingElement.WaitClickable();
			matchingElement.Click();
		}

		/// <summary>
		/// Selects element by its one-based index.
		/// </summary>
		/// <param name="position">
		/// The element position.
		/// </param>
		public virtual void SelectByPosition(int position)
		{
			Requires.Range(position >= 1, nameof(position));

			IWebElement matchingElement = this.Options[position - 1];
			matchingElement.WaitClickable();
			matchingElement.Click();
		}

		/// <summary>
		/// Selects element by its text.
		/// </summary>
		/// <param name="text">
		/// The element text.
		/// </param>
		public virtual void SelectByText(string text)
		{
			IWebElement matchingElement = null;
			WaitConditions.WaitForCondition(
				() =>
				{
					matchingElement =
						this.Options.FirstOrDefault(
							webElement =>
							webElement.Text.Trim().Equals(text, StringComparison.CurrentCultureIgnoreCase));

					return matchingElement != null;
				},
				5000);

			if (matchingElement == null)
				throw new NoSuchElementException($"Cannot locate option with text: {text}");

			this.GetClickable(matchingElement).Click();
		}

		/// <summary>
		/// Gets the clickable element from the specified option.
		/// </summary>
		/// <param name="optionElement">
		/// The specified option.
		/// </param>
		/// <returns>
		/// The clickable element which is used to select the specified option.
		/// </returns>
		protected virtual IWebElement GetClickable(IWebElement optionElement)
		{
			if (optionElement == null)
				throw new ArgumentNullException(nameof(optionElement));

			return optionElement.FindElement(By.TagName("a")).WaitClickable();
		}
	}
}
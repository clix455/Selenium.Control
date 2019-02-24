namespace Clix.QA.Selenium
{
	using System;
	using System.Collections.ObjectModel;
	using System.Linq;

	using OpenQA.Selenium;

	/// <summary>
	/// Supplies a set of common conditions that can be waited for using <see cref="WebElementWait" />.
	/// </summary>
	/// <example>
	///  <code>
	/// 		IWait wait = new WebElementWait(searchContext, TimeSpan.FromSeconds(3))
	/// 		IWebElement element = wait.Until(ElementExpectedConditions.ElementExists(By.Id("foo")));
	///  </code>
	/// </example>
	public static class ElementExpectedConditions
	{
		/// <summary>
		/// An expectation for checking that an element is present on the DOM of a page.
		///     This does not necessarily mean that the element is visible.
		/// </summary>
		/// <param name="locator">
		/// The locator used to find the element.
		/// </param>
		/// <returns>
		/// The <see cref="T:OpenQA.Selenium.IWebElement"/> once it is located.
		/// </returns>
		public static Func<IWebElement, IWebElement> ElementExists(By locator)
		{
			return searchContext => searchContext.FindElement(locator);
		}

		/// <summary>
		/// An expectation for checking that an element is present on the DOM of a page and visible.
		///     Visibility means that the element is not only displayed but also has a height and width that is greater than 0.
		/// </summary>
		/// <param name="locator">
		/// The locator used to find the element.
		/// </param>
		/// <returns>
		/// The <see cref="T:OpenQA.Selenium.IWebElement"/> once it is located and visible.
		/// </returns>
		public static Func<IWebElement, IWebElement> ElementIsVisible(By locator)
		{
			return
				searchContext =>
				{
					try
					{
						return ElementIfVisible(searchContext.FindElement(locator));
					}
					catch (StaleElementReferenceException)
					{
						return null;
					}
				};
		}

		/// <summary>
		/// An expectation for checking if the given element is in correct state.
		/// </summary>
		/// <param name="element">
		/// The element.
		/// </param>
		/// <param name="selected">
		/// selected or not selected
		/// </param>
		/// <returns>
		/// <see langword="true"/> given element is in correct state.; otherwise, <see langword="false"/>.
		/// </returns>
		public static Func<IWebElement, bool> ElementSelectionStateToBe(IWebElement element, bool selected)
		{
			return searchContext => element.Selected == selected;
		}

		/// <summary>
		/// An expectation for checking if the given element is in correct state.
		/// </summary>
		/// <param name="locator">
		/// The locator used to find the element.
		/// </param>
		/// <param name="selected">
		/// selected or not selected
		/// </param>
		/// <returns>
		/// <see langword="true"/> given element is in correct state.; otherwise, <see langword="false"/>.
		/// </returns>
		public static Func<IWebElement, bool> ElementSelectionStateToBe(By locator, bool selected)
		{
			return
				searchContext =>
				{
					try
					{
						return searchContext.FindElement(locator).Selected == selected;
					}
					catch (StaleElementReferenceException)
					{
						return false;
					}
				};
		}

		/// <summary>
		/// An expectation for checking an element is visible and enabled such that you can click it.
		/// </summary>
		/// <param name="locator">
		/// The locator used to find the element.
		/// </param>
		/// <returns>
		/// The <see cref="T:OpenQA.Selenium.IWebElement"/> once it is located and clickable (visible and enabled).
		/// </returns>
		public static Func<IWebElement, IWebElement> ElementToBeClickable(By locator)
		{
			return
				searchContext =>
				{
					IWebElement webElement = ElementIfVisible(searchContext.FindElement(locator));
					try
					{
						if (webElement != null && webElement.Enabled)
							return webElement;
						return (IWebElement)null;
					}
					catch (StaleElementReferenceException)
					{
						return (IWebElement)null;
					}
				};
		}

		/// <summary>
		/// An expectation for checking an element is visible and enabled such that you can click it.
		/// </summary>
		/// <param name="element">
		/// The element.
		/// </param>
		/// <returns>
		/// The <see cref="T:OpenQA.Selenium.IWebElement"/> once it is clickable (visible and enabled).
		/// </returns>
		public static Func<IWebElement, IWebElement> ElementToBeClickable(IWebElement element)
		{
			return
				searchContext =>
				{
					try
					{
						if (element != null && element.Displayed && element.Enabled)
							return element;
						return (IWebElement)null;
					}
					catch (StaleElementReferenceException)
					{
						return (IWebElement)null;
					}
				};
		}

		/// <summary>
		/// An expectation for checking if the given element is selected.
		/// </summary>
		/// <param name="element">
		/// The element.
		/// </param>
		/// <returns>
		/// <see langword="true"/> given element is selected.; otherwise, <see langword="false"/>.
		/// </returns>
		public static Func<IWebElement, bool> ElementToBeSelected(IWebElement element)
		{
			return ElementSelectionStateToBe(element, true);
		}

		/// <summary>
		/// An expectation for checking if the given element is selected.
		/// </summary>
		/// <param name="element">
		/// The element.
		/// </param>
		/// <param name="selected">
		/// selected or not selected
		/// </param>
		/// <returns>
		/// <see langword="true"/> given element is selected.; otherwise, <see langword="false"/>.
		/// </returns>
		public static Func<IWebElement, bool> ElementToBeSelected(IWebElement element, bool selected)
		{
			return searchContext => element.Selected == selected;
		}

		/// <summary>
		/// An expectation for checking if the given element is selected.
		/// </summary>
		/// <param name="locator">
		/// The locator used to find the element.
		/// </param>
		/// <returns>
		/// <see langword="true"/> given element is selected.; otherwise, <see langword="false"/>.
		/// </returns>
		public static Func<IWebElement, bool> ElementToBeSelected(By locator)
		{
			return ElementSelectionStateToBe(locator, true);
		}

		/// <summary>
		/// An expectation for checking that an element is either invisible or not present on the DOM.
		/// </summary>
		/// <param name="locator">
		/// The locator used to find the element.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the element is not displayed; otherwise, <see langword="false"/>.
		/// </returns>
		public static Func<IWebElement, bool> InvisibilityOfElementLocated(By locator)
		{
			return searchContext =>
					{
						try
						{
							return !searchContext.FindElement(locator).Displayed;
						}
						catch (NoSuchElementException)
						{
							return true;
						}
						catch (StaleElementReferenceException)
						{
							return true;
						}
					};
		}

		/// <summary>
		/// An expectation for checking that an element with text is either invisible or not present on the DOM.
		/// </summary>
		/// <param name="locator">
		/// The locator used to find the element.
		/// </param>
		/// <param name="text">
		/// Text of the element
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the element is not displayed; otherwise, <see langword="false"/>.
		/// </returns>
		public static Func<IWebElement, bool> InvisibilityOfElementWithText(By locator, string text)
		{
			return
				searchContext =>
				{
					try
					{
						string text1 = searchContext.FindElement(locator).Text;
						if (string.IsNullOrEmpty(text1))
							return true;
						return !text1.Equals(text);
					}
					catch (NoSuchElementException)
					{
						return true;
					}
					catch (StaleElementReferenceException)
					{
						return true;
					}
				};
		}

		/// <summary>
		/// An expectation for checking that all elements present on the web page that match the locator.
		/// </summary>
		/// <param name="locator">
		/// The locator used to find the element.
		/// </param>
		/// <returns>
		/// The list of <see cref="T:OpenQA.Selenium.IWebElement"/> once it is located.
		/// </returns>
		public static Func<IWebElement, ReadOnlyCollection<IWebElement>> PresenceOfAllElementsLocatedBy(By locator)
		{
			return
				searchContext =>
				{
					try
					{
						ReadOnlyCollection<IWebElement> elements = searchContext.FindElements(locator);
						return elements.Any() ? elements : null;
					}
					catch (StaleElementReferenceException)
					{
						return null;
					}
				};
		}

		/// <summary>
		/// Wait until an element is no longer attached to the DOM.
		/// </summary>
		/// <param name="element">
		/// The element.
		/// </param>
		/// <returns>
		/// <see langword="false"/> is the element is still attached to the DOM; otherwise, <see langword="true"/>.
		/// </returns>
		public static Func<IWebElement, bool> StalenessOf(IWebElement element)
		{
			return
				searchContext =>
				{
					try
					{
						return element == null || !element.Enabled;
					}
					catch (StaleElementReferenceException)
					{
						return true;
					}
				};
		}

		/// <summary>
		/// An expectation for checking if the given text is present in the specified element.
		/// </summary>
		/// <param name="element">
		/// The WebElement
		/// </param>
		/// <param name="text">
		/// Text to be present in the element
		/// </param>
		/// <returns>
		/// <see langword="true"/> once the element contains the given text; otherwise, <see langword="false"/>.
		/// </returns>
		public static Func<IWebElement, bool> TextToBePresentInElement(IWebElement element, string text)
		{
			return
				searchContext =>
				{
					try
					{
						return element.Text.Contains(text);
					}
					catch (StaleElementReferenceException)
					{
						return false;
					}
				};
		}

		/// <summary>
		/// An expectation for checking if the given text is present in the element that matches the given locator.
		/// </summary>
		/// <param name="locator">
		/// The locator used to find the element.
		/// </param>
		/// <param name="text">
		/// Text to be present in the element
		/// </param>
		/// <returns>
		/// <see langword="true"/> once the element contains the given text; otherwise, <see langword="false"/>.
		/// </returns>
		public static Func<IWebElement, bool> TextToBePresentInElementLocated(By locator, string text)
		{
			return
				searchContext =>
				{
					try
					{
						return searchContext.FindElement(locator).Text.Contains(text);
					}
					catch (StaleElementReferenceException)
					{
						return false;
					}
				};
		}

		/// <summary>
		/// An expectation for checking if the given text is present in the specified elements value attribute.
		/// </summary>
		/// <param name="element">
		/// The WebElement
		/// </param>
		/// <param name="text">
		/// Text to be present in the element
		/// </param>
		/// <returns>
		/// <see langword="true"/> once the element contains the given text; otherwise, <see langword="false"/>.
		/// </returns>
		public static Func<IWebElement, bool> TextToBePresentInElementValue(IWebElement element, string text)
		{
			return
				searchContext =>
				{
					try
					{
						string attribute = element.GetAttribute("value");
						if (attribute != null)
							return attribute.Contains(text);
						return false;
					}
					catch (StaleElementReferenceException)
					{
						return false;
					}
				};
		}

		/// <summary>
		/// An expectation for checking if the given text is present in the specified elements value attribute.
		/// </summary>
		/// <param name="locator">
		/// The locator used to find the element.
		/// </param>
		/// <param name="text">
		/// Text to be present in the element
		/// </param>
		/// <returns>
		/// <see langword="true"/> once the element contains the given text; otherwise, <see langword="false"/>.
		/// </returns>
		public static Func<IWebElement, bool> TextToBePresentInElementValue(By locator, string text)
		{
			return
				searchContext =>
				{
					try
					{
						string attribute = searchContext.FindElement(locator).GetAttribute("value");
						if (attribute != null)
							return attribute.Contains(text);
						return false;
					}
					catch (StaleElementReferenceException)
					{
						return false;
					}
				};
		}

		/// <summary>
		/// An expectation for checking that all elements present on the web page that match the locator are visible.
		///     Visibility means that the elements are not only displayed but also have a height and width that is greater than 0.
		/// </summary>
		/// <param name="locator">
		/// The locator used to find the element.
		/// </param>
		/// <returns>
		/// The list of <see cref="T:OpenQA.Selenium.IWebElement"/> once it is located and visible.
		/// </returns>
		public static Func<IWebElement, ReadOnlyCollection<IWebElement>> VisibilityOfAllElementsLocatedBy(By locator)
		{
			return
				searchContext =>
				{
					try
					{
						ReadOnlyCollection<IWebElement> elements = searchContext.FindElements(locator);
						if (elements.Any(element => !element.Displayed))
							return null;
						return elements.Any() ? elements : null;
					}
					catch (StaleElementReferenceException)
					{
						return null;
					}
				};
		}

		/// <summary>
		/// An expectation for checking that all elements present on the web page that match the locator are visible.
		///     Visibility means that the elements are not only displayed but also have a height and width that is greater than 0.
		/// </summary>
		/// <param name="elements">
		/// list of WebElements
		/// </param>
		/// <returns>
		/// The list of <see cref="T:OpenQA.Selenium.IWebElement"/> once it is located and visible.
		/// </returns>
		public static Func<IWebElement, ReadOnlyCollection<IWebElement>> VisibilityOfAllElementsLocatedBy(ReadOnlyCollection<IWebElement> elements)
		{
			return
				searchContext =>
				{
					try
					{
						if (elements.Any(element => !element.Displayed))
							return null;
						return elements.Any() ? elements : null;
					}
					catch (StaleElementReferenceException)
					{
						return null;
					}
				};
		}

		/// <summary>
		/// The element if visible.
		/// </summary>
		/// <param name="element">
		/// The element.
		/// </param>
		/// <returns>
		/// The <see cref="IWebElement"/>.
		/// </returns>
		private static IWebElement ElementIfVisible(IWebElement element)
		{
			if (!element.Displayed)
				return null;
			return element;
		}
	}
}
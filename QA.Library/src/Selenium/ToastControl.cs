namespace Clix.QA.Selenium
{
	using System;
	using System.Collections.ObjectModel;
	using System.Linq;

	using OpenQA.Selenium;
	using OpenQA.Selenium.Support.UI;

	/// <summary>
	///     The toast control.
	/// </summary>
	public class ToastControl : ElementControl
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ToastControl"/> class.
		/// </summary>
		/// <param name="parent">
		/// The parent.
		/// </param>
		public ToastControl(IControl parent)
			: base(parent)
		{
		}

		/// <summary>
		///     Gets the toast container.
		/// </summary>
		protected virtual IWebElement ToastContainer
		{
			get
			{
				By locator = By.Id("toast-container");
				IWebElement element = this.Driver.WaitUntil(
					ExpectedConditions.ElementIsVisible(locator),
					TimeSpan.FromSeconds(5), // Grace period for the toast control to appear
					typeof(NoSuchElementException));
				return element;
			}
		}

		/// <summary>
		///     Gets all the toasts.
		/// </summary>
		protected virtual ReadOnlyCollection<IWebElement> Toasts
		{
			get
			{
				By locator = By.CssSelector("div.toast");
				ReadOnlyCollection<IWebElement> toasts = null;
				WaitConditions.WaitForCondition(
					() =>
					{
						toasts = this.Driver.WaitUntil(
							ExpectedConditions.VisibilityOfAllElementsLocatedBy(locator),
							TimeSpan.FromSeconds(1),
							typeof(NoSuchElementException));
						return toasts != null && toasts.Any();
					},
					millisecondsTimeout: 5000);
				return toasts;
			}
		}

		/// <summary>
		/// Try to close all the toasts.
		/// </summary>
		public virtual void TryCloseAllToasts()
		{
			try
			{
				foreach (IWebElement toast in this.Toasts)
				{
					IWebElement closeButton = toast.FindElement(By.CssSelector("button.toast-close-button"));
					closeButton.Click();
				}
			}
			catch (Exception)
			{
				// Continue when exceptions happen
			}
		}

		/// <summary>
		/// Indicating if the fail message contains the expected <paramref name="message"/>.
		/// </summary>
		/// <param name="message">
		/// The expected message.
		/// </param>
		/// <param name="comparisonType">
		/// The string comparison Type.
		/// </param>
		/// <returns>
		/// True when the expected message is contained in any failed messages; otherwise false.
		/// </returns>
		public virtual bool FailMessageContains(
			string message,
			StringComparison comparisonType = StringComparison.CurrentCulture)
		{
			if (string.IsNullOrEmpty(message))
				throw new ArgumentException("Message cannot be null or empty!");

			return WaitConditions.WaitForCondition(
				() =>
				{
					IWebElement matchingElement =
						this.Toasts.FirstOrDefault(
							element =>
							{
								if (!this.IsFailToast(element))
									return false;
								string toastMessage = this.GetToastMessage(element);
								return toastMessage.IndexOf(message, comparisonType) != -1;
							});

					return matchingElement != null;
				});
		}

		/// <summary>
		/// Indicating if there is any fail message exactly equals the expected <paramref name="message"/>.
		/// </summary>
		/// <param name="message">
		/// The expected message.
		/// </param>
		/// <param name="comparisonType">
		/// The string comparison Type.
		/// </param>
		/// <returns>
		/// True when the expected message exactly equals to any fail messages; otherwise false.
		/// </returns>
		public virtual bool FailMessageEquals(
			string message,
			StringComparison comparisonType = StringComparison.CurrentCulture)
		{
			if (string.IsNullOrEmpty(message))
				throw new ArgumentException("Message cannot be null or empty!");

			IWebElement matchingElement =
				this.Toasts.FirstOrDefault(
					element =>
					{
						if (!this.IsFailToast(element))
							return false;
						string toastMessage = this.GetToastMessage(element);
						return toastMessage.Equals(message, comparisonType);
					});

			return matchingElement != null;
		}

		/// <summary>
		/// Indicating if the information message contains the expected <paramref name="message"/>.
		/// </summary>
		/// <param name="message">
		/// The expected message.
		/// </param>
		/// <param name="comparisonType">
		/// The string comparison type.
		/// </param>
		/// <returns>
		/// True when the expected message is contained in any information toast; otherwise false.
		/// </returns>
		public virtual bool InfoMessageContains(
			string message,
			StringComparison comparisonType = StringComparison.CurrentCulture)
		{
			if (string.IsNullOrEmpty(message))
				throw new ArgumentException("Message cannot be null or empty!");

			return WaitConditions.WaitForCondition(
				() =>
				{
					IWebElement matchingElement =
						this.Toasts.FirstOrDefault(
							element =>
							{
								if (!this.IsInfoToast(element))
									return false;
								string toastMessage = this.GetToastMessage(element);
								return toastMessage.IndexOf(message, comparisonType) != -1;
							});

					return matchingElement != null;
				});
		}

		/// <summary>
		/// Indicating if the success message contains the expected <paramref name="message"/>.
		/// </summary>
		/// <param name="message">
		/// The expected message.
		/// </param>
		/// <param name="comparisonType">
		/// The string comparison type.
		/// </param>
		/// <returns>
		/// True when the expected message is contained in any success messages; otherwise false.
		/// </returns>
		public virtual bool SuccessMessageContains(
			string message,
			StringComparison comparisonType = StringComparison.CurrentCulture)
		{
			if (string.IsNullOrEmpty(message))
				throw new ArgumentException("Message cannot be null or empty!");

			return WaitConditions.WaitForCondition(
				() =>
				{
					try
					{
						IWebElement matchingElement =
							this.Toasts.FirstOrDefault(
								element =>
								{
									if (!this.IsSuccessToast(element))
										return false;
									string toastMessage = this.GetToastMessage(element);
									return toastMessage.IndexOf(message, comparisonType) != -1;
								});

						return matchingElement != null;
					}
					catch (Exception)
					{
						return false;
					}
				},
				5000);
		}

		/// <summary>
		/// Indicating if there is any success message exactly equals the expected <paramref name="message"/>.
		/// </summary>
		/// <param name="message">
		/// The expected message.
		/// </param>
		/// <param name="comparisonType">
		/// The comparison Type.
		/// </param>
		/// <returns>
		/// True when the expected message exactly equals to any success messages; otherwise false.
		/// </returns>
		public virtual bool SuccessMessageEquals(
			string message,
			StringComparison comparisonType = StringComparison.CurrentCulture)
		{
			if (string.IsNullOrEmpty(message))
				throw new ArgumentException("Message cannot be null or empty!");

			return WaitConditions.WaitForCondition(
				() =>
				{
					try
					{
						IWebElement matchingElement =
							this.Toasts.FirstOrDefault(
								element =>
								{
									if (!this.IsSuccessToast(element))
										return false;
									string toastMessage = this.GetToastMessage(element);
									return toastMessage.Equals(message, comparisonType);
								});

						return matchingElement != null;
					}
					catch (Exception)
					{
						return false;
					}
				},
				5000);
		}

		/// <summary>
		/// Indicating if the warning message contains the expected <paramref name="message"/>.
		/// </summary>
		/// <param name="message">
		/// The expected message.
		/// </param>
		/// <param name="comparisonType">
		/// The string comparison type.
		/// </param>
		/// <returns>
		/// True when the expected message is contained in any warning messages; otherwise false.
		/// </returns>
		public virtual bool WarningMessageContains(
			string message,
			StringComparison comparisonType = StringComparison.CurrentCulture)
		{
			if (string.IsNullOrEmpty(message))
				throw new ArgumentException("Message cannot be null or empty!");

			return WaitConditions.WaitForCondition(
				() =>
				{
					IWebElement matchingElement =
						this.Toasts.FirstOrDefault(
							element =>
							{
								if (!this.IsWarningToast(element))
									return false;
								string toastMessage = this.GetToastMessage(element);
								return toastMessage.IndexOf(message, comparisonType) != -1;
							});

					return matchingElement != null;
				});
		}

		/// <summary>
		/// Gets the toast message.
		/// </summary>
		/// <param name="element">
		/// The toast element.
		/// </param>
		/// <returns>
		/// The message from the toast element.
		/// </returns>
		protected virtual string GetToastMessage(IWebElement element)
		{
			if (element == null)
				throw new ArgumentNullException(nameof(element));
			string toastMessage = element.FindElement(By.CssSelector("div.toast-message")).Text;
			return toastMessage;
		}

		/// <summary>
		/// Indicating if the element is a fail toast.
		/// </summary>
		/// <param name="element">
		/// The element.
		/// </param>
		/// <returns>
		/// The <see cref="bool"/>.
		/// </returns>
		protected virtual bool IsFailToast(IWebElement element)
		{
			if (element == null)
				throw new ArgumentNullException(nameof(element));
			string classAtttribute = element.GetAttribute("class");
			return !string.IsNullOrEmpty(classAtttribute)
					&& classAtttribute.IndexOf("toast-error", StringComparison.CurrentCultureIgnoreCase) != -1;
		}

		/// <summary>
		/// Indicating if the element is an info type toast.
		/// </summary>
		/// <param name="element">
		/// The element.
		/// </param>
		/// <returns>
		/// true if the toast element is an info toast, otherwise false.
		/// </returns>
		protected virtual bool IsInfoToast(IWebElement element)
		{
			if (element == null)
				throw new ArgumentNullException(nameof(element));
			string classAtttribute = element.GetAttribute("class");
			return !string.IsNullOrEmpty(classAtttribute)
					&& classAtttribute.IndexOf("toast-info", StringComparison.CurrentCultureIgnoreCase) != -1;
		}

		/// <summary>
		/// Indicating if the element is a success toast.
		/// </summary>
		/// <param name="element">
		/// The element.
		/// </param>
		/// <returns>
		/// true if the toast element is a success toast, otherwise false.
		/// </returns>
		protected virtual bool IsSuccessToast(IWebElement element)
		{
			if (element == null)
				throw new ArgumentNullException(nameof(element));
			string classAtttribute = element.GetAttribute("class");
			return !string.IsNullOrEmpty(classAtttribute)
					&& classAtttribute.IndexOf("toast-success", StringComparison.CurrentCultureIgnoreCase) != -1;
		}

		/// <summary>
		/// Indicating if the element is a warning toast.
		/// </summary>
		/// <param name="element">
		/// The element.
		/// </param>
		/// <returns>
		/// true if the toast element is a warning toast, otherwise false.
		/// </returns>
		protected virtual bool IsWarningToast(IWebElement element)
		{
			if (element == null)
				throw new ArgumentNullException(nameof(element));
			string classAtttribute = element.GetAttribute("class");
			return !string.IsNullOrEmpty(classAtttribute)
					&& classAtttribute.IndexOf("toast-warning", StringComparison.CurrentCultureIgnoreCase) != -1;
		}
	}
}
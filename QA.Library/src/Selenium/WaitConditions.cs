namespace Clix.QA.Selenium
{
	using System;
	using System.Diagnostics;
	using System.Globalization;
	using System.Threading;

	using OpenQA.Selenium;
	using OpenQA.Selenium.Support.UI;

	using Validation;

	/// <summary>
	///     The wait for conditions helpers.
	/// </summary>
	public static class WaitConditions
	{
		/// <summary>
		/// Wait until the <see cref="IWebElement"/> is clickable.
		/// </summary>
		/// <param name="webElement">
		/// The web element.
		/// </param>
		/// <param name="seconds">
		/// The timeout seconds.
		/// </param>
		/// <returns>
		/// The element.
		/// </returns>
		/// <exception cref="OpenQA.Selenium.ElementNotVisibleException">
		/// The element is not clickable after timeout.
		/// </exception>
		public static IWebElement WaitClickable(this IWebElement webElement, int? seconds = null)
		{
			IWebElement element = webElement.WaitDisplayed();
			if (element == null)
				return null;

			TimeSpan timeout = TimeSpan.FromSeconds(seconds ?? PlaybackSettings.Instance.ExplicitlyWaitSecond);
			var wait = new WebElementWait(element, timeout);
			if (!wait.Until(ele => ele.Enabled))
			{
				throw new ElementNotVisibleException("The element is not clickable at this time.");
			}

			return element;
		}

		/// <summary>
		/// Wait until the <see cref="IWebElement"/> displayed.
		/// </summary>
		/// <param name="webElement">
		/// The web element.
		/// </param>
		/// <param name="seconds">
		/// The timeout seconds.
		/// </param>
		/// <returns>
		/// The <see cref="bool"/> indicating if the <see cref="IWebElement"/> displayed.
		/// </returns>
		public static IWebElement WaitDisplayed(this IWebElement webElement, int? seconds = null)
		{
			Requires.NotNull(webElement, nameof(webElement));

			TimeSpan timeout = TimeSpan.FromSeconds(seconds ?? PlaybackSettings.Instance.ExplicitlyWaitSecond);
			var wait =
				new WebElementWait(
					webElement,
					timeout);

			wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
			if (!wait.Until(element => element.Displayed))
			{
				throw new ElementNotVisibleException();
			}

			return webElement;
		}

		/// <summary>
		/// Blocks the current thread until the specified condition is met, or until the specified time-out expires.
		/// </summary>
		/// <returns>
		/// True if the condition is met before the time-out; otherwise, false.
		/// </returns>
		/// <param name="conditionContext">
		/// The context to evaluate the condition.
		/// </param>
		/// <param name="conditionEvaluator">
		/// The delegate to evaluate the condition.
		/// </param>
		/// <param name="millisecondsTimeout">
		/// The number of milliseconds before time-out.
		/// </param>
		/// <typeparam name="T">
		/// The <see cref="T:System.Type"/> that specifies the Type for the condition and predicate.
		/// </typeparam>
		public static bool WaitForCondition<T>(
			this T conditionContext,
			Predicate<T> conditionEvaluator,
			int millisecondsTimeout)
		{
			if (Equals(conditionContext, default(T)))
			{
				throw new ArgumentNullException(nameof(conditionContext));
			}

			if (conditionEvaluator == null)
			{
				throw new ArgumentNullException(nameof(conditionEvaluator));
			}

			CheckForMinimumPermissibleValue(0, millisecondsTimeout, "millisecondsTimeout");

			Stopwatch stopwatch = Stopwatch.StartNew();
			while (!conditionEvaluator(conditionContext))
			{
				Thread.Sleep(Math.Min(Math.Max((int)(millisecondsTimeout - stopwatch.ElapsedMilliseconds), 0), 100));
				if (stopwatch.ElapsedMilliseconds >= millisecondsTimeout)
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Blocks the current thread until the specified condition is met, or until the specified time-out expires.
		/// </summary>
		/// <returns>
		/// True if the condition is met before the time-out; otherwise, false.
		/// </returns>
		/// <param name="conditionEvaluator">
		/// The delegate to evaluate the condition.
		/// </param>
		public static bool WaitForCondition(Predicate conditionEvaluator)
		{
			Requires.NotNull(conditionEvaluator, nameof(conditionEvaluator));

			return WaitForCondition(conditionEvaluator, PlaybackSettings.Instance.ExplicitlyWaitSecond * 1000);
		}

		/// <summary>
		/// Blocks the current thread until the specified condition is met, or until the specified time-out expires.
		/// </summary>
		/// <returns>
		/// True if the condition is met before the time-out; otherwise, false.
		/// </returns>
		/// <param name="conditionEvaluator">
		/// The delegate to evaluate the condition.
		/// </param>
		/// <param name="millisecondsTimeout">
		/// The number of milliseconds before time-out.
		/// </param>
		public static bool WaitForCondition(Predicate conditionEvaluator, int millisecondsTimeout)
		{
			if (conditionEvaluator == null)
			{
				throw new ArgumentNullException(nameof(conditionEvaluator));
			}

			CheckForMinimumPermissibleValue(0, millisecondsTimeout, "millisecondsTimeout");

			Stopwatch stopwatch = Stopwatch.StartNew();
			while (!conditionEvaluator())
			{
				Thread.Sleep(Math.Min(Math.Max((int)(millisecondsTimeout - stopwatch.ElapsedMilliseconds), 0), 100));
				if (stopwatch.ElapsedMilliseconds >= millisecondsTimeout)
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Waits the page loaded.
		/// </summary>
		/// <param name="driver">
		/// The driver.
		/// </param>
		/// <param name="seconds">
		/// The seconds.
		/// </param>
		/// <returns>
		/// The condition result.
		/// </returns>
		public static bool WaitPageLoaded(this IWebDriver driver, int? seconds = null)
		{
			Requires.NotNull(driver, nameof(driver));

			TimeSpan timeout = TimeSpan.FromSeconds(seconds ?? PlaybackSettings.Instance.ExplicitlyWaitSecond);
			IWait<IWebDriver> wait = new WebDriverWait(driver, timeout);
			try
			{
				return wait.Until(
					j =>
					((IJavaScriptExecutor)driver)
						.ExecuteScript("return document.readyState")
						.Equals("complete"));
			}
			catch (Exception)
			{
				return false;
			}
		}

		/// <summary>
		/// Waits the retry click.
		/// </summary>
		/// <param name="webElement">
		/// The web element.
		/// </param>
		/// <param name="retry">
		/// The retry.
		/// </param>
		/// <returns>
		/// The retried click result.
		/// </returns>
		public static bool WaitRetryClick(this IWebElement webElement, int retry = 0)
		{
			Requires.NotNull(webElement, nameof(webElement));
			IWebElement element = WaitClickable(webElement);
			do
			{
				try
				{
					element.Click();
					return true;
				}
				catch (Exception)
				{
					Thread.Sleep(TimeSpan.FromSeconds(1));
				}
			}
			while (retry-- > 0);
			return false;
		}

		/// <summary>
		/// Wait until a condition evaluated true.
		/// </summary>
		/// <param name="webElement">
		/// The web element.
		/// </param>
		/// <param name="driver">
		/// The webdriver used for waiting.
		/// </param>
		/// <param name="condition">
		/// The condition evaluator.
		/// </param>
		/// <returns>
		/// The <see cref="bool"/> indicating if expected condition achieved.
		/// </returns>
		public static bool WaitUntil(
			this IWebElement webElement,
			IWebDriver driver,
			Func<IWebElement, bool> condition)
		{
			Requires.NotNull(webElement, nameof(webElement));
			Requires.NotNull(driver, nameof(driver));
			Requires.NotNull(condition, nameof(condition));

			return WaitUntil(webElement, driver, condition, TimeSpan.FromSeconds(PlaybackSettings.Instance.ExplicitlyWaitSecond));
		}

		/// <summary>
		/// Waits until the condition evaluated.
		/// </summary>
		/// <param name="webDriver">
		/// The web driver.
		/// </param>
		/// <param name="condition">
		/// The condition.
		/// </param>
		/// <param name="timeout">
		/// The maximum waiting timeout for the specified condition to occur.
		/// </param>
		/// <typeparam name="TResult">
		/// The type of returned result.
		/// </typeparam>
		/// <param name="exceptionTypes">
		/// The types of exceptions to ignore.
		/// </param>
		/// <returns>
		/// The result evaluated from the waiting condition.
		/// </returns>
		/// <remarks>
		/// When <paramref name="timeout"/> is not specified, wait up to the default explicit wait seconds.
		/// </remarks>
		public static TResult WaitUntil<TResult>(
			this IWebDriver webDriver,
			Func<IWebDriver, TResult> condition,
			TimeSpan? timeout = null,
			params Type[] exceptionTypes)
		{
			Requires.NotNull(webDriver, nameof(webDriver));
			Requires.NotNull(condition, nameof(condition));

			webDriver.SetImplicitWait(TimeSpan.Zero);
			try
			{
				var wait = new WebDriverWait(
					webDriver,
					timeout ?? TimeSpan.FromSeconds(PlaybackSettings.Instance.ExplicitlyWaitSecond));
				if (exceptionTypes != null)
					wait.IgnoreExceptionTypes(exceptionTypes);
				return wait.Until(condition);
			}
			finally
			{
				webDriver.RestoreImplicitTimeout();
			}
		}

		/// <summary>
		/// Waits until the condition evaluated.
		/// </summary>
		/// <param name="webElement">
		/// The search context as of a <see cref="IWebElement"/>.
		/// </param>
		/// <param name="driver">
		/// The webdriver used for waiting.
		/// </param>
		/// <param name="condition">
		/// The condition.
		/// </param>
		/// <param name="timeout">
		/// The maximum waiting timeout for the specified condition to occur.
		/// </param>
		/// <typeparam name="TResult">
		/// The type of returned result.
		/// </typeparam>
		/// <param name="exceptionTypes">
		/// The types of exceptions to ignore.
		/// </param>
		/// <returns>
		/// The result evaluated from the waiting condition.
		/// </returns>
		/// <remarks>
		/// When <paramref name="timeout"/> is not specified, wait up to the default explicit wait seconds.
		/// </remarks>
		public static TResult WaitUntil<TResult>(
			this IWebElement webElement,
			IWebDriver driver,
			Func<IWebElement, TResult> condition,
			TimeSpan? timeout = null,
			params Type[] exceptionTypes)
		{
			Requires.NotNull(webElement, nameof(webElement));
			Requires.NotNull(driver, nameof(driver));
			Requires.NotNull(condition, nameof(condition));

			driver.SetImplicitWait(TimeSpan.Zero);
			try
			{
				var webElementWait = new WebElementWait(
					webElement,
					timeout ?? TimeSpan.FromSeconds(PlaybackSettings.Instance.ExplicitlyWaitSecond));

				if (exceptionTypes != null)
					webElementWait.IgnoreExceptionTypes(exceptionTypes);

				return webElementWait.Until(condition);
			}
			finally
			{
				driver.RestoreImplicitTimeout();
			}
		}

		/// <summary>
		/// Checks for minimum permissible value.
		/// </summary>
		/// <param name="minimumPermissibleValue">
		/// The minimum permissible value.
		/// </param>
		/// <param name="value">
		/// The value to be validated.
		/// </param>
		/// <param name="parameterName">
		/// The parameter name.
		/// </param>
		/// <returns>
		/// The <see cref="int"/>.
		/// </returns>
		private static int CheckForMinimumPermissibleValue(int minimumPermissibleValue, int value, string parameterName)
		{
			if (value == -1)
			{
				return int.MaxValue;
			}

			if (value >= minimumPermissibleValue)
			{
				return value;
			}

			throw new ArgumentException(
				string.Format(
					CultureInfo.CurrentCulture,
					"Invalid value {0} for parameter {1}.",
					value,
					(object)parameterName),
				parameterName);
		}
	}
}
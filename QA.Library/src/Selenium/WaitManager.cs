namespace Clix.QA.Selenium
{
	using System;
	using System.Collections.Generic;

	using OpenQA.Selenium;

	/// <summary>
	/// The <see cref="IWebDriver"/> waiting manager.
	/// </summary>
	/// <remarks>
	/// Manage either implicit wait, or explicit wait, or the combination of the two.
	/// </remarks>
	public static class WaitManager
	{
		/// <summary>
		/// The driver with timeout dictionary.
		/// </summary>
		private static readonly Dictionary<IWebDriver, DriverWithTimeout> DriverWithTimeoutDictionary
			= new Dictionary<IWebDriver, DriverWithTimeout>();

		/// <summary>
		/// The get current implicit wait.
		/// </summary>
		/// <param name="driver">
		/// The driver.
		/// </param>
		/// <returns>
		/// The <see cref="TimeSpan"/>.
		/// </returns>
		public static TimeSpan GetCurrentImplicitWait(this IWebDriver driver)
		{
			if (driver == null)
				throw new ArgumentNullException(nameof(driver));
			DriverWithTimeout driverWithTimeout = GetOrCreate(driver);
			return driverWithTimeout.CurrentImplicitTimeout();
		}

		/// <summary>
		/// The restore implicit timeout.
		/// </summary>
		/// <param name="driver">
		/// The driver.
		/// </param>
		public static void RestoreImplicitTimeout(this IWebDriver driver)
		{
			if (driver == null)
				throw new ArgumentNullException(nameof(driver));
			if (!driver.DriverHasTimeout())
				return;

			DriverWithTimeout driverWithTimeout = GetOrCreate(driver);
			driverWithTimeout.RestoreImplicitTimeout();
		}

		/// <summary>
		/// The set implicit wait.
		/// </summary>
		/// <param name="driver">
		/// The driver.
		/// </param>
		/// <param name="timeout">
		/// The timeout.
		/// </param>
		/// <returns>
		/// The <see cref="ITimeouts"/>.
		/// </returns>
		public static ITimeouts SetImplicitWait(this IWebDriver driver, TimeSpan timeout)
		{
			if (driver == null)
				throw new ArgumentNullException(nameof(driver));

			DriverWithTimeout driverWithTimeout = GetOrCreate(driver);
			return driverWithTimeout.SetImplicitTimeout(timeout);
		}

		/// <summary>
		/// The driver has timeout.
		/// </summary>
		/// <param name="driver">
		/// The driver.
		/// </param>
		/// <returns>
		/// The <see cref="bool"/>.
		/// </returns>
		private static bool DriverHasTimeout(this IWebDriver driver)
		{
			if (driver == null)
				throw new ArgumentNullException(nameof(driver));
			return DriverWithTimeoutDictionary.ContainsKey(driver);
		}

		/// <summary>
		/// The get or create.
		/// </summary>
		/// <param name="driver">
		/// The driver.
		/// </param>
		/// <returns>
		/// The <see cref="DriverWithTimeout"/>.
		/// </returns>
		private static DriverWithTimeout GetOrCreate(IWebDriver driver)
		{
			if (driver.DriverHasTimeout())
				return DriverWithTimeoutDictionary[driver];

			DriverWithTimeout driverWithTimeout = new DriverWithTimeout(driver);
			DriverWithTimeoutDictionary.Add(driver, driverWithTimeout);
			return driverWithTimeout;
		}

		/// <summary>
		/// The driver with timeout.
		/// </summary>
		private class DriverWithTimeout
		{
			/// <summary>
			/// The implicit timeouts.
			/// </summary>
			private readonly Stack<TimeSpan> implicitTimeouts = new Stack<TimeSpan>();

			/// <summary>
			/// Initializes a new instance of the <see cref="DriverWithTimeout"/> class.
			/// </summary>
			/// <param name="driver">
			/// The driver.
			/// </param>
			internal DriverWithTimeout(IWebDriver driver)
			{
				this.Driver = driver;
			}

			/// <summary>
			/// Gets the driver.
			/// </summary>
			private IWebDriver Driver { get; }

			/// <summary>
			/// The current implicit timeout.
			/// </summary>
			/// <returns>
			/// The <see cref="TimeSpan"/>.
			/// </returns>
			public TimeSpan CurrentImplicitTimeout()
			{
				return this.implicitTimeouts.Count == 0
							? TimeSpan.Zero
							: this.implicitTimeouts.Peek();
			}

			/// <summary>
			/// The restore implicit timeout.
			/// </summary>
			public void RestoreImplicitTimeout()
			{
				if (this.implicitTimeouts.Count < 2) // Current timeout is the only entry.
					return;

				this.implicitTimeouts.Pop(); // Remove the current timeout entry.
				TimeSpan timeout = this.implicitTimeouts.Peek();

				this.Driver.Manage().Timeouts().ImplicitWait = timeout;
			}

			/// <summary>
			/// The set implicit timeout.
			/// </summary>
			/// <param name="timeout">
			/// The timeout.
			/// </param>
			/// <returns>
			/// The <see cref="ITimeouts"/>.
			/// </returns>
			public ITimeouts SetImplicitTimeout(TimeSpan timeout)
			{
				ITimeouts driverTimeouts = this.Driver.Manage().Timeouts();
				driverTimeouts.ImplicitWait = timeout;
				this.implicitTimeouts.Push(timeout);

				return driverTimeouts;
			}
		}
	}
}
namespace Clix.QA.Selenium
{
	using System;

	using Autofac;

	using OpenQA.Selenium;

	/// <summary>
	/// The page object resolving helper.
	/// </summary>
	public static class PageResolveExtensions
	{
		/// <summary>
		/// Resolve a page map with a specified web driver.
		/// </summary>
		/// <param name="componentContext">
		///     The component context.
		/// </param>
		/// <param name="driver">
		/// The web driver.
		/// </param>
		/// <typeparam name="TPage">
		/// The page type.
		/// </typeparam>
		/// <returns>
		/// The <typeparamref name="TPage"/>.
		/// </returns>
		public static TPage ResolvePage<TPage>(this IComponentContext componentContext, IWebDriver driver)
			where TPage : PageBase
		{
			if (driver == null)
			{
				throw new ArgumentNullException(nameof(driver));
			}

			return componentContext
				.Resolve<TPage>(TypedParameter.From(driver));
		}

		/// <summary>
		/// Resolve a page map with a specified web driver token.
		/// </summary>
		/// <param name="componentContext">
		/// The component context.
		/// </param>
		/// <param name="driverToken">
		/// The driver token.
		/// </param>
		/// <typeparam name="TPage">
		/// The page type.
		/// </typeparam>
		/// <returns>
		/// The <typeparamref name="TPage"/>.
		/// </returns>
		public static TPage ResolvePage<TPage>(this IComponentContext componentContext, Guid driverToken = default(Guid))
			where TPage : PageBase
		{
			if (componentContext == null)
				throw new ArgumentNullException(nameof(componentContext));

			IWebDriver driver = componentContext.ResolveWebDriver(driverToken);

			return componentContext
				.ResolvePage<TPage>(driver);
		}
	}
}

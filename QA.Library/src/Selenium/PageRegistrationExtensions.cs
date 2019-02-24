namespace Clix.QA.Selenium
{
	using System;

	using Autofac;

	/// <summary>
	/// The page registration extensions.
	/// </summary>
	public static class PageRegistrationExtensions
	{
		/// <summary>
		/// To register a page object.
		/// </summary>
		/// <param name="containerBuilder">
		/// The container builder.
		/// </param>
		/// <typeparam name="TPage">
		/// The page type.
		/// </typeparam>
		public static void RegisterPage<TPage>(this ContainerBuilder containerBuilder)
			where TPage : PageBase
		{
			if (containerBuilder == null)
				throw new ArgumentNullException("containerBuilder");

			containerBuilder.RegisterType<TPage>();
		}
	}
}
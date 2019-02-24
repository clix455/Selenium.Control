using System;

namespace Clix.QA.Selenium
{
	using Autofac;

	/// <summary>
	/// The default web driver factory registration module.
	/// </summary>
	public class WebdriverFactoryModule : Module
	{
		/// <summary>
		/// Register out of the box web driver factories.
		/// </summary>
		/// <param name="builder">
		/// The builder.
		/// </param>
		protected override void Load(ContainerBuilder builder)
		{
			if (builder == null)
				throw new ArgumentNullException(nameof(builder));

			base.Load(builder);

			builder.RegisterType<WebDriverFactoryChrome>()
				.Keyed<WebDriverFactory>(BrowserTypes.Chrome)
				.SingleInstance();
			builder.RegisterType<WebDriverFactoryFirefox>()
				.Keyed<WebDriverFactory>(BrowserTypes.Firefox)
				.SingleInstance();
			builder.RegisterType<WebDriverFactoryEdge>()
				.Keyed<WebDriverFactory>(BrowserTypes.WindowsEdge)
				.SingleInstance();
			builder.RegisterType<WebDriverFactoryFirefox>()
				.Keyed<WebDriverFactory>(BrowserTypes.Phantomjs)
				.SingleInstance();

			builder.RegisterType<WebdriverFactoryManager>()
				.SingleInstance();
		}
	}
}

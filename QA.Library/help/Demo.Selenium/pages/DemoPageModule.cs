using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Selenium.pages
{
	using Autofac;

	using Clix.QA.Selenium;

	class DemoPageModule : Module
	{
		/// <summary>
		/// Override to add registrations to the container.
		/// </summary>
		/// <remarks>
		/// Note that the ContainerBuilder parameter is unique to this module.
		/// </remarks>
		/// <param name="builder">
		/// The builder through which components can be
		///     registered.
		/// </param>
		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);
			var allPageTypes = this.ThisAssembly.GetExportedTypes()
				.Where(t => t.IsSubclassOf(typeof(PageBase)));

			foreach (Type pageType in allPageTypes)
			{
				builder.RegisterType(pageType);
			}
		}
	}
}

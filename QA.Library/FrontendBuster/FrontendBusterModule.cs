using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Clix.QA.Selenium;

namespace FrontendBuster
{
    public class FrontendBusterModule : Module
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
            var allWebControlType = this.ThisAssembly.GetExportedTypes()
                .Where(t => t.IsSubclassOf(typeof(IControl)));

            foreach (Type webControlType in allWebControlType)
            {
                builder.RegisterType(webControlType);
            }
        }
    }
}

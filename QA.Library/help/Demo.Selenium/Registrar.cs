namespace Demo.Selenium
{
	using System;
	using System.Collections.Generic;

	using Autofac;
	using Autofac.Core;

	/// <summary>
	///     The component registrar.
	/// </summary>
	public class Registrar
	{
		/// <summary>
		/// The container builder.
		/// </summary>
		private ContainerBuilder ContainerBuilder { get; } = new ContainerBuilder();

		/// <summary>
		///     The modules.
		/// </summary>
		private readonly List<IModule> modules = new List<IModule>();

		/// <summary>
		///     The initialization status.
		/// </summary>
		private bool initialized;

		/// <summary>
		///     The _root container.
		/// </summary>
		private IContainer rootContainer;

		/// <summary>
		///     Initialises a new instance of the <see cref="Registrar" /> class.
		/// </summary>
		private Registrar()
		{
		}

		/// <summary>
		///     The instance.
		/// </summary>
		public static Registrar Instance { get; } = new Registrar();

		/// <summary>
		/// The new lifetime scope.
		/// </summary>
		public static ILifetimeScope NewLifetimeScope => Instance.RootContainer.BeginLifetimeScope();

		/// <summary>
		///     The root container.
		/// </summary>
		public IContainer RootContainer
		{
			get
			{
				if (!this.initialized)
				{
					this.Initialize();
				}
				return this.rootContainer;
			}
		}

		/// <summary>
		/// Register a new module.
		/// </summary>
		/// <param name="module">
		/// The module.
		/// </param>
		/// <returns>
		/// The Registrar itself with the new module registered.
		/// </returns>
		public Registrar RegisterModule(IModule module)
		{
			if (this.initialized)
				throw new InvalidOperationException("Registar already initialized.");

			this.ContainerBuilder.RegisterModule(module);

			return this;
		}

		public Registrar RegisterType(Action<ContainerBuilder> action)
		{
			if (this.initialized)
				throw new InvalidOperationException("Registar already initialized.");

			action(this.ContainerBuilder);

			return this;
		}

		/// <summary>
		///     Initialize the root container.
		/// </summary>
		private void Initialize()
		{
			this.rootContainer = this.ContainerBuilder.Build();
			this.initialized = true;
		}
	}
}

namespace Clix.QA.Selenium
{
	using System;

	using Autofac;

	/// <summary>
	/// The playback module.
	/// </summary>
	public class PlaybackModule : Module
	{
		/// <summary>
		/// Register UI tools components.
		/// </summary>
		/// <param name="builder">
		/// The builder.
		/// </param>
		protected override void Load(ContainerBuilder builder)
		{
			if (builder == null)
				throw new ArgumentNullException("builder");

			base.Load(builder);
		}
	}
}

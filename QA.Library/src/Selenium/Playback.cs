namespace Clix.QA.Selenium
{
	using System;
	using System.Threading;

	/// <summary>
	///     The playback configuration.
	/// </summary>
	public static class Playback
	{
		/// <summary>
		/// Initializes static members of the <see cref="Playback"/> class.
		/// </summary>
		static Playback()
		{
			Initialize();
		}

		/// <summary>
		///     Gets the playback settings.
		/// </summary>
		private static PlaybackSettings PlaybackSettings => PlaybackSettings.Instance;

		/// <summary>
		///     Reset the playback settings per application configurations.
		/// </summary>
		public static void Initialize()
		{
			PlaybackSettings.Initialize();
		}

		/// <summary>
		///     Wait for default polling interval.
		/// </summary>
		public static void Wait()
		{
			Wait(PlaybackSettings.PollingInterval);
		}

		/// <summary>
		/// Wait until the specified timeout.
		/// </summary>
		/// <param name="milliseconds">
		/// The milliseconds.
		/// </param>
		public static void Wait(int milliseconds)
		{
			double totalMilliseconds = milliseconds * PlaybackSettings.WaitTimeMultiplier;

			Thread.Sleep(TimeSpan.FromMilliseconds(totalMilliseconds));
		}

		/// <summary>
		/// Wait until the specified timeout.
		/// </summary>
		/// <param name="timeout">
		/// The timeout.
		/// </param>
		public static void Wait(TimeSpan timeout)
		{
			double totalMilliseconds =
				timeout.TotalMilliseconds * PlaybackSettings.WaitTimeMultiplier;

			Thread.Sleep(TimeSpan.FromMilliseconds(totalMilliseconds));
		}
	}
}
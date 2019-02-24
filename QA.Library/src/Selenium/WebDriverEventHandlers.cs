namespace Clix.QA.Selenium
{
	using System;

	using OpenQA.Selenium.Support.Events;

	/// <summary>
	/// The web driver event handlers.
	/// </summary>
	internal static class WebDriverEventHandlers
	{
		/// <summary>
		/// The take screen shot.
		/// </summary>
		/// <returns>
		/// The <see cref="EventHandler"/>.
		/// </returns>
		public static EventHandler<WebDriverExceptionEventArgs> TakeScreenShot()
		{
			return TakeScreenShot;
		}

		/// <summary>
		/// The take screen shot.
		/// </summary>
		/// <param name="sender">
		/// The sender.
		/// </param>
		/// <param name="args">
		/// The args.
		/// </param>
		private static void TakeScreenShot(object sender, WebDriverExceptionEventArgs args)
		{
			// args.Driver.TakeScreenshot();

			// Manage snapshots using a snapshot manager. the manager also is responsbile for saving snapshots when requried.
		}
	}
}
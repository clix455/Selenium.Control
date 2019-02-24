namespace Clix.Utilities
{
	using System.Collections.Specialized;

	/// <summary>
	/// The settings helper.
	/// </summary>
	public static class SettingsHelper
	{
		/// <summary>
		/// Gets the configuration option value.
		/// </summary>
		/// <param name="key">
		/// The key name.
		/// </param>
		/// <param name="defaultValue">
		/// The default value.
		/// </param>
		/// <param name="appSettings">
		/// The app settings.
		/// </param>
		/// <typeparam name="T">
		/// The value type.
		/// </typeparam>
		/// <returns>
		/// The value of type <typeparamref name="T"/> from configuration file.
		/// </returns>
		public static T GetConfigOptionValue<T>(string key, T defaultValue, NameValueCollection appSettings)
		{
			T settingValue;
			if (!ConfigurationHelper.GetValue(appSettings, key, out settingValue))
			{
				settingValue = defaultValue;
			}

			return settingValue;
		}

		/// <summary>
		/// TGets the configuration option value as double.
		/// </summary>
		/// <param name="optionName">
		/// The option name.
		/// </param>
		/// <param name="defaultValue">
		/// The default value.
		/// </param>
		/// <param name="appSettings">
		/// The app settings.
		/// </param>
		/// <returns>
		/// The <see cref="double"/>.
		/// </returns>
		public static double GetConfigOptionValueDouble(string optionName, double defaultValue, NameValueCollection appSettings)
		{
			double num;
			if (!ConfigurationHelper.GetValue(appSettings, optionName, out num) || num < 0.0)
			{
				num = defaultValue;
			}

			return num;
		}

		/// <summary>
		/// Gets the configuration option value as integer.
		/// </summary>
		/// <param name="key">
		/// The key.
		/// </param>
		/// <param name="defaultValue">
		/// The default value.
		/// </param>
		/// <param name="appSettings">
		/// The app settings.
		/// </param>
		/// <returns>
		/// The <see cref="int"/>.
		/// </returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "int", Justification = "Reviewed, ok here.")]
		public static int GetConfigOptionValueInt(string key, int defaultValue, NameValueCollection appSettings)
		{
			int settingValue;
			if (!ConfigurationHelper.GetValue(appSettings, key, out settingValue) || settingValue < 0)
			{
				settingValue = defaultValue;
			}

			return settingValue;
		}
	}
}

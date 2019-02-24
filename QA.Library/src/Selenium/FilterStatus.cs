namespace Clix.QA.Selenium
{
	/// <summary>
	/// The filter status defined for table headers where Filter control is applicable.
	/// </summary>
	public enum FilterStatus
	{
		/// <summary>
		///     Transit or unknown filter status.
		/// </summary>
		Unknow = 0,

		/// <summary>
		///     When the table column does not have a filter control.
		/// </summary>
		None = 1,

		/// <summary>
		///     When the table column filter is toggled off.
		/// </summary>
		Off = 2,

		/// <summary>
		///     When the table column filter is toggled on
		/// </summary>
		On = 3
	}
}
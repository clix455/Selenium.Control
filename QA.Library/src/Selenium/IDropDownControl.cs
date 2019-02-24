namespace Clix.QA.Selenium
{
	/// <summary>
	/// The DropDownControl interface.
	/// </summary>
	public interface IDropDownControl
	{
		/// <summary>
		/// Gets or sets a value indicating whether expanded.
		/// </summary>
		bool Expanded { get; set; }

		/// <summary>
		/// Selects element by its index.
		/// </summary>
		/// <param name="index">
		/// The element index.
		/// </param>
		void SelectByIndex(int index);

		/// <summary>
		/// Selects element by its text.
		/// </summary>
		/// <param name="text">
		/// The element text.
		/// </param>
		void SelectByText(string text);
	}
}
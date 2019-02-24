namespace Clix.QA.Selenium
{
	/// <summary>
	/// The CollectionControl interface.
	/// </summary>
	public interface ICollectionControl
	{
		/// <summary>
		/// The select by index.
		/// </summary>
		/// <param name="index">
		/// The index.
		/// </param>
		void SelectByIndex(int index);

		/// <summary>
		/// Selects element by its one-based index.
		/// </summary>
		/// <param name="position">
		/// The element position.
		/// </param>
		void SelectByPosition(int position);

		/// <summary>
		/// The select by text.
		/// </summary>
		/// <param name="text">
		/// The text.
		/// </param>
		void SelectByText(string text);
	}
}
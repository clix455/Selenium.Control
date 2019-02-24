namespace Clix.QA.Selenium
{
	using OpenQA.Selenium;

	/// <summary>
	/// The check box control.
	/// </summary>
	public class CheckBoxControl : ElementControl
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CheckBoxControl"/> class.
		/// </summary>
		/// <param name="wrappedElement">
		/// The wrapped web element.
		/// </param>
		/// <param name="parent">
		/// The parent.
		/// </param>
		/// <remarks>
		/// The wrapped element is the root for the new element control. Any children controls can be directly found/searched
		///     from it.
		/// </remarks>
		public CheckBoxControl(IWebElement wrappedElement, IControl parent)
			: base(wrappedElement, parent)
		{
		}

		/// <summary>
		/// Gets or sets a value indicating whether checked.
		/// </summary>
		public virtual bool Checked
		{
			get
			{
				return this.WrappedElement.Selected;
			}

			set
			{
				// Doing nothing
				if (value.Equals(this.Checked))
					return;

				this.WrappedElement.WaitClickable();
				this.WrappedElement.Click();
			}
		}
	}
}
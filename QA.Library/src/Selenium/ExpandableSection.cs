namespace Clix.QA.Selenium
{
	using OpenQA.Selenium;

	/// <summary>
	/// The expandable section.
	/// </summary>
	public abstract class ExpandableSection : ElementControl, IExpandableSection
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ExpandableSection"/> class.
		/// </summary>
		/// <param name="wrappedElement">
		/// The wrapped element.
		/// </param>
		/// <param name="parent">
		/// The parent.
		/// </param>
		protected ExpandableSection(IWebElement wrappedElement, IControl parent)
			: base(wrappedElement, parent)
		{
		}

		/// <summary>
		/// Gets or sets a value indicating whether expanded.
		/// </summary>
		public virtual bool Expanded
		{
			get
			{
				IWebElement expandingElement = this.WrappedElement.FindElement(By.CssSelector(".fa-plus"));
				return expandingElement.GetAttribute("class").Contains("ng-hide");
			}

			set
			{
				if (value.Equals(this.Expanded))
					return;

				IWebElement sectionHeader = this.WrappedElement.FindElement(By.CssSelector(".page-section-header"));

				switch (value)
				{
					case true:
						{
							sectionHeader.WaitDisplayed();
							sectionHeader.Click();
							this.WaitForCondition(o => o.Expanded, PlaybackSettings.Instance.ExplicitlyWaitSecond * 1000);
							break;
						}

					case false:
						{
							sectionHeader.WaitDisplayed();
							sectionHeader.Click();
							this.WaitForCondition(o => !o.Expanded, PlaybackSettings.Instance.ExplicitlyWaitSecond * 1000);
							break;
						}
				}
			}
		}
	}
}
namespace Clix.QA.Selenium
{
	using System;
	using System.Linq;

	using OpenQA.Selenium;

	/// <summary>
	///     The toggle control.
	/// </summary>
	public abstract class ToggleableCollection : CollectionControl, IToggleableCollection
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ToggleableCollection"/> class.
		/// </summary>
		/// <param name="parentElement">
		/// The parent element.
		/// </param>
		protected ToggleableCollection(IWebElement parentElement)
			: base(parentElement)
		{
		}

		/// <summary>
		/// Initialises a new instance of the <see cref="ToggleableCollection"/> class.
		/// </summary>
		/// <param name="parentElement">
		/// The parent element.
		/// </param>
		/// <param name="webDriver">
		/// The web driver instance.
		/// </param>
		protected ToggleableCollection(IWebElement parentElement, IWebDriver webDriver)
			: this(parentElement)
		{
			this.Driver = webDriver;
		}

		/// <summary>
		/// Gets or sets the driver.
		/// 
		/// </summary>
		protected IWebDriver Driver { get; set; }

		/// <summary>
		///     Gets or sets a value indicating whether expanded.
		/// </summary>
		public virtual bool Expanded
		{
			get
			{
				string expandingAttribute = this.OptionsContainer.GetAttribute("class");
				if (expandingAttribute == null)
					return false;
				string[] attributes = expandingAttribute.Split(' ');
				return attributes.Any(item => item.Equals("active", StringComparison.CurrentCultureIgnoreCase));
			}

			set
			{
				// Nothing to do
				if (value == this.Expanded)
					return;

				this.ToggleElement.WaitClickable().Click();
				WaitConditions.WaitForCondition(
					() => this.Expanded.Equals(value),
					PlaybackSettings.Instance.ExplicitlyWaitSecond * 1000);
			}
		}

		/// <summary>
		///     Gets the toggle element.
		/// </summary>
		protected abstract IWebElement ToggleElement { get; }

		/// <summary>
		/// Selects element by its zero-based index.
		/// </summary>
		/// <param name="index">
		/// The element index.
		/// </param>
		public override void SelectByIndex(int index)
		{
			this.ToggleOn();
			base.SelectByIndex(index);
		}

		/// <summary>
		/// Selects element by its one-based index.
		/// </summary>
		/// <param name="position">
		/// The element position.
		/// </param>
		public override void SelectByPosition(int position)
		{
			this.ToggleOn();
			base.SelectByPosition(position);
		}

		/// <summary>
		/// Selects element by its text.
		/// </summary>
		/// <param name="text">
		/// The element text.
		/// </param>
		public override void SelectByText(string text)
		{
			this.ToggleOn();
			base.SelectByText(text);
		}

		/// <summary>
		/// Toggle on/off the toggleable control.
		/// </summary>
		/// <param name="on">
		/// The on/off switch, default is on.
		/// </param>
		public void ToggleOn(bool on = true)
		{
			this.Expanded = on;
		}
	}
}
namespace Clix.QA.Selenium
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Runtime.CompilerServices;

	using OpenQA.Selenium;

	using Validation;

	/// <summary>
	///     The table control.
	/// </summary>
	public abstract class TableControl : ElementControl
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TableControl"/> class.
		/// </summary>
		/// <param name="wrappedElement">
		/// The wrapped element.
		/// </param>
		/// <param name="parent">
		/// The parent.
		/// </param>
		protected TableControl(IWebElement wrappedElement, IControl parent)
			: base(wrappedElement, parent)
		{
		}

		/// <summary>
		///     Gets the table rows.
		/// </summary>
		public virtual ReadOnlyCollection<IWebElement> Rows =>
			this.TableContents.FindElements(By.CssSelector("tr"));

		/// <summary>
		///     Gets the table contents.
		/// </summary>
		public virtual IWebElement TableContents =>
			this.FindElement(By.CssSelector("tbody"));

		/// <summary>
		///     Gets the table header controls.
		/// </summary>
		public virtual IEnumerable<TableHeaderControl> TableHeaderControls
		{
			get
			{
				IWebElement headerRowElement = this.FindElement(this.HeaderRowLocator);

				return headerRowElement.FindElements(this.HeaderElementLocator)
					.Select((element, i) =>
					{
						var tableHeaderControl = new TableHeaderControl(element, this)
						{
							DisplayName = this.GetHeaderName(element),
							Position = i + 1 // The header control position starts from 1.
						};
						return tableHeaderControl;
					})
					.ToList();
			}
		}

		/// <summary>
		///     Gets the header row locator.
		/// </summary>
		protected virtual By HeaderRowLocator => By.CssSelector("thead tr");

		/// <summary>
		/// Gets the header element locator.
		/// </summary>
		protected virtual By HeaderElementLocator => By.TagName("th");

		/// <summary>
		/// Gets the row <see cref="IWebElement"/> by its postion.
		/// </summary>
		/// <param name="rowPosition">
		/// The 1-based row position.
		/// </param>
		/// <returns>
		/// The <see cref="IWebElement"/> at postion specified by the <paramref name="rowPosition"/>.
		/// </returns>
		[IndexerName("TableItem")]
		public virtual IWebElement this[int rowPosition] => this.GetRow(rowPosition);

		/// <summary>
		/// Gets the data cell <see cref="IWebElement"/> by its row and cell postion.
		/// </summary>
		/// <param name="rowPosition">
		/// The 1-based row position.
		/// </param>
		/// <param name="cellPosition">
		/// The 1-based cell position.
		/// </param>
		/// <returns>
		/// The <see cref="IWebElement"/> at postion specified by the <paramref name="rowPosition"/> and <paramref name="cellPosition"/>.
		/// </returns>
		[IndexerName("TableItem")]
		public virtual IWebElement this[int rowPosition, int cellPosition] =>
			this.GetDataCellElement(this[rowPosition], cellPosition);

		/// <summary>
		/// Gets the data cell element by its table header name and data cell text.
		/// </summary>
		/// <param name="headerName">
		/// The table header name.
		/// </param>
		/// <param name="cellText">
		/// The data cell text.
		/// </param>
		/// <returns>
		/// The <see cref="IWebElement"/> represents the matched data cell element.
		/// </returns>
		public virtual IWebElement GetDataCellElement(string headerName, string cellText)
		{
			if (string.IsNullOrEmpty(headerName))
				throw new ArgumentException("Table header name cannot be null or empty.");

			return this.Rows
				.Select(row => this.GetDataCellElement(row, headerName))
				.FirstOrDefault(
					cellElement => cellElement.Text.Equals(cellText));
		}

		/// <summary>
		/// Gets the data cell element from the current row by its table header name.
		/// </summary>
		/// <param name="row">
		/// The current data row.
		/// </param>
		/// <param name="headerName">
		/// The table header name.
		/// </param>
		/// <returns>
		/// The <see cref="IWebElement"/> represents the matched data cell element.
		/// </returns>
		public virtual IWebElement GetDataCellElement(IWebElement row, string headerName)
		{
			if (row == null)
				throw new ArgumentNullException(nameof(row));
			if (string.IsNullOrEmpty(headerName))
				throw new ArgumentException("Table header name cannot be null or empty.");

			int index = this.GetDataCellIndex(headerName);

			IWebElement dataCellElement = row.FindElement(By.CssSelector($"td:nth-of-type({index})"));
			return dataCellElement;
		}

		/// <summary>
		/// Gets the data cell element from the current row by its table header position.
		/// </summary>
		/// <param name="row">
		/// The current data row.
		/// </param>
		/// <param name="position">
		/// The 1-based column position.
		/// </param>
		/// <returns>
		/// The <see cref="IWebElement"/> represents the matched data cell element.
		/// </returns>
		public virtual IWebElement GetDataCellElement(IWebElement row, int position)
		{
			if (row == null)
				throw new ArgumentNullException(nameof(row));

			IWebElement dataCellElement = row.FindElement(By.CssSelector($"td:nth-of-type({position})"));
			return dataCellElement;
		}

		/// <summary>
		/// Gets data cell's index from table header name.
		/// </summary>
		/// <param name="headerName">
		/// The header name.
		/// </param>
		/// <returns>
		/// The 1-based data cell index, return -1 if header name does not exist.
		/// </returns>
		public virtual int GetDataCellIndex(string headerName)
		{
			if (string.IsNullOrEmpty(headerName))
				throw new ArgumentException("Table header name cannot be null or empty.");
			TableHeaderControl tableHeader = this.GetTableHeaderControl(headerName);

			return tableHeader?.Position ?? -1;
		}

		/// <summary>
		/// Gets table row using its position.
		/// </summary>
		/// <param name="position">
		/// The 1-based row position.
		/// </param>
		/// <returns>
		/// The <see cref="IWebElement"/> represents the row identified by the row position.
		/// </returns>
		public virtual IWebElement GetRow(int position)
		{
			if (position > this.Rows.Count)
				throw new IndexOutOfRangeException("The row index cannot be bigger than total row count.");
			return this.Rows[position - 1];
		}

		/// <summary>
		/// Get the table header control by header name.
		/// </summary>
		/// <param name="headerName">
		/// The header name.
		/// </param>
		/// <returns>
		/// A <see cref="TableHeaderControl"/> that matches the specified <paramref name="headerName"/>.
		/// </returns>
		public virtual TableHeaderControl GetTableHeaderControl(string headerName)
		{
			if (string.IsNullOrEmpty(headerName))
				return null;

			return this.TableHeaderControls.FirstOrDefault(
				item =>
				item.DisplayName.Equals(headerName, StringComparison.CurrentCultureIgnoreCase));
		}

		/// <summary>
		/// Gets the <see cref="TableHeaderControl"/> by its position.
		/// </summary>
		/// <param name="position">
		/// The header position.
		/// </param>
		/// <returns>
		/// The <see cref="TableHeaderControl"/> at the specified position.
		/// </returns>
		public virtual TableHeaderControl GetTableHeaderControl(int position)
		{
			return this.TableHeaderControls.ElementAtOrDefault(position - 1);
		}

		/// <summary>
		/// Gets the header name from the header element.
		/// </summary>
		/// <param name="element">
		/// The header element.
		/// </param>
		/// <returns>
		/// The table header name.
		/// </returns>
		protected virtual string GetHeaderName(IWebElement element)
		{
			if (element == null)
				throw new ArgumentNullException(nameof(element));
			return element.GetAttribute("data-title");
		}

		/// <summary>
		/// Get table row by table header and the value for that column identified by the <see cref="TableHeaderControl"/>
		/// </summary>
		/// <param name="header">
		/// The <see cref="TableHeaderControl"/>
		/// </param>
		/// <param name="value">
		/// The cell value for the specified column.
		/// </param>
		/// <returns>
		/// The row that has the specified cell with the given value.
		/// </returns>
		protected virtual IWebElement GetRow(TableHeaderControl header, string value)
		{
			Requires.NotNull(header, nameof(header));
			Requires.NotNullOrEmpty(value, nameof(value));

			int headerPosition = header.Position;
			return this.Rows.FirstOrDefault(
				record =>
				{
					IWebElement cellElement = this.GetDataCellElement(record, headerPosition);
					string cellText = cellElement?.Text;
					return !string.IsNullOrEmpty(cellText)
							&& cellText.Trim().Equals(value, StringComparison.CurrentCultureIgnoreCase);
				});
		}

		/// <summary>
		/// Get table row by table header and the value for that column identified by the header name.
		/// </summary>
		/// <param name="headerName">
		/// The table header name for the column.
		/// </param>
		/// <param name="value">
		/// The cell value for the specified column.
		/// </param>
		/// <returns>
		/// The row that has the specified cell with the given value.
		/// </returns>
		protected virtual IWebElement GetRow(string headerName, string value)
		{
			Requires.NotNullOrEmpty(headerName, nameof(headerName));
			Requires.NotNullOrEmpty(value, nameof(value));

			TableHeaderControl headerControl = this.GetTableHeaderControl(headerName);
			return this.GetRow(headerControl, value);
		}
	}
}
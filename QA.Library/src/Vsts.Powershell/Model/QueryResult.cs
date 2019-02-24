// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryResult.cs" company="">
//   
// </copyright>
// <summary>
//   The query result.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Clix.Vsts.Powershell.Model
{
	using System.Collections.Generic;

	/// <summary>
	/// The query result.
	/// </summary>
	/// <typeparam name="T">
	/// </typeparam>
	public class QueryResult<T>
	{
		/// <summary>
		/// Gets or sets the count.
		/// </summary>
		public int? Count { get; set; }

		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		public IReadOnlyCollection<T> Value { get; set; }
	}
}
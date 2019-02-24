namespace Clix.Utilities
{
	using System;
	using System.Collections.ObjectModel;

	/// <summary>
	/// The string helper.
	/// </summary>
	public static class StringHelper
	{
		/// <summary>
		/// Gets the punctuations char collection.
		/// </summary>
		public static ReadOnlyCollection<char> Punctuations =>
			new ReadOnlyCollection<char>("!@#$%^&*()_-+=[{]};:>|./?".ToCharArray());

		/// <summary>
		/// Gets the digits char collection.
		/// </summary>
		public static ReadOnlyCollection<char> Digits =>
			new ReadOnlyCollection<char>("0123456789".ToCharArray());

		/// <summary>
		/// Gets the lower case letters char collection.
		/// </summary>
		public static ReadOnlyCollection<char> LowerLetters =>
			new ReadOnlyCollection<char>("abcdefghijklmnopqrstuvwxyz".ToCharArray());

		/// <summary>
		/// Gets the upper case letters char collection.
		/// </summary>
		public static ReadOnlyCollection<char> UpperLetters =>
			new ReadOnlyCollection<char>("ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray());

		/// <summary>
		/// To randomize the provided string by appending a random <see cref="Guid"/>.
		/// </summary>
		/// <param name="value">
		/// The original value.
		/// </param>
		/// <param name="length">
		/// The length of the random string to be attached.
		/// </param>
		/// <param name="appending">
		/// The flag that indicating whether to append or prefix the random string.
		/// </param>
		/// <returns>
		/// The randomized <see cref="string"/>.
		/// </returns>
		public static string Random(this string value, int length = 8, bool appending = true)
		{
			if (string.IsNullOrEmpty(value))
				return string.Empty;
			string randomValue = Guid.NewGuid().ToString("N");
			randomValue = randomValue.Substring(0, Math.Min(length, randomValue.Length));
			return appending ? $"{value}.{randomValue}" : $"{randomValue}.{value}";
		}

		/// <summary>
		/// Create a random string by trimming a new Guid.
		/// </summary>
		/// <param name="length">
		/// The length of the output result.
		/// </param>
		/// <returns>
		/// The <see cref="string"/>. Which cannot be longer than a Guid.
		/// </returns>
		public static string Random(int length = 8)
		{
			string randomValue = Guid.NewGuid().ToString("N");
			return randomValue.Substring(0, Math.Min(length, randomValue.Length));
		}

		/// <summary>
		/// Create a random string from seed.
		/// </summary>
		/// <param name="seed">
		/// The seed.
		/// </param>
		/// <param name="length">
		/// The string length.
		/// </param>
		/// <returns>
		/// The <see cref="string"/> with the specified length, contains only the characters provided by <paramref name="seed"/>.
		/// </returns>
		/// <remarks>
		/// Use this only if you have to select a set of characters, otherwise use the Random(int length = 8) method.
		/// Which utilizes the Guid generation algorithm, and has higher resolution up to 100 nanosecond.
		/// </remarks>
		public static string RandomFromSeed(string seed = "ABCDEFGHIJKLMNOPQRSTUVWXYZ123456789", int length = 8)
		{
			if (string.IsNullOrEmpty(seed))
				throw new ArgumentException("Seed cannot be null or empty");

			var random = new Random();
			int seedLength = seed.Length;

			char[] resultChars = new char[length];
			for (int i = 0; i < length; i++)
			{
				resultChars[i] = seed[random.Next(0, seedLength)];
			}

			return new string(resultChars);
		}
	}
}

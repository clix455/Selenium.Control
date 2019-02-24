// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProtectedSecret.cs" company="">
//   
// </copyright>
// <summary>
//   The protected secret.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Clix.Security
{
	using System;
	using System.Security;
	using System.Security.Cryptography;
	using System.Text;

	/// <summary>
	/// Provides methods for encrypting and decrypting data. 
	/// </summary>
	public static class ProtectedSecret
	{
		/// <summary>
		/// Protect (encrypt) a secret using DPAPI.
		/// </summary>
		/// <param name="secret">
		/// The secret to encrypt. Cannot be null.
		/// </param>
		/// <returns>
		/// The encrypted secret, as a Base64 string.
		/// </returns>
		public static string Protect(string secret)
		{
			if (secret == null)
				throw new ArgumentNullException(nameof(secret));

			return Protect(Encoding.UTF8.GetBytes(secret));
		}

		/// <summary>
		/// Unprotect (decrypt) a protected secret using DPAPI.
		/// </summary>
		/// <param name="protectedSecret">
		/// The protected secret to decrypt (as a Base64 string).
		/// </param>
		/// <returns>
		/// The origianl secret, as a string.
		/// </returns>
		public static string UnProtect(string protectedSecret)
		{
			if (protectedSecret == null)
				throw new ArgumentNullException(nameof(protectedSecret));

			return UnProtect(Convert.FromBase64String(protectedSecret));
		}

		/// <summary>
		/// Protect (encrypt) a secret using DPAPI.
		/// </summary>
		/// <param name="secret">
		/// The secret to encrypt. Cannot be null.
		/// </param>
		/// <returns>
		/// The encrypted secret, as a Base64 string.
		/// </returns>
		private static string Protect(byte[] secret)
		{
			if (secret == null)
				throw new ArgumentNullException(nameof(secret));

			byte[] protectedData = ProtectedData.Protect(secret, null, DataProtectionScope.CurrentUser);

			return Convert.ToBase64String(protectedData);
		}

		/// <summary>
		/// Unprotect (decrypt) a secret using DPAPI.
		/// </summary>
		/// <param name="protectedSecret">
		/// The protected secret to decrypt (as a Base64 string).
		/// </param>
		/// <returns>
		/// The encrypted secret, as a string.
		/// </returns>
		private static string UnProtect(byte[] protectedSecret)
		{
			if (protectedSecret == null)
				throw new ArgumentNullException(nameof(protectedSecret));

			byte[] unprotectedData = ProtectedData.Unprotect(
				protectedSecret,
				null,
				DataProtectionScope.CurrentUser);

			return Encoding.UTF8.GetString(unprotectedData);
		}

		/// <summary>
		///		Convert a normal string to a secured string.
		/// </summary>
		/// <param name="normalString">
		///		The normal string to convert.
		/// </param>
		/// <returns>
		///		A secured string.
		/// </returns>
		static SecureString ConvertToSecureString(string normalString)
		{
			if (normalString == null)
				throw new ArgumentNullException(nameof(normalString));

			var secureString = new SecureString();
			foreach (var c in normalString.ToCharArray())
				secureString.AppendChar(c);

			return secureString;
		}

		/// <summary>
		///		Protect (encrypt) a password using DPAPI.
		/// </summary>
		/// <param name="password">
		///		The password to encrypt.
		/// 
		///		Cannot be null.
		/// </param>
		/// <returns>
		///		The encrypted password, as a Base64 string.
		/// </returns>
		private static string ProtectPassword(SecureString password)
		{
			if (password == null)
				throw new ArgumentNullException(nameof(password));

			return Protect(password.ToUnSecureString());
		}
	}
}
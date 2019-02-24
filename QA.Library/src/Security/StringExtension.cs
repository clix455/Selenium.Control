namespace Clix.Security
{
	using System;
	using System.Runtime.InteropServices;
	using System.Security;

	static class StringExtension
	{
		/// <summary>
		/// Convert a secure string to normal string
		/// </summary>
		/// <param name="secstrPassword">
		/// A secure string
		/// </param>
		/// <returns>
		/// Converted string
		/// </returns>
		public static string ToUnSecureString(this SecureString secstrPassword)
		{
			IntPtr unmanagedString = IntPtr.Zero;
			try
			{
				unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secstrPassword);
				return Marshal.PtrToStringUni(unmanagedString);
			}
			finally
			{
				Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
			}
		}
	}
}

namespace Clix.Vsts.Powershell.Connection
{
	/// <summary>
	/// The authentication type.
	/// </summary>
	public enum AuthenticationType
	{
		/// <summary>
		/// Using powershell credential for authentication.
		/// </summary>
		PSCredential = 1, 

		/// <summary>
		/// The basic authentication type.
		/// </summary>
		Basic = 2, 

		/// <summary>
		/// The oauth JWT authentication type.
		/// </summary>
		OAuth = 3
	}
}
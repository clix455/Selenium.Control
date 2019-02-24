namespace Clix.Vsts.Powershell.Commands
{
	using System;
	using System.Management.Automation;

	using Model;

	/// <summary>
	/// The base class for command that requires project in their api.
	/// </summary>
	public abstract class WithProjectCmdlet : VstsCmdlet
	{
		/// <summary>
		/// The team project.
		/// </summary>
		[Parameter(ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
		public Project Project { get; set; }

		/// <summary>
		/// The team project id.
		/// </summary>
		[Parameter(ValueFromPipeline = true)]
		public Guid ProjectId { get; set; }

		/// <summary>
		/// The team project name.
		/// </summary>
		[Parameter(ValueFromPipeline = true)]
		public string ProjectName { get; set; }

		/// <summary>
		/// Team project ID or name.
		/// </summary>
		private string ResolvedProject
		{
			get
			{
				if (this.Project != null)
					return this.Project.Id;
				if (!(this.ProjectId.Equals(default(Guid))))
					return this.ProjectId.ToString("D");
				if (!string.IsNullOrWhiteSpace(this.ProjectName))
					return this.ProjectName;
				return null;
			}
		}

		/// <summary>
		/// To include the team project id or name in the request uri.
		/// </summary>
		/// <param name="requestUri">
		/// The original request uri.
		/// </param>
		/// <returns>
		/// The uri that includes the team project id or name.
		/// </returns>
		protected string GetUriWithProject(string requestUri)
		{
			string resolvedProject = this.ResolvedProject;
			if(string.IsNullOrWhiteSpace(resolvedProject))
				throw new ParameterBindingException("Can not determine the team project. Either a team project object, or project id, or project name should be provided.");
			return $"{resolvedProject}/{requestUri}";
		}
	}
}
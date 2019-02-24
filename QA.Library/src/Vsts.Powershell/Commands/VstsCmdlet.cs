// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VstsCmdlet.cs" company="">
//   
// </copyright>
// <summary>
//   The vsts cmdlet.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Clix.Vsts.Powershell.Commands
{
	using System;
	using System.Management.Automation;
	using System.Net;
	using System.Net.Http;
	using System.Net.Http.Formatting;
	using System.Net.Http.Headers;
	using System.Text;
	using System.Threading.Tasks;

	using Model;

	using Newtonsoft.Json;
	using Newtonsoft.Json.Serialization;

	using Powershell.Connection;

	/// <summary>
	/// The vsts cmdlet.
	/// </summary>
	public abstract class VstsCmdlet :PSCmdlet
	{
		/// <summary>
		/// Temporarily using api 2.0 for the requests. TODO
		/// </summary>
		protected const string ApiVersion = "api-version=2.0";

		/// <summary>
		/// The _media formatter.
		/// </summary>
		protected static JsonMediaTypeFormatter MediaFormatter { get; }

		/// <summary>
		///     Is this object been disposed.
		/// </summary>
		private bool _disposed;

		/// <summary>
		/// Initializes static members of the <see cref="VstsCmdlet"/> class.
		/// </summary>
		static VstsCmdlet()
		{
			MediaFormatter = new JsonMediaTypeFormatter();
			MediaFormatter.SerializerSettings = new JsonSerializerSettings
			{
				ContractResolver = new CamelCasePropertyNamesContractResolver(), 
				NullValueHandling = NullValueHandling.Ignore
			};
		}

		/// <summary>
		/// Finalizes an instance of the <see cref="VstsCmdlet"/> class. 
		/// </summary>
		~VstsCmdlet()
		{
			this.Dispose(false);
		}

		/// <summary>
		/// Gets or sets the connection name.
		/// </summary>
		[Parameter]
		public string ConnectionName { get; set; }

		/// <summary>
		/// Gets the http client.
		/// </summary>
		protected HttpClient HttpClient
		{
			get
			{
				ConnectionSetting connectionSetting = this.GetConnection();

				HttpClientHandler innerHandler = new HttpClientHandler();

				switch (connectionSetting.AuthenticationType)
				{
					case AuthenticationType.PSCredential:
						{
							innerHandler.Credentials =
								new NetworkCredential(
									connectionSetting.UserName, 
									connectionSetting.Password);
							break;
						}

					case AuthenticationType.Basic:
						break;
					case AuthenticationType.OAuth:
						break;
					default:
						goto case AuthenticationType.PSCredential;
				}

				HttpClient client =
					new HttpClient(innerHandler)
					{
						BaseAddress = new Uri($"{connectionSetting.Instance}/{connectionSetting.TeamCollection}/")
					};
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				return client;
			}
		}

		private ConnectionSetting GetConnection()
		{
			ConnectionSetting connectionSetting =
				string.IsNullOrEmpty(this.ConnectionName)
					? ConnectionStore.GetDefaultConnectionSetting()
					: ConnectionStore.GetConnectionSetting(this.ConnectionName);

			if (connectionSetting == null)
			{
				StringBuilder messageBuilder = new StringBuilder("Cannot found a valid tfs/vsts connection.");

				messageBuilder.AppendLine(
					string.IsNullOrWhiteSpace(this.ConnectionName) 
					? "Default connection was not configured." 
					: $"No connection could be found matching the connection name '{this.ConnectionName}'");

				messageBuilder.AppendLine("Use the Get-VstsConnection command to retrieve a list of existing connection settings.");
				messageBuilder.AppendLine("Use the New-VstsConnection command to a new connection setting.");

				var exception = new ApplicationFailedException(messageBuilder.ToString());
				this.ThrowTerminatingError(
					new ErrorRecord(
						exception, 
						"FailedApiInvoke", 
						ErrorCategory.ConnectionError, 
						this.ConnectionName));
				return null;
			}
				
			return connectionSetting;
		}

		/// <summary>
		/// Formalize the request uri from parameters.
		/// </summary>
		protected abstract string  FormalizedRequest { get; }

		/// <summary>
		///     Dispose this instance.
		/// </summary>
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// The delete async.
		/// </summary>
		/// <param name="requestUri">
		/// The request uri.
		/// </param>
		/// <returns>
		/// The <see cref="Task"/>.
		/// </returns>
		/// <exception cref="Exception">
		/// </exception>
		protected async Task DeleteAsync(string requestUri)
		{
			var response = await this.HttpClient.DeleteAsync(requestUri);

			if (!response.IsSuccessStatusCode)
			{
				// TODO: improve error handling
				var errorResponse = await response.Content.ReadAsStringAsync();
				throw new Exception(errorResponse);
			}
		}

		/// <summary>
		/// Dispose this instance.
		/// </summary>
		/// <param name="disposing">
		/// This instance is disposing.
		/// </param>
		protected virtual void Dispose(bool disposing)
		{
			if (!this._disposed)
			{
				if (disposing)
				{
				}

				this._disposed = true;
			}
		}

		/// <summary>
		/// The get async.
		/// </summary>
		/// <param name="requestUri">
		/// The request uri.
		/// </param>
		/// <typeparam name="T">
		/// </typeparam>
		/// <returns>
		/// The <see cref="Task"/>.
		/// </returns>
		protected async Task<T> GetAsync<T>(string requestUri)
		{
			var	response = await this.HttpClient.GetAsync(requestUri);

			await this.TerminateIfResponseNotSuccess(response);

			return await response.Content.ReadAsAsync<T>(new[]
			{
				MediaFormatter
			});
		}

		/// <summary>
		/// Reports a terminating error when the response is not with success status code. 
		/// </summary>
		/// <param name="response">
		/// The http response message.
		/// </param>
		/// <returns>
		/// Async task.
		/// </returns>
		private async Task TerminateIfResponseNotSuccess(HttpResponseMessage response)
		{
			if (!response.IsSuccessStatusCode)
			{
				ResponseError error = await response.Content.ReadAsAsync<ResponseError>();
				var exception = new ApplicationFailedException(error.Message);
				this.ThrowTerminatingError(new ErrorRecord(
					exception, 
					"FailedApiRequest", 
					ErrorCategory.InvalidResult, 
					response));
			}
		}

		/// <summary>
		/// Get response as string
		/// </summary>
		/// <param name="requestUri">
		/// The request uri.
		/// </param>
		/// <returns>
		/// The task contains the result string.
		/// </returns>
		protected async Task<string> GetStringAsync(string requestUri)
		{
			HttpResponseMessage response = await this.HttpClient.GetAsync(requestUri);

			await this.TerminateIfResponseNotSuccess(response);

			return await response.Content.ReadAsStringAsync();
		}

		/// <summary>
		/// The post async.
		/// </summary>
		/// <param name="requestUri">
		/// The request uri.
		/// </param>
		/// <param name="payload">
		/// The payload.
		/// </param>
		/// <typeparam name="TPayload">
		/// </typeparam>
		/// <typeparam name="TResponse">
		/// </typeparam>
		/// <returns>
		/// The <see cref="Task"/>.
		/// </returns>
		protected async Task<TResponse> PostAsync<TPayload, TResponse>(string requestUri, TPayload payload)
		{
			var response = await this.HttpClient.PostAsync(requestUri, new ObjectContent<TPayload>(payload, MediaFormatter));

			await this.TerminateIfResponseNotSuccess(response);

			return await response.Content.ReadAsAsync<TResponse>(new[]
			{
				MediaFormatter
			});
		}

		/// <summary>
		/// The post async.
		/// </summary>
		/// <param name="requestUri">
		/// The request uri.
		/// </param>
		/// <param name="payload">
		/// The payload.
		/// </param>
		/// <typeparam name="TPayload">
		/// </typeparam>
		/// <returns>
		/// The <see cref="Task"/>.
		/// </returns>
		protected async Task PostAsync<TPayload>(string requestUri, TPayload payload)
		{
			var response = await this.HttpClient.PostAsync(requestUri, new ObjectContent<TPayload>(payload, MediaFormatter));

			await this.TerminateIfResponseNotSuccess(response);
		}
	}
}
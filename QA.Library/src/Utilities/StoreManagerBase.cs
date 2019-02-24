namespace Clix.Utilities
{
	using System;
	using System.IO;
	using System.Threading;
	using System.Threading.Tasks;
	using System.Xml.Serialization;

	/// <summary>
	///     The xml store manager base.
	/// </summary>
	/// <typeparam name="TDataStore">
	///     The data store type.
	/// </typeparam>
	public class StoreManagerBase<TDataStore>
		where TDataStore : IDataStore, new()
	{
		/// <summary>
		///     A simple asynchronous lock used to synchronise access to the XML data store.
		/// </summary>
		private readonly AsyncLock storeLock = new AsyncLock();

		/// <summary>
		///     The full path to the file that holds the XML-serialised EMaaS store data.
		/// </summary>
		private readonly string storeXmlFile;

		/// <summary>
		///     Initializes a new instance of the <see cref="StoreManagerBase{TDataStore}" /> class.
		/// </summary>
		/// <param name="storeXmlFile">
		///     The store xml file.
		/// </param>
		protected StoreManagerBase(string storeXmlFile)
		{
			if (string.IsNullOrWhiteSpace(storeXmlFile))
			{
				throw new ArgumentException(
					"Argument cannot be null, empty, or composed entirely of whitespace: 'storeXmlFile'.",
					nameof(storeXmlFile));
			}

			this.storeXmlFile = storeXmlFile;
		}

		/// <summary>
		///     Gets the full path to the file that holds hold XML-serialised DMS store data.
		/// </summary>
		public virtual string StoreXmlFile => this.storeXmlFile;

		/// <summary>
		///     Gets a value indicating whether does the XML-serialised store data file exist?
		/// </summary>
		public virtual bool StoreFileExists => File.Exists(this.storeXmlFile);

		/// <summary>
		///     Asynchronously save the store contents as XML to the specified file.
		/// </summary>
		/// <param name="dataStore">
		///     The data store to save.
		/// </param>
		/// <param name="cancellationToken">
		///     An optional cancellation token that can be used to cancel the asynchronous operation.
		/// </param>
		/// <returns>
		///     A <see cref="Task" /> representing the asynchronous operation.
		/// </returns>
		public virtual async Task SaveAsync(
			TDataStore dataStore,
			CancellationToken cancellationToken = default(CancellationToken))
		{
			if (dataStore == null)
				throw new ArgumentNullException(nameof(dataStore));

			if (cancellationToken.IsCancellationRequested)
				return;

			using (await this.storeLock.LockAsync())
			{
				if (this.StoreFileExists)
					File.Delete(this.storeXmlFile);

				cancellationToken.ThrowIfCancellationRequested();

				var bufferStream = new MemoryStream();
				await Task.Factory.StartNew(
					() =>
					{
						var storeSerializer = new XmlSerializer(typeof(TDataStore));
						storeSerializer.Serialize(bufferStream, dataStore);
					},
					cancellationToken);

				using (bufferStream)
				{
					bufferStream.Seek(0, SeekOrigin.Begin);

					using (
						var storeStream = new FileStream(
							this.storeXmlFile,
							FileMode.CreateNew,
							FileAccess.Write,
							FileShare.None,
							1024,
							FileOptions.Asynchronous))
					{
						await bufferStream.CopyToAsync(storeStream, 1024, cancellationToken);
						await storeStream.FlushAsync(cancellationToken);
					}
				}
			}
		}

		/// <summary>
		///     Asynchronously load store data from the store XML file.
		/// </summary>
		/// <param name="cancellationToken">
		///     An optional cancellation token that can be used to cancel the asynchronous operation.
		/// </param>
		/// <returns>
		///     A <see cref="Task" /> representing the asynchronous operation.
		/// </returns>
		public virtual async Task<TDataStore> LoadAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			if (!this.StoreFileExists)
				throw new FileNotFoundException(string.Format("Cannot find XML data file '{0}'.", this.storeXmlFile), this.storeXmlFile);

			using (await this.storeLock.LockAsync())
			{
				cancellationToken.ThrowIfCancellationRequested();

				using (var bufferStream = new MemoryStream())
				{
					using (var storeStream = new FileStream(
						this.storeXmlFile,
						FileMode.Open,
						FileAccess.Read,
						FileShare.Read,
						1024,
						FileOptions.Asynchronous))
					{
						await storeStream.CopyToAsync(bufferStream, 1024, cancellationToken);
						await bufferStream.FlushAsync(cancellationToken);
					}

					cancellationToken.ThrowIfCancellationRequested();

					bufferStream.Seek(0, SeekOrigin.Begin);

					return
						await Task<TDataStore>.Factory.StartNew(
							() =>
							{
								var storeSerializer = new XmlSerializer(typeof(TDataStore));

								// ReSharper disable once AccessToDisposedClosure
								return (TDataStore)storeSerializer.Deserialize(bufferStream);
							},
							cancellationToken);
				}
			}
		}
	}
}

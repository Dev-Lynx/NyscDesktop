using NyscIdentify.Common.Infrastructure.Extensions;
using NyscIdentify.Common.Infrastructure.Extensions.UnityExtensions;
using NyscIdentify.Common.Infrastructure.Models;
using NyscIdentify.Common.Infrastructure.Models.Entities;
using NyscIdentify.Common.Infrastructure.Models.Interfaces;
using NyscIdentify.Common.Infrastructure.Services.Interfaces;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace NyscIdentify.Common.Infrastructure.Services
{
    [AutoBuild]
    public class LocalResourceManager : IResourceManager
    {
        #region Properties

        #region Bindables
        public ResourceConfiguration Configuration => ConfigurationManager.
            CurrentConfiguration.ResourceConfiguration;
        #endregion

        #region Services
        [DeepDependency]
        ILoggerFacade Logger { get; }
        [DeepDependency]
        IConfigurationManager ConfigurationManager { get; }
        [DeepDependency]
        IDatabaseManager DatabaseManager { get; }
        #endregion

        #region Internals
        HttpClient Client { get; set; }
        bool ClientIsDead { get; set; } = true;

        Dictionary<string, IResource> ResolutionQueue { get; } = 
            new Dictionary<string, IResource>();
        #endregion

        #endregion

        #region Methods

        #region IResourceManager Implementation
        /*
        public async Task<bool> ResolveResource(IResource resource)
        {
            



            if (Downloading.ContainsKey(resource.Url)) return false;

            Downloading.Add(resource.Url, resource);
            resource.IsBusy = true;
            resource.Status = RequestStatus.Pending;

            ResourceBase res = await DatabaseManager.GetResourceByUrl(resource.Url);

            if (!File.Exists(res.LocalPath))
            {
                await DatabaseManager.RemoveResource(res);
                res = null;
            }

            if (res != null)
            {
                resource.IsBusy = false;
                resource.Status = RequestStatus.Success;
                resource.Progress = 100;
                resource.LocalPath = res.LocalPath;
                Downloading.Remove(resource.Url);
                return true;
            }

            try
            {
                string ext = Path.GetExtension(resource.Url);

                string name = Guid.NewGuid().ToString() + ext;
                while (File.Exists(Path.Combine(Configuration.Location, name)))
                    name = Guid.NewGuid().ToString() + ext;


                string downloadPath = Path.Combine(Core.TEMP_DIR, name);



                Uri uri = new Uri(resource.Url);
                WebClient client = new WebClient();

                client.DownloadProgressChanged += ProgressChanged;
                client.DownloadFileCompleted += DownloadCompleted;

                resource.LocalPath = downloadPath;
                client.DownloadFileAsync(uri, downloadPath, resource.Url);

                while (resource.Status == RequestStatus.Pending)
                    await Task.Delay(100);

                Downloading.Remove(resource.Url);
                return resource.Status == RequestStatus.Success;
            }
            catch (Exception ex)
            {
                Logger.Error($"An error occured while resolving a resource.\n{ex}");
            }

            return false;
        }
        */

        public async Task ResolveResource(IResource resource, bool highPriority = false)
        {
            if (ResolutionQueue.ContainsKey(resource.Url)) return;

            resource.Status = RequestStatus.Pending;

            ResourceBase res = await DatabaseManager.GetResourceByUrl(resource.Url);

            if (res != null && !File.Exists(res.LocalPath))
            {
                await DatabaseManager.RemoveResource(res);
                res = null;
            }

            if (res != null)
            {
                resource.LocalPath = res.LocalPath;
                resource.Status = RequestStatus.Success;
                return;
            }


            if (highPriority)
                await FinalizeResource(resource, await 
                    DownloadResource(resource));
            else Queue(resource);
        }
        #endregion

        void Queue(IResource resource)
        {
            ResolutionQueue.Add(resource.Url, resource);

            Logger.Debug($"({resource.Url}) has been added to the queue");

            if (ClientIsDead)
                PopQueue();
        }



        async void PopQueue()
        {
            if (!ResolutionQueue.Any())
            {
                DisposeClient();
                return;
            }

            KeyValuePair<string, IResource> values = ResolutionQueue.First();
            IResource resource = values.Value;
            ResolutionQueue.Remove(values.Key);

            string tempPath = await DownloadResource(resource);
            await FinalizeResource(resource, tempPath);
            
            PopQueue();
        }

        async Task FinalizeResource(IResource resource, string tempPath)
        {
            resource.Progress = 0;
            bool success = !string.IsNullOrWhiteSpace(tempPath);

            if (!success) { resource.Status = RequestStatus.Failed; return; }

            string dest = Path.Combine(Configuration.Location,
                Path.GetFileName(tempPath));

            try { File.Move(tempPath, dest); }
            catch (Exception ex) { Logger.Debug($"An unexpected error " +
                $"occured while copying a file.\n{ex}"); }

            if (!File.Exists(dest)) { resource.Status = RequestStatus.Failed; return; }

            resource.LocalPath = dest;
            resource.Status = RequestStatus.Success;


            if (resource is ResourceBase r) await DatabaseManager.AddResource(r);
        }

        /// <summary>
        /// Downloads a resource to a temporary path
        /// </summary>
        /// <param name="resource">The resource to download.</param>
        /// <returns>The location the resource has been downloaded to. 
        /// The download failed if this value is empty.</returns>
        async Task<string> DownloadResource(IResource resource)
        {
            if (ClientIsDead) CreateClient();
            string downloadPath = string.Empty;
            try
            {
                string url = resource.Url;
                downloadPath = Core.CreateTemporaryPath(resource.Url);

                using (HttpResponseMessage response = await Client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
                using (Stream stream = await response.Content.ReadAsStreamAsync())
                using (FileStream outputStream = new FileStream(downloadPath, FileMode.Create, FileAccess.Write))
                {
                    byte[] buffer = new byte[4096];
                    int read = -1;
                    long totalBytesRecieved = 0;
                    long? totalBytes = 0;

                    while ((read = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        totalBytesRecieved += read;
                        totalBytes = response.Content.Headers.ContentLength;

                        outputStream.Write(buffer, 0, read);

                        if (totalBytes.HasValue)
                        {
                            double progress = ((double)totalBytesRecieved / totalBytes.Value) * 100;
                            if (progress > 20)
                                Core.Dispatcher.Invoke(() => resource.Progress = progress);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Debug($"An error occured while downloading a resource\n{ex}");
                downloadPath = string.Empty;
            }

            return downloadPath;
        }

        /// <summary>
        /// Configures a new http client.
        /// </summary>
        /// <returns></returns>
        void CreateClient()
        {
            ClientIsDead = false;
            Client = new HttpClient() { Timeout = TimeSpan.FromMinutes(5) };
        }

        /// <summary>
        /// Disposes the current http client.
        /// </summary>
        void DisposeClient()
        {
            if (Client != null) Client.Dispose();
            ClientIsDead = true;
        }
        #endregion
    }
}

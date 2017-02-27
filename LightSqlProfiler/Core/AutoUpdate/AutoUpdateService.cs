using LightSqlProfiler.Properties;
using log4net;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Reflection;

namespace LightSqlProfiler.Core.AutoUpdate
{
    /// <summary>
    /// Handles application updates
    /// </summary>
    internal class AutoUpdateService
    {
        private static readonly ILog Log = LogManager.GetLogger(nameof(AutoUpdateService));

        /// <summary>
        /// Gets remote manifest file
        /// </summary>
        /// <returns></returns>
        private UpdateManifest ReadManifest()
        {
            try
            {
                var wc = new WebClient();
                wc.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);

                var raw = wc.DownloadString(Settings.Default.ManifestUrl);
                return JsonConvert.DeserializeObject<UpdateManifest>(raw);
            }
            catch (Exception ex)
            {
                Log.Warn($"Cannot read manifest file from: {Settings.Default.ManifestUrl}", ex);
            }

            return null;
        }

        /// <summary>
        /// Checks remote manifest if newer version available
        /// </summary>
        /// <returns>true/false - if update available; null - error reading manifest</returns>
        public bool? IsUpdateAvailable()
        {
            Log.Debug("Checking for updates");
            var manifest = ReadManifest();

            if (manifest == null)
                return null;

            Log.Debug($"Got update manifest: {manifest.LatestVersion}");
            var current = Assembly.GetExecutingAssembly().GetName().Version;
            return manifest.GetVersion() > current;
        }

        /// <summary>
        /// Executes instructions specified in update-manifest location
        /// Typically opens an URL
        /// </summary>
        public void DownloadAndRun()
        {
            try
            {
                var manifest = ReadManifest();
                System.Diagnostics.Process.Start(manifest.Location);
            }
            catch (Exception ex)
            {
                Log.Warn($"Cannot launch download location page", ex);
            }
        }
    }
}

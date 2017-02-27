using System;

namespace LightSqlProfiler.Core.AutoUpdate
{
    /// <summary>
    /// Application update manifest structure
    /// </summary>
    internal class UpdateManifest
    {
        /// <summary>
        /// Textual version data as written in manifest
        /// </summary>
        public string LatestVersion { get; set; }

        /// <summary>
        /// URL where newer version can be found (can be a generic page)
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Converts textual version string to full Version type
        /// </summary>
        /// <returns></returns>
        public Version GetVersion()
        {
            return new Version(LatestVersion);
        }
    }
}

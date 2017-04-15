// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileManager.cs" company="">
//   
// </copyright>
// <summary>
//   The file manager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace HostFileCreate.Managers
{
    #region

    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    #endregion

    /// <summary>
    /// The file manager.
    /// </summary>
    internal class FileManager
    {
        /// <summary>
        /// The create host file.
        /// </summary>
        /// <param name="siteHostFile">
        /// The site host file.
        /// </param>
        /// <param name="fileName">
        /// The file name.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        internal bool CreateHostFile(ConcurrentDictionary<string, List<string>> siteHostFile, string fileName)
        {
            if (siteHostFile != null && fileName != null)
            {
                FileStream fs1 = new FileStream($"C:\\{fileName}.txt", FileMode.OpenOrCreate, FileAccess.Write);
                using (var writer = new StreamWriter(fs1))
                {
                    foreach (var pair in siteHostFile)
                    {
                        writer.WriteAsync($"## {pair.Key} ##\r\n");
                        writer.WriteLine($"## {pair.Key} ##\r\n");
                        foreach (var hostentry in pair.Value)
                        {
                            writer.WriteLine(hostentry);
                        }

                        writer.WriteLine("\r\n");
                    }
                }

                Console.WriteLine($"{fileName} completed\r\n");
                return true;
            }

            return false;
        }

        /// <summary>
        /// The site host file.
        /// </summary>
        /// <param name="listOfSiteNames">
        /// The list of site names.
        /// </param>
        /// <param name="serverName">
        /// The server name.
        /// </param>
        /// <param name="serverIpAddress">
        /// The server ip address.
        /// </param>
        /// <param name="serverManager">
        /// The server manager.
        /// </param>
        /// <returns>
        /// The <see cref="ConcurrentDictionary"/>.
        /// </returns>
        internal ConcurrentDictionary<string, List<string>> SiteHostFileData(
            List<string> listOfSiteNames,
            string serverName,
            string serverIpAddress,
            EnvironmentServerManager serverManager)
        {
            if (listOfSiteNames == null)
            {
                return null;
            }

            var threadSafeSiteHostFile = new ConcurrentDictionary<string, List<string>>();

            Parallel.ForEach(
                listOfSiteNames,
                siteName =>
                    {
                        var bindingsList = serverManager.ListOfBindings(serverName, siteName);

                        var listToBeAddedToHosts =
                            (from binding in bindingsList
                             where !string.IsNullOrEmpty(binding)
                             select $"{serverIpAddress} {binding}").ToList();

                        threadSafeSiteHostFile.TryAdd(siteName, listToBeAddedToHosts);
                    });
            return threadSafeSiteHostFile;
        }
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServerManager.cs" company="">
//   
// </copyright>
// <summary>
//   The environment server manager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace HostFileCreate.Managers
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.Web.Administration;

    #endregion

    /// <summary>
    /// The environment server manager.
    /// </summary>
    internal class EnvironmentServerManager
    {
        /// <summary>
        /// The get site name from server.
        /// </summary>
        /// <param name="serverName">
        /// The server name.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        internal List<string> GetSiteNameFromServer(string serverName)
        {
            var iisManager = IisManager(serverName);
            if (iisManager == null)
            {
                return null;
            }

            var listOfSites = iisManager.Sites;

            var siteNames = new List<string>();

            siteNames.AddRange(listOfSites.Select(site => site.Name));
            if (siteNames.Count != 0)
            {
                return siteNames;
            }

            Console.WriteLine($"There are no sites hosted on {serverName}.\r\n");

            return null;
        }

        /// <summary>
        /// The list of bindings.
        /// </summary>
        /// <param name="serverName">
        /// The server name.
        /// </param>
        /// <param name="siteName">
        /// The site name.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        internal List<string> ListOfBindings(string serverName, string siteName)
        {
            var iisManager = IisManager(serverName);
            if (iisManager != null)
            {
                var siteCollection = iisManager.Sites;

                var listOfBindings = new List<string>();

                Parallel.ForEach(
                    siteCollection,
                    new ParallelOptions { MaxDegreeOfParallelism = 10 },
                    sites =>
                        {
                            if (sites.Name == siteName)
                            {
                                var bindingsCollection = sites.Bindings;
                                listOfBindings.AddRange(bindingsCollection.Select(binding => binding.Host));
                            }
                        });

                if (listOfBindings.Count == 0)
                {
                    Console.WriteLine($"There are no bindings for {siteName}.\r\n");
                    return null;
                }

                Console.WriteLine(
                    $"Able to get a list of the bindings from server {serverName} for the site {siteName}. \r\n");
                var listOfDistinctBindings = listOfBindings.Distinct().ToList();
                return listOfDistinctBindings;
            }

            return null;
        }

        /// <summary>
        /// The iis manager.
        /// </summary>
        /// <param name="serverName">
        /// The server name.
        /// </param>
        /// <returns>
        /// The <see cref="ServerManager"/>.
        /// </returns>
        private static ServerManager IisManager(string serverName)
        {
            try
            {
                var iisManager = ServerManager.OpenRemote(serverName);
                return iisManager;
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}
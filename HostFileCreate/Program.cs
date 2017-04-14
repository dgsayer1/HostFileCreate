// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="">
//   
// </copyright>
// <summary>
//   The program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace HostFileCreate
{
    #region

    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using HostFileCreate.Managers;

    #endregion

    /// <summary>
    /// The program.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// The main.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        public static void Main(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                Console.WriteLine("Hello World!");
                Console.ReadLine();
            }
            else
            {
                Stopwatch S1 = new Stopwatch();
                S1.Start();
                Parallel.ForEach(args, new ParallelOptions { MaxDegreeOfParallelism = 10 }, DoIt);
                S1.Stop();
                Console.WriteLine($"This took {S1.ElapsedMilliseconds} ms to complete.");
            }
        }

        /// <summary>
        /// The do it.
        /// </summary>
        /// <param name="arg">
        /// The arg.
        /// </param>
        internal static void DoIt(string arg)
        {
            var serverName = arg;
            var serverManager = new EnvironmentServerManager();
            var createServerHost = new FileManager();
            var ipManager = new IpManager();

            var listOfSiteNames = serverManager.GetSiteNameFromServer(serverName);
            var serverIpAddress = ipManager.GetIpAddress(serverName);
            var siteHostFile = createServerHost.SiteHostFileData(
                listOfSiteNames,
                serverName,
                serverIpAddress,
                serverManager);
            var fileCreated = createServerHost.CreateHostFile(siteHostFile, serverName);

            if (fileCreated)
            {
                Process.Start($"{serverName}.txt");
            }
        }
    }
}
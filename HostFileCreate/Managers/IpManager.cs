// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IpManager.cs" company="">
//   
// </copyright>
// <summary>
//   The ip manager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace HostFileCreate.Managers
{
    #region

    using System;
    using System.Net;

    #endregion

    /// <summary>
    /// The ip manager.
    /// </summary>
    internal class IpManager
    {
        /// <summary>
        /// The get ip address.
        /// </summary>
        /// <param name="serverName">
        /// The server name.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        internal string GetIpAddress(string serverName)
        {
            try
            {
                var ipAddress = Dns.GetHostAddressesAsync(serverName).Result;

                return ipAddress[0].ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
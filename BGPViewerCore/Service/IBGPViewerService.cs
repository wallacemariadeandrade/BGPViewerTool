using System;
using System.Collections.Generic;
using BGPViewerCore.Model;

namespace BGPViewerCore.Service
{
    /// <summary>
    /// Provides methods to get relevant data about Autonomous Systems (AS's), prefixes and IP addresses.
    /// </summary>
    public interface IBGPViewerService
    {
        /// <summary>
        /// Retrieve some personal data about ASN provided.
        /// </summary>
        /// <param name="asNumber">The AS number to get details about.</param>
        /// <returns> 
        /// An AsnDetailsModel containg some data like description, name, contacts, etc.
         /// </returns>
        AsnDetailsModel GetAsnDetails(int asNumber);

        /// <summary>
        /// Retrieve provided AS downstreams.
        /// </summary>
        /// <param name="asNumber">The AS number that is being looked for.</param>
        /// <returns> 
        /// Known IPv4 and IPv6 peers.
        /// </returns>
        /// <remarks>
        /// Tuple item1 contains IPv4 peers and item2 IPv6 peers.
        /// </remarks>        
        Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>> GetAsnDownstreams(int asNumber);

        /// <summary>
        /// Retrieve IX's where provided AS is present.
        /// </summary>
        /// <param name="asNumber">The IX's AS number participant.</param>
        /// <returns> 
        /// A collection containg known IX's where provided AS is present.
        /// </returns>
        IEnumerable<IxModel> GetAsnIxs(int asNumber);

        /// <summary>
        /// Retrieve provided AS peers.
        /// </summary>
        /// <param name="asNumber">The AS number that is being looked for.</param>
        /// <returns> 
        /// Known IPv4 and IPv6 peers.
        /// </returns>
        /// <remarks>
        /// Tuple item1 contains IPv4 peers and item2 IPv6 peers.
        /// </remarks>
        Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>> GetAsnPeers(int asNumber);
        
        /// <summary>
        /// Retrieve provided AS's known prefixes.
        /// </summary>
        /// <param name="asNumber">The AS number that prefixes are being looked for.</param>
        /// <returns> 
        /// Known IPv4 and IPv6 prefixes.
        /// </returns>
        AsnPrefixesModel GetAsnPrefixes(int asNumber);

        /// <summary>
        /// Retrieve provided AS upstreams.
        /// </summary>
        /// <param name="asNumber">The AS number that is being looked for.</param>
        /// <returns> 
        /// Known IPv4 and IPv6 peers.
        /// </returns>
        /// <remarks>
        /// Tuple item1 contains IPv4 peers and item2 IPv6 peers.
        /// </remarks>
        Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>> GetAsnUpstreams(int asNumber);

        /// <summary>
        /// Retrieve provided IP details.
        /// </summary>
        /// <param name="ipAddress">The IP address that is being looked for.</param>
        /// <returns> 
        /// Details about provided IP address like PTR record, country code, allocation prefix and so on.
        /// </returns>
        IpDetailModel GetIpDetails(string ipAddress);

        /// <summary>
        /// Retrieve provided prefix details.
        /// </summary>
        /// <param name="prefix">The prefix that is being looked for.</param>
        /// <param name="cidr">The prefix cidr.</param>
        /// <returns> 
        /// Details about provided prefix like parent AS numbers, name, description and so on.
        /// </returns>
        PrefixDetailModel GetPrefixDetails(string prefix, byte cidr);

        /// <summary>
        /// Searches data on API based on provided term.
        /// </summary>
        /// <param name="queryTerm">The query term that will be used to look for.</param>
        /// <returns> 
        /// Details about provided prefix like parent AS numbers, name, description and so on.
        /// </returns>
        /// <remarks>
        /// Common query terms are AS number, name, description, IP address and prefix.
        /// </remarks>
        SearchModel SearchBy(string queryTerm);
    }
}
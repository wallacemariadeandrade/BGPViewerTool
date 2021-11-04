using BGPViewerCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace BGPViewerCore.Service
{
    /// <summary>
    /// Provides methods to get relevant data about Autonomous Systems (AS's), prefixes and IP addresses.
    /// </summary>
    public class BGPViewerService : IBGPViewerService
    {
        private IBGPViewerApi _jsonApi;

        /// <summary>
        /// Constructs the service.
        /// </summary>
        /// <param name="api">Data provider JSON API.</param>
        public BGPViewerService(IBGPViewerApi api)
        {
            _jsonApi = api;
        }

        /// <summary>
        /// Retrieve some personal data about ASN provided.
        /// </summary>
        /// <param name="asNumber">The AS number to get details about.</param>
        /// <returns> 
        /// An AsnDetailsModel containg some data like description, name, contacts, etc.
         /// </returns>
        public AsnDetailsModel GetAsnDetails(int asNumber)
        {
            var jsonData = _jsonApi.RetrieveAsnDetails(asNumber);
            ValidateStatus(jsonData);
            var dataElement = jsonData.RootElement.GetProperty("data");   
            return new AsnDetailsModel 
            {
                ASN = asNumber,
                Name = dataElement.GetProperty("name").GetString(),
                Description = dataElement.GetProperty("description_short").GetString(),
                CountryCode = dataElement.GetProperty("rir_allocation").GetProperty("country_code").GetString(),
                EmailContacts = dataElement
                    .GetProperty("email_contacts")
                    .EnumerateArray()
                    .Select(email => email.GetString()),
                AbuseContacts = dataElement
                    .GetProperty("abuse_contacts")
                    .EnumerateArray()
                    .Select(email => email.GetString()),
                LookingGlassUrl = dataElement.GetProperty("looking_glass").GetString(),
            };
        }

        /// <summary>
        /// Retrieve provided AS's known prefixes.
        /// </summary>
        /// <param name="asNumber">The AS number that prefixes are being looked for.</param>
        /// <returns> 
        /// Known IPv4 and IPv6 prefixes.
        /// </returns>
        public AsnPrefixesModel GetAsnPrefixes(int asNumber)
        {
            var jsonData = _jsonApi.RetrieveAsnPrefixes(asNumber);
            ValidateStatus(jsonData);
            var dataElement = jsonData.RootElement.GetProperty("data");
            return new AsnPrefixesModel
            {
                ASN = asNumber,
                IPv4 = dataElement
                .GetProperty("ipv4_prefixes")
                .EnumerateArray()
                .Select(prefix => prefix.GetProperty("prefix").GetString()),
                IPv6 = dataElement
                .GetProperty("ipv6_prefixes")
                .EnumerateArray()
                .Select(prefix => prefix.GetProperty("prefix").GetString()),
            };
        } 

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
        public Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>> GetAsnPeers(int asNumber)
        {
            var jsonData = _jsonApi.RetrieveAsnPeers(asNumber);
            ValidateStatus(jsonData);
            var dataElement = jsonData.RootElement.GetProperty("data");
            return new Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>>(
                item1: ExtractInfoFromArray(dataElement.GetProperty("ipv4_peers")),
                item2: ExtractInfoFromArray(dataElement.GetProperty("ipv6_peers"))
            );
        }

        /// <summary>
        /// Retrieve provided AS upstreams.
        /// </summary>
        /// <param name="asNumber">The AS number that is being looked for.</param>
        /// <returns> 
        /// Known IPv4 and IPv6 upstreams.
        /// </returns>
        /// <remarks>
        /// Tuple item1 contains IPv4 upstreams and item2 IPv6 upstreams.
        /// </remarks>
        public Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>> GetAsnUpstreams(int asNumber)
        {
            var jsonData = _jsonApi.RetrieveAsnUpstreams(asNumber);
            ValidateStatus(jsonData);
            var dataElement = jsonData.RootElement.GetProperty("data");
            return new Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>>(
                item1: ExtractInfoFromArray(dataElement.GetProperty("ipv4_upstreams")),
                item2: ExtractInfoFromArray(dataElement.GetProperty("ipv6_upstreams"))
            );
        }

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
        public Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>> GetAsnDownstreams(int asNumber)
        {
            var jsonData = _jsonApi.RetrieveAsnDownstreams(asNumber);
            ValidateStatus(jsonData);
            var dataElement = jsonData.RootElement.GetProperty("data");
            return new Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>>(
                item1: ExtractInfoFromArray(dataElement.GetProperty("ipv4_downstreams")),
                item2: ExtractInfoFromArray(dataElement.GetProperty("ipv6_downstreams"))
            );
        }

        /// <summary>
        /// Retrieve IX's where provided AS is present.
        /// </summary>
        /// <param name="asNumber">The IX's AS number participant.</param>
        /// <returns> 
        /// A collection containg known IX's where provided AS is present.
        /// </returns>
        public IEnumerable<IxModel> GetAsnIxs(int asNumber)
        {
            var jsonData =  _jsonApi.RetrieveAsnIxs(asNumber);
            ValidateStatus(jsonData);
            var dataElement = jsonData.RootElement.GetProperty("data");
            foreach(var ixJson in dataElement.EnumerateArray())
                yield return new IxModel {
                    Name = ixJson.GetProperty("name").GetString(),
                    FullName = ixJson.GetProperty("name_full").GetString(),
                    CountryCode = ixJson.GetProperty("country_code").GetString(),
                    IPv4 = ixJson.GetProperty("ipv4_address").GetString(),
                    IPv6 = ixJson.GetProperty("ipv6_address").GetString(),
                    AsnSpeed = ixJson.GetProperty("speed").GetInt32()
                };
        }

        /// <summary>
        /// Retrieve provided IP details.
        /// </summary>
        /// <param name="ipAddress">The IP address that is being looked for.</param>
        /// <returns> 
        /// Details about provided IP address like PTR record, country code, allocation prefix and so on.
        /// </returns>
        public IpDetailModel GetIpDetails(string ipAddress)
        {
            var jsonData = _jsonApi.RetrieveIpDetails(ipAddress);
            ValidateStatus(jsonData);
            var dataElement = jsonData.RootElement.GetProperty("data");
            return new IpDetailModel 
            {
                IPAddress = ipAddress,
                RIRAllocationPrefix = dataElement.GetProperty("rir_allocation").GetProperty("prefix").GetString(),
                CountryCode = dataElement.GetProperty("rir_allocation").GetProperty("country_code").GetString(),
                PtrRecord = dataElement.GetProperty("ptr_record").GetString(),
                RelatedPrefixes = dataElement.GetProperty("prefixes")
                    .EnumerateArray()
                    .Select(x => new PrefixDetailModel {
                        Prefix = x.GetProperty("prefix").GetString(),
                        Name = x.GetProperty("name").GetString(),
                        Description = x.GetProperty("description").GetString(),
                        ParentAsns = new AsnModel[] {
                            new AsnModel {
                                ASN = x.GetProperty("asn").GetProperty("asn").GetInt32(),
                                Name = x.GetProperty("asn").GetProperty("name").GetString(),
                                Description = x.GetProperty("asn").GetProperty("name").GetString(),
                                CountryCode = x.GetProperty("asn").GetProperty("country_code").GetString(),
                            }
                        }
                    })
            };
        }

        /// <summary>
        /// Retrieve provided prefix details.
        /// </summary>
        /// <param name="prefix">The prefix that is being looked for.</param>
        /// <param name="cidr">The prefix cidr.</param>
        /// <returns> 
        /// Details about provided prefix like parent AS numbers, name, description and so on.
        /// </returns>
        public PrefixDetailModel GetPrefixDetails(string prefix, byte cidr)
        {
            var jsonData = _jsonApi.RetrievePrefixDetails(prefix, cidr);
            ValidateStatus(jsonData);
            var dataElement = jsonData.RootElement.GetProperty("data");
            return new PrefixDetailModel 
            {
                Name = dataElement.GetProperty("name").GetString(),
                Description = dataElement.GetProperty("description_short").GetString(),
                Prefix = $"{prefix}/{cidr}",
                ParentAsns = dataElement.GetProperty("asns")
                    .EnumerateArray()
                    .Select(asn => new AsnModel {
                        ASN = asn.GetProperty("asn").GetInt32(),
                        Name = asn.GetProperty("name").GetString(),
                        Description = asn.GetProperty("description").GetString(),
                        CountryCode = asn.GetProperty("country_code").GetString()
                    })
            };
        }

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
        public SearchModel SearchBy(string queryTerm)
        {
            var jsonData = _jsonApi.RetrieveSearchBy(queryTerm);
            ValidateStatus(jsonData);
            var dataElement = jsonData.RootElement.GetProperty("data");
            return new SearchModel
            {
                RelatedAsns = dataElement.GetProperty("asns")
                    .EnumerateArray()
                    .Select(asn => new AsnWithContactsModel {
                        ASN = asn.GetProperty("asn").GetInt32(),
                        Name = asn.GetProperty("name").GetString(),
                        Description = asn.GetProperty("description").GetString(),
                        CountryCode = asn.GetProperty("country_code").GetString(),
                        EmailContacts = asn
                            .GetProperty("email_contacts")
                            .EnumerateArray()
                            .Select(email => email.GetString()),
                        AbuseContacts = asn
                            .GetProperty("abuse_contacts")
                            .EnumerateArray()
                            .Select(email => email.GetString()),
                    }),
                IPv4 = dataElement.GetProperty("ipv4_prefixes")
                    .EnumerateArray()
                    .Select(x => new PrefixModel {
                        Prefix = x.GetProperty("prefix").GetString(),
                        Name = x.GetProperty("name").GetString(),
                        Description = x.GetProperty("description").GetString()
                    }),
                IPv6 = dataElement.GetProperty("ipv6_prefixes")
                    .EnumerateArray()
                    .Select(x => new PrefixModel {
                        Prefix = x.GetProperty("prefix").GetString(),
                        Name = x.GetProperty("name").GetString(),
                        Description = x.GetProperty("description").GetString()
                    }) 
            };
        }

        private IEnumerable<AsnModel> ExtractInfoFromArray(JsonElement jsonArrayElement)
            => jsonArrayElement.EnumerateArray()
                .Select(peer => new AsnModel {
                    ASN = peer.GetProperty("asn").GetInt32(),
                    Name = peer.GetProperty("name").GetString(),
                    Description = peer.GetProperty("description").GetString(),
                    CountryCode = peer.GetProperty("country_code").GetString()
                }
            );
    
        private void ValidateStatus(JsonDocument rawJson)
        {
            if(rawJson.RootElement.GetProperty("status").GetString() != "ok")
                throw new ArgumentException(rawJson.RootElement.GetProperty("status_message").GetString());
        }

        public void Dispose()
        {
            _jsonApi = null;
        }

        public async Task<AsnDetailsModel> GetAsnDetailsAsync(int asNumber)
        {
            var jsonData = await _jsonApi.RetrieveAsnDetailsAsync(asNumber);
            ValidateStatus(jsonData);
            var dataElement = jsonData.RootElement.GetProperty("data");   
            return new AsnDetailsModel 
            {
                ASN = asNumber,
                Name = dataElement.GetProperty("name").GetString(),
                Description = dataElement.GetProperty("description_short").GetString(),
                CountryCode = dataElement.GetProperty("rir_allocation").GetProperty("country_code").GetString(),
                EmailContacts = dataElement
                    .GetProperty("email_contacts")
                    .EnumerateArray()
                    .Select(email => email.GetString()),
                AbuseContacts = dataElement
                    .GetProperty("abuse_contacts")
                    .EnumerateArray()
                    .Select(email => email.GetString()),
                LookingGlassUrl = dataElement.GetProperty("looking_glass").GetString(),
            };
        }

        public async Task<Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>>> GetAsnDownstreamsAsync(int asNumber)
        {
            var jsonData = await _jsonApi.RetrieveAsnDownstreamsAsync(asNumber);
            ValidateStatus(jsonData);
            var dataElement = jsonData.RootElement.GetProperty("data");
            return new Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>>(
                item1: ExtractInfoFromArray(dataElement.GetProperty("ipv4_downstreams")),
                item2: ExtractInfoFromArray(dataElement.GetProperty("ipv6_downstreams"))
            );
        }

        public async Task<IEnumerable<IxModel>> GetAsnIxsAsync(int asNumber)
        {
            var jsonData = await _jsonApi.RetrieveAsnIxsAsync(asNumber);
            ValidateStatus(jsonData);
            var dataElement = jsonData.RootElement.GetProperty("data");
            return ExtractIxModelFromJsonArray(dataElement);
        }

        private IEnumerable<IxModel> ExtractIxModelFromJsonArray(JsonElement jsonArray)
        {
            foreach(var ixJson in jsonArray.EnumerateArray())
                yield return new IxModel {
                    Name = ixJson.GetProperty("name").GetString(),
                    FullName = ixJson.GetProperty("name_full").GetString(),
                    CountryCode = ixJson.GetProperty("country_code").GetString(),
                    IPv4 = ixJson.GetProperty("ipv4_address").GetString(),
                    IPv6 = ixJson.GetProperty("ipv6_address").GetString(),
                    AsnSpeed = ixJson.GetProperty("speed").GetInt32()
                };
        }

        public async Task<Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>>> GetAsnPeersAsync(int asNumber)
        {
            var jsonData = await _jsonApi.RetrieveAsnPeersAsync(asNumber);
            ValidateStatus(jsonData);
            var dataElement = jsonData.RootElement.GetProperty("data");
            return new Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>>(
                item1: ExtractInfoFromArray(dataElement.GetProperty("ipv4_peers")),
                item2: ExtractInfoFromArray(dataElement.GetProperty("ipv6_peers"))
            );
        }

        public async Task<AsnPrefixesModel> GetAsnPrefixesAsync(int asNumber)
        {
            var jsonData = await _jsonApi.RetrieveAsnPrefixesAsync(asNumber);
            ValidateStatus(jsonData);
            var dataElement = jsonData.RootElement.GetProperty("data");
            return new AsnPrefixesModel
            {
                ASN = asNumber,
                IPv4 = dataElement
                .GetProperty("ipv4_prefixes")
                .EnumerateArray()
                .Select(prefix => prefix.GetProperty("prefix").GetString()),
                IPv6 = dataElement
                .GetProperty("ipv6_prefixes")
                .EnumerateArray()
                .Select(prefix => prefix.GetProperty("prefix").GetString()),
            };
        }

        public async Task<Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>>> GetAsnUpstreamsAsync(int asNumber)
        {
            var jsonData = await _jsonApi.RetrieveAsnUpstreamsAsync(asNumber);
            ValidateStatus(jsonData);
            var dataElement = jsonData.RootElement.GetProperty("data");
            return new Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>>(
                item1: ExtractInfoFromArray(dataElement.GetProperty("ipv4_upstreams")),
                item2: ExtractInfoFromArray(dataElement.GetProperty("ipv6_upstreams"))
            );
        }

        public Task<IpDetailModel> GetIpDetailsAsync(string ipAddress)
        {
            throw new NotImplementedException();
        }

        public Task<PrefixDetailModel> GetPrefixDetailsAsync(string prefix, byte cidr)
        {
            throw new NotImplementedException();
        }

        public Task<SearchModel> SearchByAsync(string queryTerm)
        {
            throw new NotImplementedException();
        }
    }
}
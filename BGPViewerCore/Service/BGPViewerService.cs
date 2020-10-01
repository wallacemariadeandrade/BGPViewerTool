using BGPViewerCore.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace BGPViewerCore.Service
{
    public class BGPViewerService
    {
        private IBGPViewerApi _jsonApi;

        public BGPViewerService(IBGPViewerApi api)
        {
            _jsonApi = api;
        }

        public AsnDetailsModel GetAsnDetails(int asNumber)
        {
            var jsonData = _jsonApi.RetrieveAsnDetails(asNumber).RootElement.GetProperty("data");   
            return new AsnDetailsModel 
            {
                Info = new AsnInfo 
                {
                    ASN = asNumber,
                    Name = jsonData.GetProperty("name").GetString(),
                    Description = jsonData.GetProperty("description_short").GetString(),
                    CountryCode = jsonData.GetProperty("rir_allocation").GetProperty("country_code").GetString()
                },
                EmailContacts = jsonData
                    .GetProperty("email_contacts")
                    .EnumerateArray()
                    .Select(email => email.GetString()),
                AbuseContacts = jsonData
                    .GetProperty("abuse_contacts")
                    .EnumerateArray()
                    .Select(email => email.GetString()),
                LookingGlassUrl = jsonData.GetProperty("looking_glass").GetString(),
            };
        }

        public AsnPrefixesModel GetAsnPrefixes(int asNumber)
        {
            var jsonData = _jsonApi.RetrieveAsnPrefixes(asNumber).RootElement.GetProperty("data");
            return new AsnPrefixesModel
            {
                ASN = asNumber,
                IPv4 = jsonData
                .GetProperty("ipv4_prefixes")
                .EnumerateArray()
                .Select(prefix => prefix.GetProperty("prefix").GetString()),
                IPv6 = jsonData
                .GetProperty("ipv6_prefixes")
                .EnumerateArray()
                .Select(prefix => prefix.GetProperty("prefix").GetString()),
            };
        } 

        public AsnPeersModel GetAsnPeers(int asNumber)
        {
            var jsonData = _jsonApi.RetrieveAsnPeers(asNumber).RootElement.GetProperty("data");
            return new AsnPeersModel 
            {
                ASN = asNumber,
                IPv4 = ExtractInfoFromArray(jsonData.GetProperty("ipv4_peers")),
                IPv6 = ExtractInfoFromArray(jsonData.GetProperty("ipv6_peers")),
            };
        }

        public AsnUpstreamsModel GetAsnUpstreams(int asNumber)
        {
            var jsonData = _jsonApi.RetrieveAsnUpstreams(asNumber).RootElement.GetProperty("data");
            return new AsnUpstreamsModel 
            {
                ASN = asNumber,
                IPv4 = ExtractInfoFromArray(jsonData.GetProperty("ipv4_upstreams")),
                IPv6 = ExtractInfoFromArray(jsonData.GetProperty("ipv6_upstreams"))
            };
        }

        public AsnDownstreamsModel GetAsnDownstreams(int asNumber)
        {
            var jsonData = _jsonApi.RetrieveAsnDownstreams(asNumber).RootElement.GetProperty("data");
            return new AsnDownstreamsModel 
            {
                ASN = asNumber,
                IPv4 = ExtractInfoFromArray(jsonData.GetProperty("ipv4_downstreams")),
                IPv6 = ExtractInfoFromArray(jsonData.GetProperty("ipv6_downstreams"))
            };
        }

        private IEnumerable<AsnInfo> ExtractInfoFromArray(JsonElement jsonArrayElement)
            => jsonArrayElement.EnumerateArray()
                .Select(peer => new AsnInfo {
                    ASN = peer.GetProperty("asn").GetInt32(),
                    Name = peer.GetProperty("name").GetString(),
                    Description = peer.GetProperty("description").GetString(),
                    CountryCode = peer.GetProperty("country_code").GetString()
                }
            );
    }
}
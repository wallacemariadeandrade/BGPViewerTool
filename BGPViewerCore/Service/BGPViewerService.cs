using BGPViewerCore.Model;
using System;
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
                ASN = asNumber,
                Name = jsonData.GetProperty("name").GetString(),
                Description = jsonData.GetProperty("description_short").GetString(),
                CountryCode = jsonData.GetProperty("rir_allocation").GetProperty("country_code").GetString(),
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

        public Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>> GetAsnPeers(int asNumber)
        {
            var jsonData = _jsonApi.RetrieveAsnPeers(asNumber).RootElement.GetProperty("data");
            return new Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>>(
                item1: ExtractInfoFromArray(jsonData.GetProperty("ipv4_peers")),
                item2: ExtractInfoFromArray(jsonData.GetProperty("ipv6_peers"))
            );
        }

        public Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>> GetAsnUpstreams(int asNumber)
        {
            var jsonData = _jsonApi.RetrieveAsnUpstreams(asNumber).RootElement.GetProperty("data");
            return new Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>>(
                item1: ExtractInfoFromArray(jsonData.GetProperty("ipv4_upstreams")),
                item2: ExtractInfoFromArray(jsonData.GetProperty("ipv6_upstreams"))
            );
        }

        public Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>> GetAsnDownstreams(int asNumber)
        {
            var jsonData = _jsonApi.RetrieveAsnDownstreams(asNumber).RootElement.GetProperty("data");
            return new Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>>(
                item1: ExtractInfoFromArray(jsonData.GetProperty("ipv4_downstreams")),
                item2: ExtractInfoFromArray(jsonData.GetProperty("ipv6_downstreams"))
            );
        }

        public IEnumerable<IxModel> GetAsnIxs(int asNumber)
        {
            var jsonData = _jsonApi.RetrieveAsnIxs(asNumber).RootElement.GetProperty("data");
            foreach(var ixJson in jsonData.EnumerateArray())
                yield return new IxModel {
                    Name = ixJson.GetProperty("name").GetString(),
                    FullName = ixJson.GetProperty("name_full").GetString(),
                    CountryCode = ixJson.GetProperty("country_code").GetString(),
                    IPv4 = ixJson.GetProperty("ipv4_address").GetString(),
                    IPv6 = ixJson.GetProperty("ipv6_address").GetString(),
                    AsnSpeed = ixJson.GetProperty("speed").GetInt32()
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
    }
}
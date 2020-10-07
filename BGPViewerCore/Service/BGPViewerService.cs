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
    }
}
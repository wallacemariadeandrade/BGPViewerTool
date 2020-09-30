using BGPViewerCore.Model;
using System.Linq;

namespace BGPViewerCore.Service
{
    public class BGPViewerService
    {
        private IBGPViewerApi _api;

        public BGPViewerService(IBGPViewerApi api)
        {
            _api = api;
        }

        public AsnDetailsModel GetAsnDetails(int asNumber)
        {
            var jsonData = _api.RetrieveAsnDetails(asNumber).RootElement.GetProperty("data");   
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
            var jsonData = _api.RetrieveAsnPrefixes(asNumber).RootElement.GetProperty("data");
            return new AsnPrefixesModel
            {
                ASN = asNumber,
                IPv4Prefixes = jsonData
                .GetProperty("ipv4_prefixes")
                .EnumerateArray()
                .Select(prefix => prefix.GetProperty("prefix").GetString()),
                IPv6Prefixes = jsonData
                .GetProperty("ipv6_prefixes")
                .EnumerateArray()
                .Select(prefix => prefix.GetProperty("prefix").GetString()),
            };
        } 

        public AsnPeersModel GetAsnPeers(int asNumber)
        {
            var jsonData = _api.RetrieveAsnPeers(asNumber).RootElement.GetProperty("data");
            return new AsnPeersModel 
            {
                ASN = asNumber,
                IPv4Peers = jsonData
                .GetProperty("ipv4_peers")
                .EnumerateArray()
                .Select(peer => new AsnInfo {
                    ASN = peer.GetProperty("asn").GetInt32(),
                    Name = peer.GetProperty("name").GetString(),
                    Description = peer.GetProperty("description").GetString(),
                    CountryCode = peer.GetProperty("country_code").GetString()
                }),
                IPv6Peers = jsonData
                .GetProperty("ipv6_peers")
                .EnumerateArray()
                .Select(peer => new AsnInfo {
                    ASN = peer.GetProperty("asn").GetInt32(),
                    Name = peer.GetProperty("name").GetString(),
                    Description = peer.GetProperty("description").GetString(),
                    CountryCode = peer.GetProperty("country_code").GetString()
                })
            };
        }
    }
}
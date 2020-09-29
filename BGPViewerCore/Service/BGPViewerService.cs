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
                ASN = asNumber,
                Name = jsonData.GetProperty("name").GetString(),
                DescriptionShort = jsonData.GetProperty("description_short").GetString(),
                EmailContacts = jsonData
                    .GetProperty("email_contacts")
                    .EnumerateArray()
                    .Select(email => email.GetString()),
                AbuseContacts = jsonData
                    .GetProperty("abuse_contacts")
                    .EnumerateArray()
                    .Select(email => email.GetString()),
                LookingGlassUrl = jsonData.GetProperty("looking_glass").GetString(),
                CountryCode = jsonData.GetProperty("rir_allocation").GetProperty("country_code").GetString()
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
    }
}
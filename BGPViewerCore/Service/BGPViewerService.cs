using BGPViewerCore.Model;
using System.Linq;
using System.Text.Json;

namespace BGPViewerCore.Service
{
    public class BGPViewerService
    {
        public AsnDetailsModel GetAsnDetails(int asNumber)
        {
            var jsonData = JsonDocument.Parse(WebService.GetContentFrom($"https://api.bgpview.io/asn/{asNumber}"))
                .RootElement
                .GetProperty("data");
            
            return new AsnDetailsModel 
            {
                ASN = asNumber,
                Name = jsonData.GetProperty("name").GetString(),
                DescriptionShort = jsonData.GetProperty("description_short").GetString(),
                EmailContacts = jsonData
                    .GetProperty("email_contacts")
                    .EnumerateArray()
                    .Select(x => x.GetString()),
                AbuseContacts = jsonData
                    .GetProperty("abuse_contacts")
                    .EnumerateArray()
                    .Select(x => x.GetString()),
                LookingGlassUrl = jsonData.GetProperty("looking_glass").GetString(),
                CountryCode = jsonData.GetProperty("rir_allocation").GetProperty("country_code").GetString()
            };
        } 
    }
}
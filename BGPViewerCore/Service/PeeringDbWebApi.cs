using System.Text.Json;
using System.Threading.Tasks;

namespace BGPViewerCore.Service
{
    public class PeeringDbWebApi
    {
        private const string BASE_ENDPOINT_URL = "https://www.peeringdb.com/api";
        private string BuildAsnDetailsEndpoint(int asNumber) => $"{BASE_ENDPOINT_URL}/net?asn__lte={asNumber}&asn__gte={asNumber}";
        private string BuildAsnIxsEndpoint(int asNumber) => $"{BASE_ENDPOINT_URL}/netixlan?asn__lte={asNumber}&asn__gte={asNumber}";

        public virtual JsonDocument RetrieveAsnDetails(int asNumber)
            => JsonDocument.Parse(WebService.GetContentFrom(BuildAsnDetailsEndpoint(asNumber)));
     
        public virtual async Task<JsonDocument> RetrieveAsnDetailsAsync(int asNumber)
        {
            using(var stream = await WebService.GetContentStreamFromAsync(BuildAsnDetailsEndpoint(asNumber)))
            {
                return await JsonDocument.ParseAsync(stream);
            }
        }

        public virtual JsonDocument RetrieveAsnIxs(int asNumber)
            => JsonDocument.Parse(WebService.GetContentFrom(BuildAsnIxsEndpoint(asNumber)));
     
        public virtual async Task<JsonDocument> RetrieveAsnIxsAsync(int asNumber)
        {
            using (var stream = await WebService.GetContentStreamFromAsync(BuildAsnIxsEndpoint(asNumber)))
            {
                return await JsonDocument.ParseAsync(stream);
            }
        }
    }
}
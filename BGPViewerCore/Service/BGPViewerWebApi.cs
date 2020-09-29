using System.Text.Json;

namespace BGPViewerCore.Service
{
    public class BGPViewerWebApi : IBGPViewerApi
    {
        public JsonDocument RetrieveAsnDetails(int asNumber)
            => JsonDocument.Parse(CallApi($"https://api.bgpview.io/asn/{asNumber}"));

        public JsonDocument RetrieveAsnPrefixes(int asNumber)
            => JsonDocument.Parse(CallApi($"https://api.bgpview.io/asn/{asNumber}/prefixes"));

        private string CallApi(string url) => WebService.GetContentFrom(url);
    }
}
using System.Text.Json;

namespace BGPViewerCore.Service
{
    public class BGPViewerWebApi : IBGPViewerApi
    {
        public JsonDocument RetrieveAsnDetails(int asNumber)
            => JsonDocument.Parse(CallApi($"https://api.bgpview.io/asn/{asNumber}"));

        public JsonDocument RetrieveAsnDownstreams(int asNumber)
        {
            throw new System.NotImplementedException();
        }

        public JsonDocument RetrieveAsnPeers(int asNumber)
            => JsonDocument.Parse(CallApi($"https://api.bgpview.io/asn/{asNumber}/peers"));

        public JsonDocument RetrieveAsnPrefixes(int asNumber)
            => JsonDocument.Parse(CallApi($"https://api.bgpview.io/asn/{asNumber}/prefixes"));

        public JsonDocument RetrieveAsnUpstreams(int asNumber)
            => JsonDocument.Parse(CallApi($"https://api.bgpview.io/asn/{asNumber}/upstreams"));

        private string CallApi(string url) => WebService.GetContentFrom(url);
    }
}
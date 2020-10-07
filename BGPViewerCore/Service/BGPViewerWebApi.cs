using System.Text.Json;

namespace BGPViewerCore.Service
{
    public class BGPViewerWebApi : IBGPViewerApi
    {
        public JsonDocument RetrieveAsnDetails(int asNumber)
            => JsonDocument.Parse(CallApi($"https://api.bgpview.io/asn/{asNumber}"));

        public JsonDocument RetrieveAsnDownstreams(int asNumber)
            => JsonDocument.Parse(CallApi($"https://api.bgpview.io/asn/{asNumber}/downstreams"));

        public JsonDocument RetrieveAsnIxs(int asNumber)
            => JsonDocument.Parse(CallApi($"https://api.bgpview.io/asn/{asNumber}/ixs"));

        public JsonDocument RetrieveAsnPeers(int asNumber)
            => JsonDocument.Parse(CallApi($"https://api.bgpview.io/asn/{asNumber}/peers"));

        public JsonDocument RetrieveAsnPrefixes(int asNumber)
            => JsonDocument.Parse(CallApi($"https://api.bgpview.io/asn/{asNumber}/prefixes"));

        public JsonDocument RetrieveAsnUpstreams(int asNumber)
            => JsonDocument.Parse(CallApi($"https://api.bgpview.io/asn/{asNumber}/upstreams"));

        public JsonDocument RetrieveIpDetails(string ipAddress)
            => JsonDocument.Parse(CallApi($"https://api.bgpview.io/ip/{ipAddress}"));

        public JsonDocument RetrievePrefixDetails(string prefix, byte cidr)
            => JsonDocument.Parse(CallApi($"https://api.bgpview.io/prefix/{prefix}/{cidr}"));

        private string CallApi(string url) => WebService.GetContentFrom(url);
    }
}
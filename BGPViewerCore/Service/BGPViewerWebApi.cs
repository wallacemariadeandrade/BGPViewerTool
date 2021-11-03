using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace BGPViewerCore.Service
{
    public class BGPViewerWebApi : IBGPViewerApi
    {
        private const string BASE_ASN_URL = "https://api.bgpview.io/asn/";
        public JsonDocument RetrieveAsnDetails(int asNumber)
            => JsonDocument.Parse(CallApi($"{BASE_ASN_URL}{asNumber}"));

        public async Task<JsonDocument> RetrieveAsnDetailsAsync(int asNumber)
            => await JsonDocument.ParseAsync(await WebService.GetContentStreamFromAsync($"{BASE_ASN_URL}{asNumber}"));

        public JsonDocument RetrieveAsnDownstreams(int asNumber)
            => JsonDocument.Parse(CallApi($"https://api.bgpview.io/asn/{asNumber}/downstreams"));

        public Task<JsonDocument> RetrieveAsnDownstreamsAsync(int asNumber)
        {
            throw new System.NotImplementedException();
        }

        public JsonDocument RetrieveAsnIxs(int asNumber)
            => JsonDocument.Parse(CallApi($"https://api.bgpview.io/asn/{asNumber}/ixs"));

        public Task<JsonDocument> RetrieveAsnIxsAsync(int asNumber)
        {
            throw new System.NotImplementedException();
        }

        public JsonDocument RetrieveAsnPeers(int asNumber)
            => JsonDocument.Parse(CallApi($"https://api.bgpview.io/asn/{asNumber}/peers"));

        public Task<JsonDocument> RetrieveAsnPeersAsync(int asNumber)
        {
            throw new System.NotImplementedException();
        }

        public JsonDocument RetrieveAsnPrefixes(int asNumber)
            => JsonDocument.Parse(CallApi($"https://api.bgpview.io/asn/{asNumber}/prefixes"));

        public Task<JsonDocument> RetrieveAsnPrefixesAsync(int asNumber)
        {
            throw new System.NotImplementedException();
        }

        public JsonDocument RetrieveAsnUpstreams(int asNumber)
            => JsonDocument.Parse(CallApi($"https://api.bgpview.io/asn/{asNumber}/upstreams"));

        public Task<JsonDocument> RetrieveAsnUpstreamsAsync(int asNumber)
        {
            throw new System.NotImplementedException();
        }

        public JsonDocument RetrieveIpDetails(string ipAddress)
            => JsonDocument.Parse(CallApi($"https://api.bgpview.io/ip/{ipAddress}"));

        public Task<JsonDocument> RetrieveIpDetailsAsync(string ipAddress)
        {
            throw new System.NotImplementedException();
        }

        public JsonDocument RetrievePrefixDetails(string prefix, byte cidr)
            => JsonDocument.Parse(CallApi($"https://api.bgpview.io/prefix/{prefix}/{cidr}"));

        public Task<JsonDocument> RetrievePrefixDetailsAsync(string prefix, byte cidr)
        {
            throw new System.NotImplementedException();
        }

        public JsonDocument RetrieveSearchBy(string queryTerm)
            => JsonDocument.Parse(CallApi($"https://api.bgpview.io/search?query_term={queryTerm}"));

        public Task<JsonDocument> RetrieveSearchByAsync(string queryTerm)
        {
            throw new System.NotImplementedException();
        }

        private string CallApi(string url) => WebService.GetContentFrom(url);
        private Task<Stream> CallApiAsync(string url) => WebService.GetContentStreamFromAsync(url);
    }
}
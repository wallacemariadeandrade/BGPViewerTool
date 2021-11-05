using System.Text.Json;
using System.Threading.Tasks;

namespace BGPViewerCore.Service
{
    public class BGPViewerWebApi : IBGPViewerApi
    {
        #region ENDPOINTS BUILDERS
        private const string BASE_ENDPOINT_URL = "https://api.bgpview.io";
        private string BuildAsnDetailsEndpoint(int asNumber) => $"{BASE_ENDPOINT_URL}/asn/{asNumber}";
        private string BuildAsnDownstreamsEndpoint(int asNumber) => $"{BuildAsnDetailsEndpoint(asNumber)}/downstreams";
        private string BuildAsnIxsEndpoint(int asNumber) => $"{BuildAsnDetailsEndpoint(asNumber)}/ixs";
        private string BuildAsnPeersEndpoint(int asNumber) => $"{BuildAsnDetailsEndpoint(asNumber)}/peers";
        private string BuildAsnPrefixesEndpoint(int asNumber) => $"{BuildAsnDetailsEndpoint(asNumber)}/prefixes";
        private string BuildAsnUpstreamsEndpoint(int asNumber) => $"{BuildAsnDetailsEndpoint(asNumber)}/upstreams";
        private string BuildIpDetailsEndpoint(string ipAddress) => $"{BASE_ENDPOINT_URL}/ip/{ipAddress}";
        private string BuildPrefixDetailsEndpoint(string prefix, byte cidr) => $"{BASE_ENDPOINT_URL}/prefix/{prefix}/{cidr}";
        private string BuildSearchByEndpoint(string queryTerm) => $"{BASE_ENDPOINT_URL}/search?query_term={queryTerm}";
        #endregion

        #region INTERNAL OPERATIONS
        private JsonDocument ParseOperation(string url) => JsonDocument.Parse(WebService.GetContentFrom(url));
        private async Task<JsonDocument> ParseOperationAsync(string url) 
            => await JsonDocument.ParseAsync(await WebService.GetContentStreamFromAsync(url));
        #endregion

        #region SYNC APIs
        public JsonDocument RetrieveAsnDetails(int asNumber) => ParseOperation(BuildAsnDetailsEndpoint(asNumber));
        public JsonDocument RetrieveAsnDownstreams(int asNumber) => ParseOperation(BuildAsnDownstreamsEndpoint(asNumber));
        public JsonDocument RetrieveAsnIxs(int asNumber) => ParseOperation(BuildAsnIxsEndpoint(asNumber));
        public JsonDocument RetrieveAsnPeers(int asNumber) => ParseOperation(BuildAsnPeersEndpoint(asNumber));
        public JsonDocument RetrieveAsnPrefixes(int asNumber) => ParseOperation(BuildAsnPrefixesEndpoint(asNumber));
        public JsonDocument RetrieveAsnUpstreams(int asNumber) => ParseOperation(BuildAsnUpstreamsEndpoint(asNumber));
        public JsonDocument RetrieveIpDetails(string ipAddress) => ParseOperation(BuildIpDetailsEndpoint(ipAddress));
        public JsonDocument RetrievePrefixDetails(string prefix, byte cidr) => ParseOperation(BuildPrefixDetailsEndpoint(prefix, cidr));
        public JsonDocument RetrieveSearchBy(string queryTerm) => ParseOperation(BuildSearchByEndpoint(queryTerm));
        #endregion

        #region ASYNC APIs
        public async Task<JsonDocument> RetrieveAsnDetailsAsync(int asNumber) => await ParseOperationAsync(BuildAsnDetailsEndpoint(asNumber));
        public async Task<JsonDocument> RetrieveAsnDownstreamsAsync(int asNumber) => await ParseOperationAsync(BuildAsnDownstreamsEndpoint(asNumber));
        public async Task<JsonDocument> RetrieveAsnIxsAsync(int asNumber) => await ParseOperationAsync(BuildAsnIxsEndpoint(asNumber));
        public async Task<JsonDocument> RetrieveAsnPeersAsync(int asNumber) => await ParseOperationAsync(BuildAsnPeersEndpoint(asNumber));
        public async Task<JsonDocument> RetrieveAsnPrefixesAsync(int asNumber) => await ParseOperationAsync(BuildAsnPrefixesEndpoint(asNumber));
        public async Task<JsonDocument> RetrieveAsnUpstreamsAsync(int asNumber) => await ParseOperationAsync(BuildAsnUpstreamsEndpoint(asNumber));        
        public async Task<JsonDocument> RetrieveIpDetailsAsync(string ipAddress) => await ParseOperationAsync(BuildIpDetailsEndpoint(ipAddress));
        public async Task<JsonDocument> RetrievePrefixDetailsAsync(string prefix, byte cidr) => await ParseOperationAsync(BuildPrefixDetailsEndpoint(prefix, cidr));
        public async Task<JsonDocument> RetrieveSearchByAsync(string queryTerm) => await ParseOperationAsync(BuildSearchByEndpoint(queryTerm));
        #endregion
    }
}
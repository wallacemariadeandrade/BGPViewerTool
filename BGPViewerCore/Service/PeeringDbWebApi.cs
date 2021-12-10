using System.Text.Json;
using System.Threading.Tasks;

namespace BGPViewerCore.Service
{
    public class PeeringDbWebApi : IBGPViewerApi
    {
        #region ENDPOINTS BUILDERS
        private const string BASE_ENDPOINT_URL = "https://www.peeringdb.com/api";
        private string BuildAsnDetailsEndpoint(int asNumber) => $"{BASE_ENDPOINT_URL}/net?asn__lte={asNumber}&asn__gte={asNumber}";
        private string BuildAsnIxsEndpoint(int asNumber) => $"{BASE_ENDPOINT_URL}/netixlan?asn__lte={asNumber}&asn__gte={asNumber}";
        private JsonDocument Empty() => JsonDocument.Parse(@"{""data"": [], ""meta"": {}}");
        #endregion

        #region INTERNAL OPERATIONS
        private JsonDocument ParseOperation(string url) => JsonDocument.Parse(WebService.GetContentFrom(url));
        private async Task<JsonDocument> ParseOperationAsync(string url) 
        {
            using (var stream = await WebService.GetContentStreamFromAsync(url))
            {
                return await JsonDocument.ParseAsync(stream);
            }
        }
        #endregion

        #region SYNC APIs
        public JsonDocument RetrieveAsnDetails(int asNumber) => ParseOperation(BuildAsnDetailsEndpoint(asNumber));
        public JsonDocument RetrieveAsnDownstreams(int asNumber) => Empty();
        public JsonDocument RetrieveAsnIxs(int asNumber) => ParseOperation(BuildAsnIxsEndpoint(asNumber));
        public JsonDocument RetrieveAsnPeers(int asNumber) => Empty();
        public JsonDocument RetrieveAsnPrefixes(int asNumber) => Empty();
        public JsonDocument RetrieveAsnUpstreams(int asNumber) => Empty();
        public JsonDocument RetrieveIpDetails(string ipAddress) => Empty();
        public JsonDocument RetrievePrefixDetails(string prefix, byte cidr) => Empty();
        public JsonDocument RetrieveSearchBy(string queryTerm) => Empty();
        #endregion

        #region ASYNC APIs
        public async Task<JsonDocument> RetrieveAsnDetailsAsync(int asNumber) => await ParseOperationAsync(BuildAsnDetailsEndpoint(asNumber));
        public Task<JsonDocument> RetrieveAsnDownstreamsAsync(int asNumber) => Task.FromResult(Empty());
        public async Task<JsonDocument> RetrieveAsnIxsAsync(int asNumber) => await ParseOperationAsync(BuildAsnIxsEndpoint(asNumber));
        public Task<JsonDocument> RetrieveAsnPeersAsync(int asNumber) => Task.FromResult(Empty());
        public Task<JsonDocument> RetrieveAsnPrefixesAsync(int asNumber) => Task.FromResult(Empty());
        public Task<JsonDocument> RetrieveAsnUpstreamsAsync(int asNumber) => Task.FromResult(Empty());
        public Task<JsonDocument> RetrieveIpDetailsAsync(string ipAddress) => Task.FromResult(Empty());
        public Task<JsonDocument> RetrievePrefixDetailsAsync(string prefix, byte cidr) => Task.FromResult(Empty());
        public Task<JsonDocument> RetrieveSearchByAsync(string queryTerm) => Task.FromResult(Empty());
        #endregion
    }
}
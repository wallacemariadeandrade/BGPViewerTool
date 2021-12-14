using System.Text.Json;
using System.Threading.Tasks;

namespace BGPViewerCore.Service
{
    public class RipeStatApi
    {
        private const string BASE_ENDPOINT = "https://stat.ripe.net/data";
        private string BuildAsnDetailsEndpoint(int asNumber) => $"{BASE_ENDPOINT}/as-overview/data.json?resource={asNumber}";
        private string BuildAsnPrefixesEndpoint(int asNumber) => $"{BASE_ENDPOINT}/announced-prefixes/data.json?resource={asNumber}";
        private string BuildAsnNeighboursEndpoint(int asNumber) => $"{BASE_ENDPOINT}/asn-neighbours/data.json?resource={asNumber}";
        private string BuildIpDetailsEndpoint(string ipAddress) => $"{BASE_ENDPOINT}/network-info/data.json?resource={ipAddress}";
        private string BuildPrefixDetailsEndpoint(string prefix, byte cidr) => $"{BASE_ENDPOINT}/prefix-overview/data.json?resource={prefix}/{cidr}";

        private JsonDocument ParseOperation(string url) => JsonDocument.Parse(WebService.GetContentFrom(url));
        private async Task<JsonDocument> ParseOperationAsync(string url) 
        {
            using (var stream = await WebService.GetContentStreamFromAsync(url))
            {
                return await JsonDocument.ParseAsync(stream);
            }
        }

        public virtual JsonDocument RetrieveAsnDetails(int asNumber)
            => ParseOperation(BuildAsnDetailsEndpoint(asNumber));
     
        public virtual Task<JsonDocument> RetrieveAsnDetailsAsync(int asNumber)
            => ParseOperationAsync(BuildAsnDetailsEndpoint(asNumber));

        public virtual JsonDocument RetrieveAsnPrefixes(int asNumber)
            => ParseOperation(BuildAsnPrefixesEndpoint(asNumber));

        public virtual Task<JsonDocument> RetrieveAsnPrefixesAsync(int asNumber)
            => ParseOperationAsync(BuildAsnPrefixesEndpoint(asNumber));

        public virtual JsonDocument RetrieveAsnNeighbours(int asNumber)
            => ParseOperation(BuildAsnNeighboursEndpoint(asNumber));

        public virtual Task<JsonDocument> RetrieveAsnNeighboursAsync(int asNumber)
            => ParseOperationAsync(BuildAsnNeighboursEndpoint(asNumber));

        public virtual JsonDocument RetrieveIpDetails(string ipAddress)
            => ParseOperation(BuildIpDetailsEndpoint(ipAddress));

        public virtual Task<JsonDocument> RetrieveIpDetailsAsync(string ipAddress)
            => ParseOperationAsync(BuildIpDetailsEndpoint(ipAddress));

        public virtual JsonDocument RetrievePrefixDetails(string prefix, byte cidr)
            => ParseOperation(BuildPrefixDetailsEndpoint(prefix, cidr));

        public virtual Task<JsonDocument> RetrievePrefixDetailsAsync(string prefix, byte cidr)
            => ParseOperationAsync(BuildPrefixDetailsEndpoint(prefix, cidr));
    }
}
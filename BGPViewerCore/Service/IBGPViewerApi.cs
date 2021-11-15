using System.Text.Json;
using System.Threading.Tasks;

namespace BGPViewerCore.Service
{   
    // Defines interactions with api
    public interface IBGPViewerApi
    {
        JsonDocument RetrieveAsnDetails(int asNumber);
        Task<JsonDocument> RetrieveAsnDetailsAsync(int asNumber);
        JsonDocument RetrieveAsnPrefixes(int asNumber);
        Task<JsonDocument> RetrieveAsnPrefixesAsync(int asNumber);
        JsonDocument RetrieveAsnPeers(int asNumber);
        Task<JsonDocument> RetrieveAsnPeersAsync(int asNumber);
        JsonDocument RetrieveAsnUpstreams(int asNumber);
        Task<JsonDocument> RetrieveAsnUpstreamsAsync(int asNumber);
        JsonDocument RetrieveAsnDownstreams(int asNumber);
        Task<JsonDocument> RetrieveAsnDownstreamsAsync(int asNumber);
        JsonDocument RetrieveAsnIxs(int asNumber);
        Task<JsonDocument> RetrieveAsnIxsAsync(int asNumber);
        JsonDocument RetrieveIpDetails(string ipAddress);
        Task<JsonDocument> RetrieveIpDetailsAsync(string ipAddress);
        JsonDocument RetrievePrefixDetails(string prefix, byte cidr);
        Task<JsonDocument> RetrievePrefixDetailsAsync(string prefix, byte cidr);
        JsonDocument RetrieveSearchBy(string queryTerm);
        Task<JsonDocument> RetrieveSearchByAsync(string queryTerm);
    }
}
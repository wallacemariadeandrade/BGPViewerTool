using System.Text.Json;
using System.Threading.Tasks;

namespace BGPViewerCore.Service
{
    public class PeeringDbWebApi : IBGPViewerApi
    {
        public JsonDocument RetrieveAsnDetails(int asNumber)
        {
            throw new System.NotImplementedException();
        }

        public Task<JsonDocument> RetrieveAsnDetailsAsync(int asNumber)
        {
            throw new System.NotImplementedException();
        }

        public JsonDocument RetrieveAsnDownstreams(int asNumber)
        {
            throw new System.NotImplementedException();
        }

        public Task<JsonDocument> RetrieveAsnDownstreamsAsync(int asNumber)
        {
            throw new System.NotImplementedException();
        }

        public JsonDocument RetrieveAsnIxs(int asNumber)
        {
            throw new System.NotImplementedException();
        }

        public Task<JsonDocument> RetrieveAsnIxsAsync(int asNumber)
        {
            throw new System.NotImplementedException();
        }

        public JsonDocument RetrieveAsnPeers(int asNumber)
        {
            throw new System.NotImplementedException();
        }

        public Task<JsonDocument> RetrieveAsnPeersAsync(int asNumber)
        {
            throw new System.NotImplementedException();
        }

        public JsonDocument RetrieveAsnPrefixes(int asNumber)
        {
            throw new System.NotImplementedException();
        }

        public Task<JsonDocument> RetrieveAsnPrefixesAsync(int asNumber)
        {
            throw new System.NotImplementedException();
        }

        public JsonDocument RetrieveAsnUpstreams(int asNumber)
        {
            throw new System.NotImplementedException();
        }

        public Task<JsonDocument> RetrieveAsnUpstreamsAsync(int asNumber)
        {
            throw new System.NotImplementedException();
        }

        public JsonDocument RetrieveIpDetails(string ipAddress)
        {
            throw new System.NotImplementedException();
        }

        public Task<JsonDocument> RetrieveIpDetailsAsync(string ipAddress)
        {
            throw new System.NotImplementedException();
        }

        public JsonDocument RetrievePrefixDetails(string prefix, byte cidr)
        {
            throw new System.NotImplementedException();
        }

        public Task<JsonDocument> RetrievePrefixDetailsAsync(string prefix, byte cidr)
        {
            throw new System.NotImplementedException();
        }

        public JsonDocument RetrieveSearchBy(string queryTerm)
        {
            throw new System.NotImplementedException();
        }

        public Task<JsonDocument> RetrieveSearchByAsync(string queryTerm)
        {
            throw new System.NotImplementedException();
        }
    }
}
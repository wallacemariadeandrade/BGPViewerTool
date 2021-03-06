using System.Text.Json;

namespace BGPViewerCore.Service
{   
    // Defines interactions with api
    public interface IBGPViewerApi
    {
        JsonDocument RetrieveAsnDetails(int asNumber);
        JsonDocument RetrieveAsnPrefixes(int asNumber);
        JsonDocument RetrieveAsnPeers(int asNumber);
        JsonDocument RetrieveAsnUpstreams(int asNumber);
        JsonDocument RetrieveAsnDownstreams(int asNumber);
        JsonDocument RetrieveAsnIxs(int asNumber);
        JsonDocument RetrieveIpDetails(string ipAddress);
        JsonDocument RetrievePrefixDetails(string prefix, byte cidr);
        JsonDocument RetrieveSearchBy(string queryTerm);
    }
}
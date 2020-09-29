using System.Text.Json;

namespace BGPViewerCore.Service
{   
    // Defines interactions with api
    public interface IBGPViewerApi
    {
        JsonDocument RetrieveAsnDetails(int asNumber);
        JsonDocument RetrieveAsnPrefixes(int asNumber);
    }
}
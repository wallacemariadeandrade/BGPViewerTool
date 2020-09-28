using System.Text.Json;

namespace BGPViewerCore.Service
{
    public class BGPViewerWebApi : IBGPViewerApi
    {
        public JsonDocument RetrieveAsnDetails(int asNumber)
            => JsonDocument
                .Parse(WebService.GetContentFrom($"https://api.bgpview.io/asn/{asNumber}"));
    }
}
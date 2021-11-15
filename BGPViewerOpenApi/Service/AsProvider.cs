using System.Collections.Generic;
using System.Threading.Tasks;
using BGPViewerCore.Models;
using BGPViewerOpenApi.Model;

namespace BGPViewerOpenApi.Service
{
    public class AsProvider
    {
        private readonly ApiProvider apiProvider;

        public AsProvider(ApiProvider apiProvider)
        {
            this.apiProvider = apiProvider;
        }

        internal Task<AsnDetailsModel> GetDetailsAsync(int apiId, int asNumber)
            => apiProvider.GetApiById(apiId).GetAsnDetailsAsync(asNumber);

        internal async Task<Peers> GetPeersAsync(int apiId, int asNumber)
        {
            var api = apiProvider.GetApiById(apiId);

            var peersTuple = await api.GetAsnPeersAsync(asNumber);

            return new Peers {
                IPv4 = peersTuple.Item1,
                IPv6 = peersTuple.Item2
            };
        }

        internal async Task<Peers> GetUpstreamsAsync(int apiId, int asNumber)
        {
            var api = apiProvider.GetApiById(apiId);

            var upstreamsTuple = await api.GetAsnUpstreamsAsync(asNumber);

            return new Peers {
                IPv4 = upstreamsTuple.Item1,
                IPv6 = upstreamsTuple.Item2
            };
        }

        internal async Task<Peers> GetDownstreamsAsync(int apiId, int asNumber)
        {
            var api = apiProvider.GetApiById(apiId);

            var downstreamsTuple = await api.GetAsnDownstreamsAsync(asNumber);

            return new Peers {
                IPv4 = downstreamsTuple.Item1,
                IPv6 = downstreamsTuple.Item2,
            };
        }

        internal Task<IEnumerable<IxModel>> GetIxsAsync(int apiId, int asNumber)
            => apiProvider.GetApiById(apiId).GetAsnIxsAsync(asNumber);

        internal Task<AsnPrefixesModel> GetPrefixesAsync(int apiId, int asNumber)
            => apiProvider.GetApiById(apiId).GetAsnPrefixesAsync(asNumber);
    }
}
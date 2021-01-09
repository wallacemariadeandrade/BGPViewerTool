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

        internal async Task<AsnDetailsModel> GetDetailsAsync(int apiId, int asNumber)
        {
            var api = apiProvider.GetApiById(apiId);

            return await Task.FromResult(api.GetAsnDetails(asNumber));
        }

        internal async Task<Peers> GetPeersAsync(int apiId, int asNumber)
        {
            var api = apiProvider.GetApiById(apiId);

            var peersTuple = await Task.FromResult(api.GetAsnPeers(asNumber));

            return new Peers {
                IPv4 = peersTuple.Item1,
                IPv6 = peersTuple.Item2
            };
        }

        internal async Task<Peers> GetUpstreamsAsync(int apiId, int asNumber)
        {
            var api = apiProvider.GetApiById(apiId);

            var upstreamsTuple = await Task.FromResult(api.GetAsnUpstreams(asNumber));

            return new Peers {
                IPv4 = upstreamsTuple.Item1,
                IPv6 = upstreamsTuple.Item2
            };
        }

        internal async Task<Peers> GetDownstreamsAsync(int apiId, int asNumber)
        {
            var api = apiProvider.GetApiById(apiId);

            var downstreamsTuple = await Task.FromResult(api.GetAsnDownstreams(asNumber));

            return new Peers {
                IPv4 = downstreamsTuple.Item1,
                IPv6 = downstreamsTuple.Item2,
            };
        }

        internal async Task<IEnumerable<IxModel>> GetIxsAsync(int apiId, int asNumber)
        {
            var api = apiProvider.GetApiById(apiId);

            return await Task.FromResult(api.GetAsnIxs(asNumber));
        }

        internal async Task<AsnPrefixesModel> GetPrefixesAsync(int apiId, int asNumber)
        {
            var api = apiProvider.GetApiById(apiId);

            return await Task.FromResult(api.GetAsnPrefixes(asNumber));
        }
    }
}
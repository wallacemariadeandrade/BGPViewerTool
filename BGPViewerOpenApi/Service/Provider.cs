using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BGPViewerCore.Service;
using BGPViewerCore.Model;
using BGPViewerOpenApi.Model;

namespace BGPViewerOpenApi.Service
{
    public class Provider
    {
        private readonly IEnumerable<ApiBase> availableApis;
        private readonly IServiceProvider serviceProvider;

        public Provider(IEnumerable<ApiBase> availableApis, IServiceProvider serviceProvider)
        {
            this.availableApis = availableApis;
            this.serviceProvider = serviceProvider;
        }

        internal async Task<IEnumerable<ApiBase>> ListAvailableAsync()
        {
            if(ContainsDuplicatedId()) throw new InvalidOperationException("There are one or more APIs with same ID.");

            return await Task.FromResult(availableApis);
        }

        internal async Task<AsnDetailsModel> GetDetailsAsync(int apiId, int asNumber)
        {
            var api = GetApiById(apiId);

            return await Task.FromResult(api.GetAsnDetails(asNumber));
        }

        internal async Task<Peers> GetPeersAsync(int apiId, int asNumber)
        {
            var api = GetApiById(apiId);

            var peersTuple = await Task.FromResult(api.GetAsnPeers(asNumber));

            return new Peers {
                IPv4 = peersTuple.Item1,
                IPv6 = peersTuple.Item2
            };
        }

        internal async Task<Peers> GetUpstreamsAsync(int apiId, int asNumber)
        {
            var api = GetApiById(apiId);

            var upstreamsTuple = await Task.FromResult(api.GetAsnUpstreams(asNumber));

            return new Peers {
                IPv4 = upstreamsTuple.Item1,
                IPv6 = upstreamsTuple.Item2
            };
        }

        internal async Task<Peers> GetDownstreamsAsync(int apiId, int asNumber)
        {
            var api = GetApiById(apiId);

            var downstreamsTuple = await Task.FromResult(api.GetAsnDownstreams(asNumber));

            return new Peers {
                IPv4 = downstreamsTuple.Item1,
                IPv6 = downstreamsTuple.Item2,
            };
        }

        internal async Task<IEnumerable<IxModel>> GetIxsAsync(int apiId, int asNumber)
        {
            var api = GetApiById(apiId);

            return await Task.FromResult(api.GetAsnIxs(asNumber));
        }

        internal async Task<AsnPrefixesModel> GetPrefixesAsync(int apiId, int asNumber)
        {
            var api = GetApiById(apiId);

            return await Task.FromResult(api.GetAsnPrefixes(asNumber));
        }

        private IBGPViewerService GetApiById(int apiId)
        {
            var selectedApi = availableApis.FirstOrDefault(x => x.Id == apiId);
            
            if(selectedApi == null) throw new KeyNotFoundException($"Don't exist an API with ID {apiId}.");

            return serviceProvider.GetService(selectedApi.ApiType) as IBGPViewerService;
        }

        private bool ContainsDuplicatedId()
        {
            var ids = availableApis.Select(x => x.Id);
            return ids.Count() != ids.Distinct().Count();
        }
    }
}
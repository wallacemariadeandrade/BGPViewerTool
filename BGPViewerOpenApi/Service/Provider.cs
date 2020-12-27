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

        internal Task<IEnumerable<ApiBase>> ListAvailableAsync()
        {
            if(ContainsDuplicatedId()) throw new InvalidOperationException("There are one or more APIs with same ID.");

            return Task.FromResult(availableApis);
        }

        internal Task<AsnDetailsModel> GetDetailsAsync(int apiId, int asNumber)
        {
            var api = GetApiById(apiId);

            return Task.FromResult(api.GetAsnDetails(asNumber));
        }

        internal Task<AsnPrefixesModel> GetPrefixesAsync(int apiId, int asNumber)
        {
            var api = GetApiById(apiId);

            return Task.FromResult(api.GetAsnPrefixes(asNumber));
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
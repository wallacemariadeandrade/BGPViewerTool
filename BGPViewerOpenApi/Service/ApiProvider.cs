using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BGPViewerCore.Service;
using BGPViewerOpenApi.Model;

namespace BGPViewerOpenApi.Service
{
    public class ApiProvider
    {
        private readonly IEnumerable<ApiBase> availableApis;
        private readonly IEnumerable<int> availableIds;
        private readonly IServiceProvider serviceProvider;

        public ApiProvider(IEnumerable<ApiBase> availableApis, IServiceProvider serviceProvider)
        {
            this.availableApis = availableApis;
            this.serviceProvider = serviceProvider;
            availableIds = availableApis.Select(x => x.Id);
        }

        internal async Task<IEnumerable<ApiBase>> ListAvailableAsync()
        {
            if(ContainsDuplicatedId()) throw new InvalidOperationException("There are one or more APIs with same ID.");

            return await Task.FromResult(availableApis);
        }

        internal IBGPViewerService GetApiById(int apiId)
        {
            if(!CheckIfExists(apiId)) throw new KeyNotFoundException($"Do not exist an API with ID {apiId}.");

            var selectedApi = availableApis.FirstOrDefault(x => x.Id == apiId);

            return serviceProvider.GetService(selectedApi.ApiType) as IBGPViewerService;
        }

        public bool CheckIfExists(int apiId) => availableIds.Contains(apiId);
        private bool ContainsDuplicatedId() => availableIds.Count() != availableIds.Distinct().Count();
    }
}
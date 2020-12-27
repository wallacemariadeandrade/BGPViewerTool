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
        private readonly IServiceProvider serviceProvider;

        public ApiProvider(IEnumerable<ApiBase> availableApis, IServiceProvider serviceProvider)
        {
            this.availableApis = availableApis;
            this.serviceProvider = serviceProvider;
        }

        internal async Task<IEnumerable<ApiBase>> ListAvailableAsync()
        {
            if(ContainsDuplicatedId()) throw new InvalidOperationException("There are one or more APIs with same ID.");

            return await Task.FromResult(availableApis);
        }

        internal IBGPViewerService GetApiById(int apiId)
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
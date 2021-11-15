using System.Threading.Tasks;
using BGPViewerCore.Models;

namespace BGPViewerOpenApi.Service
{
    public class SearchProvider
    {
        private readonly ApiProvider apiProvider;

        public SearchProvider(ApiProvider apiProvider)
        {
            this.apiProvider = apiProvider;
        }

        internal Task<SearchModel> Search(string queryTerm, int apiId)
            => apiProvider.GetApiById(apiId).SearchByAsync(queryTerm);
    }
}
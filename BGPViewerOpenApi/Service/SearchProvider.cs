using System.Threading.Tasks;
using BGPViewerCore.Model;

namespace BGPViewerOpenApi.Service
{
    public class SearchProvider
    {
        private readonly ApiProvider apiProvider;

        public SearchProvider(ApiProvider apiProvider)
        {
            this.apiProvider = apiProvider;
        }

        internal async Task<SearchModel> Search(string queryTerm, int apiId)
        {
            var api = apiProvider.GetApiById(apiId);

            return await Task.FromResult(api.SearchBy(queryTerm));
        }
    }
}
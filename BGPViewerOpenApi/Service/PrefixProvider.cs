using System.Threading.Tasks;
using BGPViewerCore.Models;

namespace BGPViewerOpenApi.Service
{
    public class PrefixProvider
    {
        private readonly ApiProvider apiProvider;

        public PrefixProvider(ApiProvider apiProvider)
        {
            this.apiProvider = apiProvider;
        }

        internal async Task<PrefixDetailModel> GetDetailsAsync(int apiId, string prefix, byte cidr)
        {
            var api = apiProvider.GetApiById(apiId);

            return await Task.FromResult(api.GetPrefixDetails(prefix, cidr));
        }
    }
}
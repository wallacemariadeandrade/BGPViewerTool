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

        internal Task<PrefixDetailModel> GetDetailsAsync(int apiId, string prefix, byte cidr)
            => apiProvider.GetApiById(apiId).GetPrefixDetailsAsync(prefix, cidr);
    }
}
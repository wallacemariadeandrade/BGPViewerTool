using System.Threading.Tasks;
using BGPViewerCore.Model;

namespace BGPViewerOpenApi.Service
{
    public class IPAddressProvider
    {
        private readonly ApiProvider apiProvider;
        public IPAddressProvider(ApiProvider apiProvider)
        {
            this.apiProvider = apiProvider;
        }

        internal async Task<IpDetailModel> GetDetailsAsync(string ipAddress, int apiId)
        {
            var api = apiProvider.GetApiById(apiId);

            return await Task.FromResult(api.GetIpDetails(ipAddress));
        }
    }
}
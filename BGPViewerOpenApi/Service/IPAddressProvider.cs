using System.Threading.Tasks;
using BGPViewerCore.Models;

namespace BGPViewerOpenApi.Service
{
    public class IPAddressProvider
    {
        private readonly ApiProvider apiProvider;
        public IPAddressProvider(ApiProvider apiProvider)
        {
            this.apiProvider = apiProvider;
        }

        internal Task<IpDetailModel> GetDetailsAsync(string ipAddress, int apiId)
            => apiProvider.GetApiById(apiId).GetIpDetailsAsync(ipAddress);
    }
}
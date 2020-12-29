using System.Threading.Tasks;
using BGPViewerOpenApi.Service;
using BGPViewerOpenApi.Validators;
using Microsoft.AspNetCore.Mvc;

namespace BGPViewerOpenApi.Controllers
{
    [Route("api/address")]
    [ApiController]
    [ValidateSelectedApiExistence]
    public class IPAddressController : Controller
    {
        private readonly IPAddressProvider provider;

        public IPAddressController(IPAddressProvider provider)
        {
            this.provider = provider;
        }

        [HttpGet("{ipAddress}/details/{apiId}")]
        public async Task<IActionResult> IpDetails([ValidateIPAddress]string ipAddress, int apiId)
            => Ok(await provider.GetDetailsAsync(ipAddress, apiId));
    }
}
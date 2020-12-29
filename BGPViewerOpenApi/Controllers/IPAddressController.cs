using System.Threading.Tasks;
using BGPViewerCore.Model;
using BGPViewerOpenApi.Service;
using BGPViewerOpenApi.Validators;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BGPViewerOpenApi.Controllers
{
    [Produces("application/json")]
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
        [SwaggerOperation(Summary = "Retrieves details from provided IP address.", OperationId = "IPAddressDetails", Tags = new [] {"IP Address"})]
        [SwaggerResponse(200, Type = typeof(IpDetailModel))]
        public async Task<IActionResult> IpDetails([ValidateIPAddress]string ipAddress, int apiId)
            => Ok(await provider.GetDetailsAsync(ipAddress, apiId));
    }
}
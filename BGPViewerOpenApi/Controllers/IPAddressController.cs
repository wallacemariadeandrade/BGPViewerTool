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
        [SwaggerOperation(Summary = "Retrieves details from provided IP address.", Description = "Addresses examples: 8.8.8.8, 1.1.1.1, 684d:1111:222:3333:4444:5555:6:77, fe80::0202:b3ff:fe1e:8329, 2001:db8:0::1 and so on." ,OperationId = "IPAddressDetails", Tags = new [] {"IP Address"})]
        [SwaggerResponse(200, Type = typeof(IpDetailModel))]
        [SwaggerResponse(404, "When there is no API with the given ID or when searched IP address doesn't exist on selected API", Type = typeof(ValidationProblemDetails))]
        [SwaggerResponse(400, "When provided IP address is not correctly formatted." , Type = typeof(ValidationProblemDetails))]
        public async Task<IActionResult> IpDetails([ValidateIPAddress]string ipAddress, int apiId)
            => Ok(await provider.GetDetailsAsync(ipAddress, apiId));
    }
}
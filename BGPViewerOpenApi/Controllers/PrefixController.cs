using System.Threading.Tasks;
using BGPViewerOpenApi.Service;
using Microsoft.AspNetCore.Mvc;
using BGPViewerOpenApi.Validators;
using Swashbuckle.AspNetCore.Annotations;
using BGPViewerCore.Models;

namespace BGPViewerOpenApi.Controllers
{
    [Produces("application/json")]
    [Route("api/{apiId}/prefix")]
    [ApiController]
    [ValidateSelectedApiExistence]
    public class PrefixController : Controller
    {
        private readonly PrefixProvider provider;

        public PrefixController(PrefixProvider provider)
        {
            this.provider = provider;
        }

        [HttpGet("{prefix}/{cidr}/details")]
        [ValidatePrefix]
        [SwaggerOperation(Summary = "Retrieves details from provided prefix.", Description = "Prefixes examples: 2001:db8::, 8.8.8.0, 2001:db8:3c4d:15::, 2002:: and so on. Note that cidr comes separated in other field.", OperationId = "PrefixDetails", Tags = new [] {"Prefix (v4 or v6)"})]
        [SwaggerResponse(200, Type = typeof(PrefixDetailModel))]
        [SwaggerResponse(404, "When there is no API with the given ID or when searched prefix doesn't exist on selected API.", Type = typeof(ValidationProblemDetails))]
        [SwaggerResponse(400, "When provided prefix is not correctly formatted." , Type = typeof(ValidationProblemDetails))]
        public async Task<IActionResult> GetDetails(string prefix, byte cidr, int apiId)
        {
            return Ok(await provider.GetDetailsAsync(apiId, prefix, cidr));
        }
    }
}
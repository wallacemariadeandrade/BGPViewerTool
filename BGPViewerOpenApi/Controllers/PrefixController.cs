using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using BGPViewerOpenApi.Service;
using Microsoft.AspNetCore.Mvc;
using BGPViewerCore.Service;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using BGPViewerOpenApi.Validators;
using Swashbuckle.AspNetCore.Annotations;
using BGPViewerCore.Model;

namespace BGPViewerOpenApi.Controllers
{
    [Produces("application/json")]
    [Route("api/prefix")]
    [ApiController]
    [ValidateSelectedApiExistence]
    [SwaggerResponse(404, "When there is no API with the given ID.", Type = typeof(ValidationProblemDetails))]
    public class PrefixController : Controller
    {
        private readonly PrefixProvider provider;

        public PrefixController(PrefixProvider provider)
        {
            this.provider = provider;
        }

        [HttpGet("{prefix}/{cidr}/details/{apiId}")]
        [ValidatePrefix]
        [SwaggerOperation(Summary = "Retrieves details from provided prefix.", Description = "Supports IPv4 and IPv6 prefixes.", OperationId = "PrefixDetails", Tags = new [] {"Prefix (v4 or v6)"})]
        [SwaggerResponse(200, Type = typeof(PrefixDetailModel))]
        public async Task<IActionResult> GetDetails(string prefix, byte cidr, int apiId)
        {
            return Ok(await provider.GetDetailsAsync(apiId, prefix, cidr));
        }
    }
}
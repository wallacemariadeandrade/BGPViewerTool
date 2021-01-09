using System.Collections.Generic;
using System.Threading.Tasks;
using BGPViewerCore.Models;
using BGPViewerOpenApi.Model;
using BGPViewerOpenApi.Service;
using BGPViewerOpenApi.Validators;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BGPViewerOpenApi.Controllers
{
    [Produces("application/json")]
    [Route("api/{apiId}/as")]
    [ApiController]
    [ValidateSelectedApiExistence]
    [SwaggerResponse(404, "When there is no API with the given ID.", Type = typeof(ValidationProblemDetails))]
    public class AsController : ControllerBase
    {
        private readonly AsProvider provider;

        public AsController(AsProvider provider)
        {
            this.provider = provider;
        }

        [HttpGet("{asNumber}/details")]
        [SwaggerOperation(Summary = "Retrieves personal data from provided ASN.", OperationId = "ASDetails", Tags = new [] {"Autonomous System (AS)"})]
        [SwaggerResponse(200, Type = typeof(AsnDetailsModel))]
        public async Task<IActionResult> GetDetails(int asNumber, int apiId)
            => Ok(await provider.GetDetailsAsync(apiId, asNumber));

        [HttpGet("{asNumber}/prefixes")]
        [SwaggerOperation(Summary = "Retrieves known prefixes from provided ASN.", OperationId = "ASPrefixes", Tags = new [] {"Autonomous System (AS)"})]
        [SwaggerResponse(200, Type = typeof(AsnPrefixesModel))]
        public async Task<IActionResult> GetPrefixes(int asNumber, int apiId)
            => Ok(await provider.GetPrefixesAsync(apiId, asNumber));

        [HttpGet("{asNumber}/peers")]
        [SwaggerOperation(Summary = "Retrieves known peers from provided ASN.", OperationId = "ASPeers", Tags = new [] {"Autonomous System (AS)"})]
        [SwaggerResponse(200, Type = typeof(Peers))]
        public async Task<IActionResult> GetPeers(int asNumber, int apiId)
            => Ok(await provider.GetPeersAsync(apiId, asNumber));

        [HttpGet("{asNumber}/upstreams")]
        [SwaggerOperation(Summary = "Retrieves known upstreams from provided ASN.", OperationId = "ASUpstreams", Tags = new [] {"Autonomous System (AS)"})]
        [SwaggerResponse(200, Type = typeof(Peers))]
        public async Task<IActionResult> GetUpstreams(int asNumber, int apiId)
            => Ok(await provider.GetUpstreamsAsync(apiId, asNumber));

        [HttpGet("{asNumber}/downstreams")]
        [SwaggerOperation(Summary = "Retrieves known downstreams from provided ASN.", OperationId = "ASDownstreams", Tags = new [] {"Autonomous System (AS)"})]
        [SwaggerResponse(200, Type = typeof(Peers))]
        public async Task<IActionResult> GetDownstreams(int asNumber, int apiId)
            => Ok(await provider.GetDownstreamsAsync(apiId, asNumber));

        [HttpGet("{asNumber}/ixs")]
        [SwaggerOperation(Summary = "Retrieves known IXs from provided ASN.", OperationId = "ASIXs", Tags = new [] {"Autonomous System (AS)"})]
        [SwaggerResponse(200, Type = typeof(IEnumerable<IxModel>))]
        public async Task<IActionResult> GetIxs(int asNumber, int apiId)
            => Ok(await provider.GetIxsAsync(apiId, asNumber));
    }
}
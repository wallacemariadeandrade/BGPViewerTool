using System.Threading.Tasks;
using BGPViewerOpenApi.Service;
using Microsoft.AspNetCore.Mvc;

namespace BGPViewerOpenApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class AsController : ControllerBase
    {
        private readonly AsProvider provider;

        public AsController(AsProvider provider)
        {
            this.provider = provider;
        }

        [HttpGet("{asNumber}/details/{apiId}")]
        public async Task<IActionResult> GetDetails(int asNumber, int apiId)
            => Ok(await provider.GetDetailsAsync(apiId, asNumber));

        [HttpGet("{asNumber}/prefixes/{apiId}")]
        public async Task<IActionResult> GetPrefixes(int asNumber, int apiId)
            => Ok(await provider.GetPrefixesAsync(apiId, asNumber));

        [HttpGet("{asNumber}/peers/{apiId}")]
        public async Task<IActionResult> GetPeers(int asNumber, int apiId)
            => Ok(await provider.GetPeersAsync(apiId, asNumber));

        [HttpGet("{asNumber}/upstreams/{apiId}")]
        public async Task<IActionResult> GetUpstreams(int asNumber, int apiId)
            => Ok(await provider.GetUpstreamsAsync(apiId, asNumber));

        [HttpGet("{asNumber}/downstreams/{apiId}")]
        public async Task<IActionResult> GetDownstreams(int asNumber, int apiId)
            => Ok(await provider.GetDownstreamsAsync(apiId, asNumber));

        [HttpGet("{asNumber}/ixs/{apiId}")]
        public async Task<IActionResult> GetIxs(int asNumber, int apiId)
            => Ok(await provider.GetIxsAsync(apiId, asNumber));
    }
}
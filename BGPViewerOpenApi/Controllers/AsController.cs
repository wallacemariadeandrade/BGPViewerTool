using System.Threading.Tasks;
using BGPViewerOpenApi.Service;
using Microsoft.AspNetCore.Mvc;

namespace BGPViewerOpenApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsController : ControllerBase
    {
        private readonly Provider provider;

        public AsController(Provider provider)
        {
            this.provider = provider;
        }

        [HttpGet("{asNumber}/details/{apiId}")]
        public async Task<IActionResult> GetDetails(int asNumber, int apiId)
            => Ok(await provider.GetDetailsAsync(apiId, asNumber));

        [HttpGet("{asNumber}/prefixes/{apiId}")]
        public async Task<IActionResult> GetPrefixes(int asNumber, int apiId)
            => Ok(await provider.GetPrefixesAsync(apiId, asNumber));
    }
}
using System.Threading.Tasks;
using BGPViewerOpenApi.Service;
using Microsoft.AspNetCore.Mvc;

namespace BGPViewerOpenApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsnController : ControllerBase
    {
        private readonly Provider provider;

        public AsnController(Provider provider)
        {
            this.provider = provider;
        }

        [HttpGet("{asNumber}/details/{apiId}")]
        public async Task<IActionResult> GetDetails(int asNumber, int apiId)
        {
            var details = await provider.GetDetailsAsync(apiId, asNumber);
            return Ok(details);   
        }
    }
}
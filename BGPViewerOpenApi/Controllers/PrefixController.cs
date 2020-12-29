using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using BGPViewerOpenApi.Service;
using Microsoft.AspNetCore.Mvc;
using BGPViewerCore.Service;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using BGPViewerOpenApi.Validators;

namespace BGPViewerOpenApi.Controllers
{
    [Route("api/prefix")]
    [ApiController]
    [ValidateSelectedApiExistence]
    public class PrefixController : Controller
    {
        private readonly PrefixProvider provider;

        public PrefixController(PrefixProvider provider)
        {
            this.provider = provider;
        }

        [HttpGet("{prefix}/{cidr}/details/{apiId}")]
        [ValidatePrefix]
        public async Task<IActionResult> GetDetails(string prefix, byte cidr, int apiId)
        {
            return Ok(await provider.GetDetailsAsync(apiId, prefix, cidr));
        }
    }
}
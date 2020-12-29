using System.Threading.Tasks;
using BGPViewerCore.Model;
using BGPViewerOpenApi.Service;
using BGPViewerOpenApi.Validators;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BGPViewerOpenApi.Controllers
{
    [Produces("application/json")]
    [Route("api/search")]
    [ApiController]
    [ValidateSelectedApiExistence]
    public class SearchByController : Controller
    {
        private readonly SearchProvider provider;

        public SearchByController(SearchProvider provider)
        {
            this.provider = provider;
        }

        [HttpGet("{queryTerm}/{apiId}")]
        [SwaggerOperation(Summary = "Searches data on API based on provided term.", Description = "Common query terms are AS number, name, description, IP address and prefix.", OperationId = "Search", Tags = new [] {"Search"})]
        [SwaggerResponse(200, Type = typeof(SearchModel))]
        public async Task<IActionResult> Search(string queryTerm, int apiId)
            => Ok(await provider.Search(queryTerm, apiId));
    }
}
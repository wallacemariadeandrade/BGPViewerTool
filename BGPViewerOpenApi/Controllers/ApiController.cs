using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BGPViewerOpenApi.Model;
using BGPViewerOpenApi.Service;
using BGPViewerOpenApi.ViewModel.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BGPViewerOpenApi.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly ApiProvider provider;

        public ApiController(ApiProvider provider)
        {
            this.provider = provider;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Enumerates available API's to retrieve data.", Description = "Use it to get API's IDs", OperationId = "GetApis", Tags = new [] {"APIs"})]
        [SwaggerResponse(200, Type = typeof(IEnumerable<ApiResponse>))]
        public async Task<IActionResult> GetApis()
        {
            var apis = await provider.ListAvailableAsync();

            var viewModel = apis.Select(x => new ApiResponse {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description
            });

            return Ok(viewModel);
        }
    }
}
using System.Linq;
using System.Threading.Tasks;
using BGPViewerOpenApi.Service;
using BGPViewerOpenApi.ViewModel.Response;
using Microsoft.AspNetCore.Mvc;

namespace BGPViewerOpenApi.Controllers
{
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
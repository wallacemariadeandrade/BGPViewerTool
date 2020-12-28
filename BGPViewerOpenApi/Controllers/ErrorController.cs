using Microsoft.AspNetCore.Mvc;

namespace BGPViewerOpenApi.Controllers
{
    [Route("/Error")]
    [ApiController]
    public class ErrorController : Controller
    {
        public IActionResult Error() => Problem();
    }
}
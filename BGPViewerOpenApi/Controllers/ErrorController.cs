using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BGPViewerOpenApi.Controllers
{
    [Route("/Error")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : Controller
    {
        [HttpGet]
        public IActionResult Error()
        {
            var problem = Problem();
            var error = HttpContext.Features.Get<IExceptionHandlerFeature>().Error;

            if(error.GetType().IsAssignableFrom(typeof(KeyNotFoundException)) 
                || error.GetType().IsAssignableFrom(typeof(ArgumentException)))
            {
                var problemDetails = problem.Value as ProblemDetails;
                problemDetails.Detail = error.Message;
            }

            return problem;
        }
    }
}
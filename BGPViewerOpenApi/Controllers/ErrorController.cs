using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
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
            var error = HttpContext.Features.Get<IExceptionHandlerFeature>().Error;
            var objectResult = Problem();
            var oldProblemDetailsObject = objectResult.Value as ProblemDetails;

            var newObjectProblem = new ValidationProblemDetails
            {
                Type = oldProblemDetailsObject.Title,
                Detail = "Please refer to the errors property for additional details",
                Title = oldProblemDetailsObject.Title,
                Status = oldProblemDetailsObject.Status,
                Instance = oldProblemDetailsObject.Instance,
            };

            objectResult.Value = newObjectProblem;

            if(error.GetType().IsAssignableFrom(typeof(KeyNotFoundException)) 
                || error.GetType().IsAssignableFrom(typeof(ArgumentException)))
            {
                newObjectProblem.Status = StatusCodes.Status404NotFound;
                objectResult.StatusCode = StatusCodes.Status404NotFound;
                newObjectProblem.Errors.Add("error", new string[]{ error.Message });
            }
            else
                newObjectProblem.Errors.Add("error", new string[]{ oldProblemDetailsObject.Detail });

            return objectResult;
        }
    }
}
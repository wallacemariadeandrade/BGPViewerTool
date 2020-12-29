using BGPViewerOpenApi.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BGPViewerOpenApi.Validators
{
    public class ValidateSelectedApiExistence : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var apiId = int.Parse(context.RouteData.Values["apiId"].ToString());

            var provider = context.HttpContext.RequestServices.GetService(typeof(ApiProvider)) as ApiProvider;

            if(!provider.CheckIfExists(apiId))
            {
                var problemDetails = new ValidationProblemDetails {
                    Detail = "Please refer to the errors property for additional details",
                    Status = 404,
                    Instance = apiId.ToString(),
                    Title = "One or more validation errors occurred.",
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",  
                };

                problemDetails.Errors.Add("apiId", new string[]{$"Do not exist an API with ID {apiId}."});
                
                context.Result = new ObjectResult(problemDetails);
            }
        }
    }
}
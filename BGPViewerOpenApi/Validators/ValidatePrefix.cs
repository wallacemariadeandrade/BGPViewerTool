using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.ComponentModel.DataAnnotations;
using static BGPViewerCore.Service.RegexPatterns;
using static System.Text.RegularExpressions.Regex;

namespace BGPViewerOpenApi.Validators
{
    public class ValidatePrefix : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var prefix = context.RouteData.Values["prefix"] as string;
            var cidr = context.RouteData.Values["cidr"] as string;
            var fullPrefix = $"{prefix}/{cidr}";

            if(!(IsMatch(fullPrefix, IPV4_PREFIX_PATTERN) || IsMatch(fullPrefix, IPV6_PREFIX_PATTERN)))
            {
                var problemDetails = new ValidationProblemDetails {
                    Detail = "Please refer to the errors property for additional details",
                    Status = 400,
                    Instance = fullPrefix,
                    Title = "One or more validation errors occurred.",
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",  
                };

                problemDetails.Errors.Add("prefix", new string[]{"Provided prefix was not correctly formatted."});
                
                context.Result = new ObjectResult(problemDetails);
            }
        }
    }
}
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

/* since we’ve implemented custom media types, we want our Accept header to be present 
* in our requests so we can detect when the user requested the HATEOAS-enriched response.
* We check for the existence of the Accept header first. If it’s not present, we return BadRequest. 
* If it is, we parse the media type — and if there is no valid media type present, we return BadRequest.
* Once we’ve passed the validation checks, we pass the parsed media type to the HttpContext of the controller.*/

namespace Presentation.ActionFilters
{
    public class ValidateMediaTypeAttribute : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var acceptHeaderPresent = context.HttpContext
            .Request.Headers.ContainsKey("Accept");

            if (!acceptHeaderPresent)
            {
                context.Result = new BadRequestObjectResult($"Accept header is missing.");
                return;
            }

            var mediaType = context.HttpContext
            .Request.Headers["Accept"].FirstOrDefault();

            if (!MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue?
            outMediaType))
            {
                context.Result = new BadRequestObjectResult
                    ($"Media type not present.Please add Accept header with the required media type.");

                return;
            }

            context.HttpContext.Items.Add("AcceptHeaderMediaType", outMediaType);
        }

        public void OnActionExecuted(ActionExecutedContext context) { }

    }
}

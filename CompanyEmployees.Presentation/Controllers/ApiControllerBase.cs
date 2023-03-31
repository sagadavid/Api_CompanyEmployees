using Entities.ErrorModel;
using Entities.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    /* we have to create a way to handle error responses and return them to 
     * the client – similar to what we have with the global error handler middleware.
     * We are not going to create any additional middleware but another controller base class */
   
    public class ApiControllerBase : ControllerBase
    {
        public IActionResult ProcessError(ApiBaseResponse baseResponse)
        {
            return baseResponse switch 
            /* This class inherits from the ControllerBase class and implements a single ProcessError 
             * action accepting an ApiBaseResponse parameter. Inside the action, we are inspecting 
             * the type of the sent parameter, and based on that type we return an appropriate message to the client.
             * A similar thing we did in the exception middleware class.If you add additional error response 
             * classes to the Response folder, you only have to add them here to process the response for the
             * client. Additionally, this is where we can see the advantage of our abstraction approach. */
            {
                ApiNotFoundResponse => NotFound(new ErrorDetails
                {
                    Message = ((ApiNotFoundResponse)baseResponse).Message,
                    StatusCode = StatusCodes.Status404NotFound
                }),

                ApiBadRequestResponse => BadRequest(new ErrorDetails
                {
                    Message = ((ApiBadRequestResponse)baseResponse).Message,
                    StatusCode = StatusCodes.Status400BadRequest
                }),

                _ => throw new NotImplementedException()

            };

        }

    }
}

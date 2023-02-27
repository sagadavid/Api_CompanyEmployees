using Contracts;
using Entities.ErrorModel;
using Entities.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace CodeMaze_CompanyEmployees.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this WebApplication app,//The web application used to configure the HTTP pipeline, and routes.
            ILoggerManager logger)
        {
            /*Adds a middleware to the pipeline that will catch exceptions, log them, 
             * and re-execute the request in an alternate pipeline.
        /// The request will not be re-executed if the response has already started.*/
            app.UseExceptionHandler(appError =>
            {
                /*Adds a terminal middleware delegate to the application's request pipeline.*/
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        context.Response.StatusCode = contextFeature.Error switch
                        {
                            NotFoundException => StatusCodes.Status404NotFound,
                            _ => StatusCodes.Status500InternalServerError
                        };
                        logger.LogError($"noen er feil her: {contextFeature.Error}");

                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,
                            //Message = "Internal Server Error",
                            Message=contextFeature.Error.Message,
                        }.ToString());
                    }
                });

            });
        }
    }
}
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Entities.ErrorModel
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }

        public override string ToString() => JsonSerializer.Serialize(this);
        /*
         Even though there is nothing wrong with the try-catch blocks in our 
        Actions and methods in the Web API project, we can extract all the exception 
        handling logic into a single centralized place. By doing that, we make our 
        actions cleaner, more readable, and the error handling process more maintainable.
       
        The UseExceptionHandler middleware is a built-in middleware
        that we can use to handle exceptions.
         */
    }
}

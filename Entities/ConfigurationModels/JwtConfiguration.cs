using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* Having to type sections and keys to get the values can be repetitive and error-prone.
 * We risk introducing errors to our code, and these kinds of errors can cost us a lot 
 * of time until we discover them since someone else can introduce them, and we won’t 
 * notice them since a null result is returned when values are missing.
 * To overcome this problem, 
 * we can bind configuration data to strongly typed objects. */

namespace Entities.ConfigurationModels
{
    public class JwtConfiguration
    {
        public string Section { get; set; } = "JwtSettings";
        public string? ValidIssuer { get; set; }
        public string? ValidAudience { get; set; }
        public string? Expires { get; set; }

    }
}

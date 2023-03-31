using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Response
{
    public abstract class ApiBaseResponse//the main return type for all of our methods
    {
        public bool Success { get; set; }
        protected ApiBaseResponse(bool success) => Success = success;
    }
}

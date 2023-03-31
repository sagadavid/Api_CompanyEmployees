using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Response
{
    public sealed class CompanyNotFoundResponse : ApiNotFoundResponse
    {
        //accepts an id parameter and creates a message that sends to the base class.
        public CompanyNotFoundResponse(Guid id) : base($"Company with id: {id} is not found in db.")
        {

        }

    }
}

using Entities.LinkModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    /* If we inspect our CompaniesController , we can see that GetCompanies and CreateCompany 
     * are the only actions on the root URI level (api/companies). 
     * Therefore, we are going to create links only to them. */

    [Route("api")]
    [ApiController]
    public class RootController:ControllerBase
    {
        private readonly LinkGenerator _linkGenerator;
        public RootController(LinkGenerator linkGenerator) => _linkGenerator = linkGenerator;

        [HttpGet(Name = "GetRoot")]
        public IActionResult GetRoot([FromHeader(Name = "Accept")] string mediaType)
        {
            //if (mediaType.Contains("application/vnd.codemaze.apiroot"))//In this action, we generate links only if a custom media type is provided from the Accept header. Otherwise, we return NoContent() .
            //{
                var list = new List<Link>
                {
                    new Link
                    {
                        Href = _linkGenerator.GetUriByName(HttpContext, nameof(GetRoot), new {}),
                        Rel = "self",
                        Method = "GET"
                    },
                    new Link
                    {
                        Href = _linkGenerator.GetUriByName(HttpContext, "GetCompanies", new {}),
                        Rel = "companies",
                        Method = "GET"
                    },
                    new Link
                    {
                        Href = _linkGenerator.GetUriByName(HttpContext, "CreateCompany", new {}),
                        Rel = "create_company",
                        Method = "POST"
                    }
                };

            return Ok(list);
            //}

            //return NoContent();
        }
    }
}

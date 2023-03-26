using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Presentation.ModelBinders;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Presentation.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/companies")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private IServiceManager _serviceManger;

        public CompaniesController(IServiceManager serviceManager) => _serviceManger = serviceManager;

        [HttpGet]
        public async Task<IActionResult> GetCompanies()//when we async modify, dont need to add to method names in controller
        {
            /*no need for try-catch, after error hanler middleware added*/
            //try {/*check if error handler middleware is working*/
            //throw new Exception("Exception");

            var companies = await _serviceManger.CompanyService
                .GetAllCompaniesAsync(trackChanges: false);
            return Ok(companies);
            //}//catch { return StatusCode(500, "internal server error");}

        }

        [HttpGet("{companyId:guid}", Name = "GetCompanyById")]
        public async Task<IActionResult> GetCompany(Guid companyId)
        {
            var company =await _serviceManger.CompanyService
                .GetCompanyByIdAsync(companyId, trackChanges: false);
            return Ok(company);
        }

        [HttpPost]
        //[ServiceFilter(typeof(ValidationFilterAttribute))]//validation instead if company checks below
        public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDto company)
        {
            if (company is null)
                return BadRequest("CompanyForCreationDto object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);//now we get on invalid posting -->422 unprocessable entity..

            var createdCompany = await _serviceManger.CompanyService.CreateCompanyAsync(company);
            return CreatedAtRoute("GetCompanyById", new { id = createdCompany.Id }, createdCompany);
            
            ////here, createdatroute requires name decoaration of get api, 
            ///if it is misspelled post request doesnt turn with body on postman
            ///response body contains location header au
        }

        [HttpGet("collection/({ids})", Name = "CompanyCollection")]
        public IActionResult GetCompanyCollection
            ([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            var companies = _serviceManger.CompanyService.GetByIdsAsync(ids, trackChanges: false);
            return Ok(companies);
        }

        [HttpPost("collection")]
        public async Task<IActionResult> CreateCompanyCollection
            ([FromBody] IEnumerable<CompanyForCreationDto> companyCollection)
        {
            var result =await _serviceManger.CompanyService
                .CreateCompanyCollectionAsync(companyCollection);
            return CreatedAtRoute
                ("CompanyCollection", new { result.ids }, result.companies);

            /*postman post : https://localhost:7165/api/companies/collection
             response body location : https://localhost:7165/api/companies/collection/(5a706767-9c19-4c21-3a80-08db1a7422b6,df651ec2-0615-4be0-3a81-08db1a7422b6)
             */
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCompany(Guid id)
        {
            await _serviceManger.CompanyService
                .DeleteCompanyAsync(id, trackChanges: false);
            return NoContent();
        }

        [HttpPut("{id:guid}")]
        //[ServiceFilter(typeof(ValidationFilterAttribute))]//no need for company check codes below 
        public async Task<IActionResult> UpdateCompany
            (Guid id,[FromBody] CompanyForUpdateDto company)
        {
            if (company is null) return BadRequest("CompanyForUpdateDto object is null");

            await _serviceManger.CompanyService
                .UpdateCompanyAsync(id, company, trackChanges: true);
            return NoContent();

        }
        /* We have to decorate our action with the HttpOptions attribute.
         * As we said, the available options should be returned in the 
         * Allow response header, and that is exactly what we are doing here. 
         * The URI for this action is /api/companies , so we state which actions can be executed for 
         * that certain URI. Finally, the Options request should return the 200 OK status code. 
         * We have to understand that the response, if it is empty, must include the content-length 
         * field with the value of zero. We don’t have to add it by ourselves because ASP.NET Core 
         * takes care of that for us.*/
        
        [HttpOptions]
        public IActionResult GetCompaniesOptions()
        {
            Response.Headers.Add("Allow", "GET, OPTIONS, POST, PUT, PATCH, DELETE");
            return Ok();
        }

    }
}

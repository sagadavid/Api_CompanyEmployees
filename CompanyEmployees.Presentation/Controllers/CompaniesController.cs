using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using Presentation.ActionFilters;
using Presentation.ModelBinders;
using Service.Contracts;
using Shared.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Entities.Response;
using Presentation.Controllers;
using Presentation.Extensions;

namespace CompanyEmployees.Presentation.Controllers
{
    /* commentet to give space to response performance improvement
     * [ApiVersion("1.0")]////no need to use when version by convention is added in extension method
      [Route("api/companies")]
      [ApiController]
      [ApiExplorerSettings(GroupName = "v1")]
      public class CompaniesController : ControllerBase
      {
          private IServiceManager _serviceManger;

          public CompaniesController(IServiceManager serviceManager) => _serviceManger = serviceManager;

          /// <summary>
          /// Gets the list of all companies
          /// </summary>
          /// <returns>The companies list</returns>
          [HttpGet(Name ="GetCompanies")]
          //[ResponseCache(CacheProfileName = "120SecondsDuration")]//configured in program.cs..now, this cache rule
          //will apply to all the actions inside the controller EXCEPT
          //the ones that already have the ResponseCache attribute applied.
          [Authorize(Roles ="Manager")]
          public async Task<IActionResult> GetCompanies()//when we async modify, dont need to add to method names in controller
          {
              //no need for try-catch, after error hanler middleware added
              //try {//check if error handler middleware is working
              //throw new Exception("Exception");

              var companies = await _serviceManger.CompanyService
                  .GetAllCompaniesAsync(trackChanges: false);
              return Ok(companies);
              //}//catch { return StatusCode(500, "internal server error");}

          }

          [HttpGet("{companyId:guid}", Name = "GetCompanyById")]
          //[ResponseCache(Duration = 60)]//cashing by attribute // commented for marvin.cash.header to work
          //local cashing(attribute) overrrides the global one ! example below..
          [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 60)]
          [HttpCacheValidation(MustRevalidate = false)]
          public async Task<IActionResult> GetCompany(Guid companyId)
          {
              var company =await _serviceManger.CompanyService
                  .GetCompanyByIdAsync(companyId, trackChanges: false);
              return Ok(company);
          }

          /// <summary>
          /// Creates a newly created company
          /// </summary>
          /// <param name="company"></param>
          /// <returns>A newly created company</returns>
          /// <response code="201">Returns the newly created item</response>
          /// <response code="400">If the item is null</response>
          /// <response code="422">If the model is invalid</response>
          [HttpPost(Name = "CreateCompany")]
          [ProducesResponseType(201)]
          [ProducesResponseType(400)]
          [ProducesResponseType(422)]
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

              //postman post : https://localhost:7165/api/companies/collection
               //response body location : https://localhost:7165/api/companies/collection/(5a706767-9c19-4c21-3a80-08db1a7422b6,df651ec2-0615-4be0-3a81-08db1a7422b6)

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
          //* We have to decorate our action with the HttpOptions attribute.
           * As we said, the available options should be returned in the 
           * Allow response header, and that is exactly what we are doing here. 
           * The URI for this action is /api/companies , so we state which actions can be executed for 
           * that certain URI. Finally, the Options request should return the 200 OK status code. 
           * We have to understand that the response, if it is empty, must include the content-length 
           * field with the value of zero. We don’t have to add it by ourselves because ASP.NET Core 
           * takes care of that for us.

          [HttpOptions]
          public IActionResult GetCompaniesOptions()
          {
              Response.Headers.Add("Allow", "GET, OPTIONS, POST, PUT, PATCH, DELETE");
              return Ok();
          }
  } */
    //response performance improvement implementation
    [Route("api/companies")]
    [ApiController]
    public class CompaniesController : ApiControllerBase
    {
        private readonly IServiceManager _service;
        public CompaniesController(IServiceManager service) => _service = service;
        
        [HttpGet]
        public IActionResult GetCompanies()
        {
            var baseResult = _service.CompanyService.GetAllCompanies(trackChanges:
            false);
            //var companies =((ApiOkResponse<IEnumerable<CompanyDto>>)baseResult).Result;
            var companies = baseResult.GetResult<IEnumerable<CompanyDto>>();//This is much cleaner and easier to read and understand.
            return Ok(companies);
        }

        [HttpGet("{id:guid}")]
        public IActionResult GetCompany(Guid id)
        {
            var baseResult = _service.CompanyService.GetCompany(id, trackChanges: false);
            if (!baseResult.Success)
                return ProcessError(baseResult);
            //var company = ((ApiOkResponse<CompanyDto>)baseResult).Result;
            var company = baseResult.GetResult<CompanyDto>();//This is much cleaner and easier to read and understand.
            return Ok(company);
        }

    }

}

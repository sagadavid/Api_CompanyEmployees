using Microsoft.AspNetCore.Mvc;
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

        public CompaniesController(IServiceManager service)=> _serviceManger=service;

        [HttpGet]
        public IActionResult GetCompanies()
        {
            /*no need for try-catch, after error hanler middleware added*/
            //try
            //{

            /*check if error handler middleware is working*/
            //throw new Exception("Exception");

            var companies = _serviceManger.CompanyService.GetAllCompanies(trackChanges: false);
            return Ok(companies);
            //}
            //catch 
            //{
            //    return StatusCode(500, "internal server error");
            //}
        
        }

        [HttpGet("id:guid", Name ="GetCompanyById")]
        public IActionResult GetCompany(Guid companyId)
        { 
            var company = _serviceManger.CompanyService.GetCompanyById(companyId, trackChanges: false);

            return Ok(company);
        }

        [HttpPost]
        public IActionResult CreateCompany([FromBody] CompanyForCreationDto company)
        {
            if (company is null)
                return BadRequest("CompanyForCreationDto object is null");
            var createdCompany = _serviceManger.CompanyService.CreateCompany(company);
            return CreatedAtRoute("GetCompanyById", new { id = createdCompany.Id }, createdCompany);
            ////here, createdatroute requires name decoaration of get api, 
            ///if it is misspelled post request doesnt turn with body on postman
            ///response body contains location header au
        }

    }
}

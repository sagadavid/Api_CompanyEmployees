using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace CompanyEmployees.Presentation.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/companies")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private IServiceManager _service;

        public CompaniesController(IServiceManager service)=> _service=service;

        [HttpGet]
        public IActionResult GetCompanies()
        {
            /*no need for try-catch, after error hanler middleware added*/
            //try
            //{

            /*check if error handler middleware is working*/
            //throw new Exception("Exception");

            var companies = _service.CompanyService.GetAllCompanies(trackChanges: false);
            return Ok(companies);
            //}
            //catch 
            //{
            //    return StatusCode(500, "internal server error");
            //}
        
        }

    }
}

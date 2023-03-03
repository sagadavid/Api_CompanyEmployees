using Microsoft.AspNetCore.Mvc;
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

        public CompaniesController(IServiceManager serviceManager)=> _serviceManger=serviceManager;

        [HttpGet]
        public IActionResult GetCompanies()
        {
            /*no need for try-catch, after error hanler middleware added*/
            //try
            //{

            /*check if error handler middleware is working*/
            //throw new Exception("Exception");

            var companies = _serviceManger.CompanyService
                .GetAllCompanies(trackChanges: false);
            return Ok(companies);
            //}
            //catch 
            //{
            //    return StatusCode(500, "internal server error");
            //}
        
        }

        [HttpGet("{companyId:guid}", Name ="GetCompanyById")]
        public IActionResult GetCompany(Guid companyId)
        { 
            var company = 
                _serviceManger.CompanyService
                .GetCompanyById(companyId, trackChanges: false);

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

        [HttpGet("collection/({ids})", Name = "CompanyCollection")]
        public IActionResult GetCompanyCollection
            ([ModelBinder(BinderType =typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            var companies = _serviceManger.CompanyService.GetByIds(ids, trackChanges: false);
            return Ok(companies);
        }

        [HttpPost("collection")]
        public IActionResult CreateCompanyCollection
            ([FromBody] IEnumerable<CompanyForCreationDto> companyCollection)
        {
            var result =
            _serviceManger.CompanyService.CreateCompanyCollection(companyCollection);
            return CreatedAtRoute
                ("CompanyCollection", new { result.ids },result.companies);
            /*postman post : https://localhost:7165/api/companies/collection
             response body location : https://localhost:7165/api/companies/collection/(5a706767-9c19-4c21-3a80-08db1a7422b6,df651ec2-0615-4be0-3a81-08db1a7422b6)
             */
        }

        [HttpDelete("{id:guid}")]
        public IActionResult DeleteCompany(Guid id)
        {
            _serviceManger.CompanyService
                .DeleteCompany(id, trackChanges: false);
            return NoContent();
        }

        [HttpPut("{id:guid}")]
        public IActionResult UpdateCompany
            (Guid id, 
            [FromBody] CompanyForUpdateDto company)
        {
            if (company is null) return BadRequest("CompanyForUpdateDto object is null");

            _serviceManger.CompanyService
                .UpdateCompany(id, company, trackChanges: true);

            return NoContent();
            /*company updates, employee ADDS !! check comment in the dto..
             * postman put 
             * https://localhost:7165/api/companies/3d490a70-94ce-4d15-9494-5248280c2ce3
             * put body
             * {
                    "name":"company 1",
                    "address": "312 Forest Avenue", 
                    "country":"VA 22202 USA",
                    "employees": [{
                    "name" : "employee 1",
                    "age": "77",
                    "position":"owner"
                        }]
                }
             * postman get 
             * https://localhost:7165/api/companies/3d490a70-94ce-4d15-9494-5248280c2ce3/EMPLOYEES
             * response body
             * [
                        {
                            "id": "d8f7f7cb-283a-420b-fcf3-08db1bccd643",
                            "name": "employee 1",
                            "age": 77,
                            "position": "owner"
                        },
                        {
                            "id": "6543ec34-d129-4b73-113e-08db1b4adedf",
                            "name": "hanri cahnged his age",
                            "age": 34,
                            "position": "leder"
                        }
                ]
             */
        }

    }
}

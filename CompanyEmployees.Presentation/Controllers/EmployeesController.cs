using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Route("api/companies/{companyId}/employees")]
    public class EmployeesController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public EmployeesController(IServiceManager serviceManager) =>
            _serviceManager = serviceManager;

        [HttpGet]
        public IActionResult GetEmployeesPerCompany(Guid companyId)
        {
            var employees = _serviceManager.EmployeeService.GetEmployees
                    (companyId, trackChanges: false);
            return Ok(employees);
            /*we have the companyId parameter in our action and this parameter will be 
             * mapped from the main route. For that reason, we didn’t place it in the 
             * [HttpGet] attribute as we did with the GetCompany action.*/
        }

        [HttpGet("{id:guid}", Name = "GetEmployeeForCompany")]//we provide parameters for post/createdatroute 

        public IActionResult GetEmployeeForCompany(Guid companyId, Guid id)

        {
            var employee = _serviceManager.EmployeeService.GetEmployee(companyId, id, trackChanges: false);
            return Ok(employee);
        }

        [HttpPost]
        public IActionResult CreateEmployeeForCompany
            (Guid companyId, [FromBody] EmployeeForCreationDto employee)
        {
            if (employee is null)
                return BadRequest("EmployeeForCreationDto object is null");
            var employeeToReturn =
            _serviceManager.EmployeeService.CreateEmployeeForCompany
                    (companyId, employee, trackChanges: false);

            return CreatedAtRoute("GetEmployeeForCompany", 
                new {companyId, id = employeeToReturn.Id},employeeToReturn);}
        /*
         postman post : https://localhost:7165/api/companies/0eec53d0-6091-40d6-b994-08db19cfa35a/employees
        json row body at post : {"name":"david saga", "age":"45","position":"team ansvarlig" }
        postman get : https://localhost:7165/api/companies/0eec53d0-6091-40d6-b994-08db19cfa35a/employees/f978edfc-a8e1-4506-bef0-08db19e133e2
        postman get response body : 
        {
    "id": "f978edfc-a8e1-4506-bef0-08db19e133e2",
    "name": "david saga",
    "age": 45,
    "position": "team ansvarlig"
        }
         */


        [HttpDelete("{id:guid}")]
        public IActionResult DeleteEmployeeForCompany(Guid companyId, Guid id)
        {
            _serviceManager.EmployeeService.DeleteEmployeeForCompany
                (companyId, id, trackChanges: false);
            return NoContent();
            /*postman delete : https://localhost:7165/api/companies/3d490a70-94ce-4d15-9494-5248280c2ce3/employees/021ca3c1-0deb-4afd-ae94-2159a8479811
             postman get : https://localhost:7165/api/companies/3d490a70-94ce-4d15-9494-5248280c2ce3/employees/021ca3c1-0deb-4afd-ae94-2159a8479811
            response body : 
            {
    "StatusCode": 404,
    "Message": "employee in this id 021ca3c1-0deb-4afd-ae94-2159a8479811 is not here"
}
             */
        }


    }
}

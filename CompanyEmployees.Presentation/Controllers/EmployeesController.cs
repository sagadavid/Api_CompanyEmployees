using Microsoft.AspNetCore.JsonPatch;
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
        private readonly 
            IServiceManager _serviceManager;

        public 
            EmployeesController (IServiceManager serviceManager) =>
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

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);//now we get on invalid posting -->422 unprocessable entity..
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

        [HttpPut("{id:guid}")]//api/companies/{companyId}/employees/{id}
        public IActionResult UpdateEmployeeForCompany
                    (Guid companyId,
                    Guid id,
                    [FromBody] EmployeeForUpdateDto employee)
        {
            if (employee is null) return BadRequest("EmployeeForUpdateDto object is null");

            if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

            _serviceManager.EmployeeService.UpdateEmployeeForCompany
                (companyId, id, employee, compTrackChanges: false, empTrackChanges: true);

            return NoContent();

            /*
             postman change property value in put/body, and then 
            check the same url by get/body
            https://localhost:7165/api/companies/3d490a70-94ce-4d15-9494-5248280c2ce3/employees/6543ec34-d129-4b73-113e-08db1b4adedf
             
             We’ve changed only the Age property, but we have sent all the other properties 
            with unchanged values as well. Therefore, Age is only updated in the database. 
            But if we send the object with just the Age property, other properties will be 
            set to their default values and the whole object will be updated — not just the 
            Age column. That’s because the PUT is a request for a full update. This is very 
            important to know.

            The update action we just executed is a connected update 
            (an update where we use the same context object to fetch the entity and to update it). 
            But sometimes we can work with disconnected updates. This kind of update action uses 
            different context objects to execute fetch and update actions or sometimes we can 
            receive an object from a client with the Id property set as well, so we don’t have to 
            fetch it from the database. In that situation, all we have to do is to inform EF Core to
            track changes on that entity and to set its state to modified. We can do both actions 
            with the Update method from our RepositoryBase class. So, you see, having that method 
            is crucial as well.
            One note, though. If we use the Update method from our repository, even if we change 
            just the Age property, all properties will be updated in the database.
             */
        }


        [HttpPatch("{id:guid}")]
            public IActionResult PartiallyUpdateEmployeeForCompany
               (Guid companyId, 
                Guid id, 
                [FromBody] 
                    JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
        {
            if (patchDoc is null) 
                return BadRequest("patchDoc object sent from client is null.");

            var result = 
                _serviceManager.EmployeeService
                .GetEmployeeForPatch (companyId, 
                                        id, 
                                        compTrackChanges: false, 
                                        empTrackChanges: true);

            patchDoc.ApplyTo (result.employeeToPatch, ModelState);

            TryValidateModel(result.employeeToPatch);

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            _serviceManager.EmployeeService
                .SaveChangesForPatch
                        (result.employeeToPatch,
                         result.employeeEntity);

            return NoContent();

        }


    }
}

﻿using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Service.Contracts;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.RequestFeatures;
using System.Fabric.Query;
using System.Text.Json;

namespace Presentation.Controllers
{
    [Route("api/companies/{companyId}/employees")]
    public class EmployeesController : ControllerBase
    {
        private readonly
            IServiceManager _serviceManager;

        public
            EmployeesController(IServiceManager serviceManager) =>
            _serviceManager = serviceManager;

        [HttpGet]
        [HttpHead]//The Head is identical to Get but without a response body. This type of request could be used to obtain information about validity, accessibility, and recent modifications of the resource.we receive a 200 OK status code with the empty body BUT WITH PAGINATION.
        public async Task<IActionResult> GetEmployeesForCompany
            (Guid companyId, [FromQuery] EmployeeParameters employeeParameters)
        {
            //var employees = await _serviceManager.EmployeeService.GetEmployeesAsync
            //(companyId, employeeParameters, trackChanges: false);
            //return Ok(employees);

            //pagedlist version
            var pagedResult = await _serviceManager.EmployeeService.GetEmployeesAsync
                (companyId,employeeParameters, trackChanges: false);
            Response.Headers.Add("X-Pagination",
            JsonSerializer.Serialize(pagedResult.metaData));//now we have some additional useful information
                                                            //in the X-Pagination response header
                                                            //{"CurrentPage":5,"TotalPages":8,"PageSize":1,"TotalCount":8,"HasPrevious":true,"HasNext":true}
            return Ok(pagedResult.employees);
        }

        [HttpGet("{id:guid}", Name = "GetEmployeeForCompany")]//we provide parameters for post/createdatroute 
        public async Task<IActionResult> GetEmployeeForCompany(Guid companyId, Guid id)
        {
            var employee = await _serviceManager.EmployeeService.GetEmployeeByIdAsync(companyId, id, trackChanges: false);
            return Ok(employee);
        }

        [HttpPost]
        //[ServiceFilter(typeof(ValidationFilterAttribute))]//drop checks below
        public async Task<IActionResult> CreateEmployeeForCompany
            (Guid companyId, [FromBody] EmployeeForCreationDto employee)
        {
            if (employee is null)
                return BadRequest("EmployeeForCreationDto object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);//now we get on invalid posting -->422 unprocessable entity..
            var employeeToReturn =
            await _serviceManager.EmployeeService.CreateEmployeeForCompanyAsync
                    (companyId, employee, trackChanges: false);

            return CreatedAtRoute("GetEmployeeForCompany",
                new { companyId, id = employeeToReturn.Id }, employeeToReturn);
        }
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
        public async Task<IActionResult> DeleteEmployeeForCompany(Guid companyId, Guid id)
        {
            await _serviceManager.EmployeeService.DeleteEmployeeForCompanyAsync
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
        //[ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateEmployeeForCompany
                    (Guid companyId,
                    Guid id,
                    [FromBody] EmployeeForUpdateDto employee)
        {
            if (employee is null) return BadRequest("EmployeeForUpdateDto object is null");

            if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

            await _serviceManager.EmployeeService.UpdateEmployeeForCompanyAsync
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
        public async Task<IActionResult> PartiallyUpdateEmployeeForCompany
               (Guid companyId,
                Guid id,
                [FromBody]
                    JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
        {
            if (patchDoc is null)
                return BadRequest("patchDoc object sent from client is null.");

            var result =
                await _serviceManager.EmployeeService
                .GetEmployeeForPatchAsync(companyId,
                                        id,
                                        compTrackChanges: false,
                                        empTrackChanges: true);

            patchDoc.ApplyTo(result.employeeToPatch, ModelState);

            TryValidateModel(result.employeeToPatch);

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            await _serviceManager.EmployeeService
                .SaveChangesForPatchAsync
                        (result.employeeToPatch,
                         result.employeeEntity);

            return NoContent();

        }


    }
}

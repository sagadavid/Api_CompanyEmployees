using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
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
            var employess = _serviceManager.EmployeeService.GetEmployees
                    (companyId, trackChanges: false);
            return Ok(employess);
            /*we have the companyId parameter in our action and this parameter will be 
             * mapped from the main route. For that reason, we didn’t place it in the 
             * [HttpGet] attribute as we did with the GetCompanyById action.*/
        }

        [HttpGet("{id:guid}")]

        public IActionResult GetEmployeeForCompany(Guid companyId, Guid id)

        {

            var employee = _serviceManager.EmployeeService.GetEmployee(companyId, id, trackChanges: false);
            return Ok(employee);

        }
    }
}

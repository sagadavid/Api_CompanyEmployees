using Entities.LinkModels;
using Entities.Models;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IEmployeeService
    {
        //Now the Tuple return type has the LinkResponse as the first field and also
        //we have LinkParameters as the second parameter.
        Task<(LinkResponse linkResponse, MetaData metaData)> GetEmployeesAsync
            (Guid companyId, LinkParameters linkParameters, bool trackChanges);

        Task <EmployeeDto> GetEmployeeByIdAsync
            (Guid companyId, 
            Guid id, 
            bool trackChanges);

        Task <EmployeeDto> CreateEmployeeForCompanyAsync
            (Guid companyId, 
            EmployeeForCreationDto employeeForCreation, 
            bool trackChanges);

        Task DeleteEmployeeForCompanyAsync
            (Guid companyId, 
            Guid id, 
            bool trackChanges);

        Task UpdateEmployeeForCompanyAsync
            (Guid companyId, 
            Guid id, 
            EmployeeForUpdateDto employeeForUpdate, 
            bool compTrackChanges, 
            bool empTrackChanges);//We are doing that because we won't track changes
                                  //while fetching the company entity, but we will
                                  //track changes while fetching the employee.

        Task <(EmployeeForUpdateDto employeeToPatch, 
        Employee employeeEntity)> 
                GetEmployeeForPatchAsync
                       (Guid companyId, 
                        Guid id, 
                        bool compTrackChanges, 
                        bool empTrackChanges);

        Task SaveChangesForPatchAsync
            (EmployeeForUpdateDto employeeToPatch,
            Employee employeeEntity);


    }
}

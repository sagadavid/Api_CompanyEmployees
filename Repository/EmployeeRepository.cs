using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(RepositoryContext repositoryContext) :
                base(repositoryContext)
        { }

        public async Task<IEnumerable<Employee>> GetEmployeesAsync//pagination added/employee parameter
            (Guid companyId, EmployeeParameters employeeParameters, bool trackChanges) =>
       await FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges)
            .OrderBy(e => e.Name)
            .Skip((employeeParameters.PageNumber - 1) * employeeParameters.PageSize)//skip the first ((3 – 1) * 20) = 40 results,
            .Take(employeeParameters.PageSize)//then take the next 20 results and return them to the caller.
            .ToListAsync();

        public async Task<Employee> GetEmployeeByIdAsync
            (Guid companyId, Guid id, bool trackChanges) =>
        await FindByCondition(e => e.CompanyId.Equals(companyId) && 
                                    e.Id.Equals(id), trackChanges)
            .SingleOrDefaultAsync();

        public void CreateEmployeeForCompany(Guid companyId, Employee employee)
        {
            employee.CompanyId = companyId;
            Create(employee);
        }

        public void DeleteEmployee(Employee employee) => Delete(employee);
    }
}

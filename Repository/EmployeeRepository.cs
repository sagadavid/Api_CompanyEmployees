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
        ///pagination added/employee parameter
        // public async Task<IEnumerable<Employee>> GetEmployeesAsync
        //     (Guid companyId, EmployeeParameters employeeParameters, bool trackChanges) =>
        //await FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges)
        //     .OrderBy(e => e.Name)
        //     .Skip((employeeParameters.PageNumber - 1) * employeeParameters.PageSize)//skip the first ((3 – 1) * 20) = 40 results,
        //     .Take(employeeParameters.PageSize)//then take the next 20 results and return them to the caller.
        //     .ToListAsync();

        ///pagedlist version of getemployeesasync method
        public async Task<PagedList<Employee>> GetEmployeesAsync
            (Guid companyId,EmployeeParameters employeeParameters, bool trackChanges)
        {
            //var employees = await FindByCondition(e => e.CompanyId.Equals(companyId),trackChanges)
            //.OrderBy(e => e.Name)
            
            //age filtering added 
            var employees = await FindByCondition
                (e => e.CompanyId.Equals(companyId) && 
                (e.Age>= employeeParameters.MinAge && e.Age <= employeeParameters.MaxAge), trackChanges)
            /* https://localhost:7165/api/companies/3d490a70-94ce-4d15-9494-5248280c2ce3/employees?minAge=32*/
            /*https://localhost:7165/api/companies/3d490a70-94ce-4d15-9494-5248280c2ce3/employees?minAge=22&maxAge=34*/
            /*https://localhost:7165/api/companies/3d490a70-94ce-4d15-9494-5248280c2ce3/employees?pageNumber=1&pageSize=3&minAge=22&maxAge=34*/
            //paging props skip/take, for larger data
            .Skip((employeeParameters.PageNumber - 1) * employeeParameters.PageSize)
            .Take(employeeParameters.PageSize)
            .ToListAsync();

            var count = await FindByCondition
                (e => e.CompanyId.Equals(companyId), trackChanges).CountAsync();//Even though we have an additional
                                                                                //call to the database with the CountAsync method,
                                                                                //this solution was tested upon millions of rows and
                                                                                //was much faster than the previous one.
            //for relatively smaller data
            //return PagedList<Employee>
            //.ToPagedList(employees, employeeParameters.PageNumber,employeeParameters.PageSize);

            return new PagedList<Employee>
                (employees, count,employeeParameters.PageNumber, employeeParameters.PageSize);
        }

        public async Task<Employee> GetEmployeeByIdAsync(Guid companyId, Guid id, bool trackChanges) =>
        await FindByCondition(e => e.CompanyId.Equals(companyId) && e.Id.Equals(id), trackChanges).SingleOrDefaultAsync();

        public void CreateEmployeeForCompany(Guid companyId, Employee employee)
        {
            employee.CompanyId = companyId;
            Create(employee);
        }

        public void DeleteEmployee(Employee employee) => Delete(employee);
    }
}

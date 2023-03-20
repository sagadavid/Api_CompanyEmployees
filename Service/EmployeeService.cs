using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.Extensions.Options;
using Service.Contracts;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    internal sealed class 
        EmployeeService : IEmployeeService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILoggerManager _logMan;
        private readonly IMapper _mapper;

        public 
            EmployeeService
                    (IRepositoryManager repositoryManager, 
                     ILoggerManager logMan,
                     IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _logMan = logMan;
            _mapper = mapper;
        }

        public async Task 
            <EmployeeDto >
                GetEmployeeByIdAsync
                    (Guid companyId, 
                     Guid id, 
                     bool trackChanges)
        {
            var company = 
                await _repositoryManager.CompanyRepo
                .GetCompanyByIdAsync(companyId, trackChanges);

            if (company == null) throw new CompanyNotFoundException(companyId);

            //select employee depending company id
            var employeeFromDb = 
                await _repositoryManager.EmployeeRepo
                .GetEmployeeByIdAsync(companyId, id, trackChanges);

            if (employeeFromDb == null) throw new EmployeeNotFoundException(id);

            //map each employee to enumetable dto
            var employeeDto = 
                _mapper.Map<EmployeeDto>(employeeFromDb);

            return employeeDto;

        }

        public async Task
            <IEnumerable<EmployeeDto> >
                GetEmployeesAsync
                    (Guid companyId, 
                    bool trackChanges)
        {
            var company = 
                await _repositoryManager.CompanyRepo
                .GetCompanyByIdAsync(companyId, trackChanges);

            if (company == null) throw new CompanyNotFoundException(companyId);
            
            //select employees depending company id
                var employeesFromDb = 
                        await _repositoryManager.EmployeeRepo
                        .GetEmployeesAsync(companyId, trackChanges);
                //map each employee to enumetable dto
                var employeesDto = 
                        _mapper.Map<IEnumerable<EmployeeDto>>(employeesFromDb);
                
                return employeesDto;
                    

        }

        public  async Task
            <EmployeeDto>
            CreateEmployeeForCompanyAsync
                    (Guid companyId, 
                    EmployeeForCreationDto employeeForCreation, 
                    bool trackChanges)
        {
            var company = 
                await _repositoryManager.CompanyRepo
                .GetCompanyByIdAsync(companyId, trackChanges);
            
            if (company is null)
                throw new CompanyNotFoundException(companyId);
            var employeeEntity = 
                _mapper.Map<Employee>(employeeForCreation);
            
            _repositoryManager.EmployeeRepo
                   .CreateEmployeeForCompany(companyId, employeeEntity);
            
            await _repositoryManager.SaveAsync();
            var employeeToReturn = 
                _mapper.Map<EmployeeDto>(employeeEntity);
            
            return employeeToReturn;
        }

        public async Task 
            DeleteEmployeeForCompanyAsync
                    (Guid companyId, 
                     Guid id, 
                     bool trackChanges)
        {
            var company =
                await _repositoryManager.CompanyRepo
                .GetCompanyByIdAsync(companyId, trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(companyId);
            ////If company exists, we fetch the employee for that company,
            ///that means, company is companyId actually
            
            var employeeForCompany = 
                await _repositoryManager.EmployeeRepo
                .GetEmployeeByIdAsync(companyId, id, trackChanges);

            ////company and employee id intersects, gives us employee for company
            if (employeeForCompany is null)
                throw new EmployeeNotFoundException(id);

            _repositoryManager.EmployeeRepo
                    .DeleteEmployee(employeeForCompany);
            await _repositoryManager.SaveAsync();
        }

        public async Task 
            UpdateEmployeeForCompanyAsync
                (Guid companyId, 
                Guid id, 
                EmployeeForUpdateDto employeeForUpdate,
                bool compTrackChanges, 
                bool empTrackChanges)
        {

            var company = await _repositoryManager.CompanyRepo
                          .GetCompanyByIdAsync(companyId, compTrackChanges);

            if (company is null) throw new CompanyNotFoundException(companyId);

            var employeeEntity = await _repositoryManager.EmployeeRepo
                                .GetEmployeeByIdAsync(companyId, id, empTrackChanges);

            if (employeeEntity is null) throw new EmployeeNotFoundException(id);

            _mapper.Map(employeeForUpdate, employeeEntity);

            await _repositoryManager.SaveAsync();//we are mapping from the employeeForUpdate object
                                      //(we will change just the age property in a request) to the
                                      //employeeEntity — thus changing the state of the
                                      //employeeEntity object to Modified.
                                      //Because our entity has a modified state, it is enough to
                                      //call the Save method without any additional update
                                      //actions.As soon as we call the Save method, our entity
                                      //is going to be updated in the database.

        }

        public async Task
            <(EmployeeForUpdateDto employeeToPatch, 
            Employee employeeEntity)> 
                    GetEmployeeForPatchAsync
                            (Guid companyId, 
                             Guid id, 
                             bool compTrackChanges, 
                             bool empTrackChanges)
        {
            var company = 
                await _repositoryManager.CompanyRepo
                .GetCompanyByIdAsync(companyId, compTrackChanges);

            if (company is null)
                throw new CompanyNotFoundException(companyId);

            var employeeEntity = 
                await _repositoryManager.EmployeeRepo
                .GetEmployeeByIdAsync(companyId, id, empTrackChanges);

            if (employeeEntity is null)
                throw new EmployeeNotFoundException(companyId);

            var employeeToPatch = 
                _mapper.Map<EmployeeForUpdateDto>(employeeEntity);

            return (employeeToPatch, employeeEntity);//return both objects
                                                     //(employeeToPatch and employeeEntity )
                                                     //inside the Tuple to the controller.

        }

        public async Task  
            SaveChangesForPatchAsync
                    (EmployeeForUpdateDto employeeToPatch, 
                     Employee employeeEntity)
        {
            _mapper.Map(employeeToPatch, employeeEntity);
            await _repositoryManager.SaveAsync();
        }


    }
}

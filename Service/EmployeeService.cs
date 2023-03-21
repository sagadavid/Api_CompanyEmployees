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
            <EmployeeDto>
                GetEmployeeByIdAsync
                    (Guid companyId,
                     Guid id,
                     bool trackChanges)
        {
            await CheckIfCompanyExists(companyId, trackChanges);

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
            <IEnumerable<EmployeeDto>>
                GetEmployeesAsync
                    (Guid companyId,
                    bool trackChanges)
        {
            await CheckIfCompanyExists(companyId, trackChanges);

            //select employees depending company id
            var employeesFromDb =
                    await _repositoryManager.EmployeeRepo
                    .GetEmployeesAsync(companyId, trackChanges);
            //map each employee to enumetable dto
            var employeesDto =
                    _mapper.Map<IEnumerable<EmployeeDto>>(employeesFromDb);

            return employeesDto;


        }

        public async Task
            <EmployeeDto>
            CreateEmployeeForCompanyAsync
                    (Guid companyId,
                    EmployeeForCreationDto employeeForCreation,
                    bool trackChanges)
        {
            await CheckIfCompanyExists(companyId, trackChanges);
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
            await CheckIfCompanyExists(companyId, trackChanges);

            var employeeForCompany =
                await GetEmployeeForCompanyAndCheckIfItExists
                        (companyId, id,trackChanges);

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

            await CheckIfCompanyExists(companyId, compTrackChanges);

            var employeeEntity = await GetEmployeeForCompanyAndCheckIfItExists
                        (companyId, id,empTrackChanges);

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
            await CheckIfCompanyExists(companyId, compTrackChanges);

            var employeeEntity =
                await GetEmployeeForCompanyAndCheckIfItExists(companyId, id, empTrackChanges);

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

        private async Task CheckIfCompanyExists(Guid companyId, bool trackChanges)
        {
            var company = await _repositoryManager.CompanyRepo
                .GetCompanyByIdAsync(companyId, trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(companyId);
        }

        private async Task<Employee> GetEmployeeForCompanyAndCheckIfItExists
            (Guid companyId, Guid id, bool trackChanges)
        {
            var employeeDb = await _repositoryManager.EmployeeRepo
                .GetEmployeeByIdAsync(companyId, id, trackChanges);
            if (employeeDb is null)
                throw new EmployeeNotFoundException(id);
            return employeeDb;
        }
        /*all of the methods are cleaner and easier to maintain since our 
         * validation code is in a single place, and if we need to modify these 
         * validations, there’s only one place we need to change.
         Additionally, if you want you can create a new class and extract these methods,
        register that class as a service, inject it into our service classes and use
        the validation methods.*/

    }
}

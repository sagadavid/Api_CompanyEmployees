using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Service.Contracts;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    internal class EmployeeService : IEmployeeService
    {
        private readonly IRepositoryManager _repoMan;
        private readonly ILoggerManager _logMan;
        private readonly IMapper _mapper;

        public EmployeeService(IRepositoryManager repoMan, ILoggerManager logMan,
            IMapper mapper)
        {
            _repoMan = repoMan;
            _logMan = logMan;
            _mapper = mapper;
        }

        public EmployeeDto GetEmployee(Guid companyId, Guid id, bool trackChanges)
        {
            var company = _repoMan.CompanyRepo.GetCompanyById(companyId, trackChanges);

            if (company == null) throw new CompanyNotFoundException(companyId);

            //select employee depending company id
            var employeeFromDb = _repoMan.EmployeeRepo.GetEmployee(companyId, id, trackChanges);
            if (employeeFromDb == null) throw new EmployeeNotFoundException(id);

            //map each employee to enumetable dto
            var employeeDto = _mapper.Map<EmployeeDto>(employeeFromDb);

            return employeeDto;

        }

        public IEnumerable<EmployeeDto> GetEmployees(Guid companyId, bool trackChanges)
        {
            var company = _repoMan.CompanyRepo.GetCompanyById(companyId, trackChanges);
            
            if (company == null) throw new CompanyNotFoundException(companyId);

                //select employees depending company id
                var employeesFromDb = _repoMan.EmployeeRepo.GetEmployees(companyId, trackChanges);
                //map each employee to enumetable dto
                var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesFromDb);

                return employeesDto;
                    

        }
    }
}

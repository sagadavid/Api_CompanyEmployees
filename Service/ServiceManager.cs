using AutoMapper;
using Contracts;
using Service.Contracts;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<ICompanyService> _companyService;
        private readonly Lazy<IEmployeeService> _employeeService;

        public ServiceManager(IRepositoryManager repoMan, ILoggerManager logMan, 
            IMapper mapper, IDataShaper<EmployeeDto> dataShaper)
        {
            _companyService = new Lazy<ICompanyService>(() => 
            new CompanyService(repoMan, logMan, mapper));

            _employeeService = new Lazy<IEmployeeService>(() => 
            new EmployeeService(repoMan, logMan, mapper, dataShaper));
        }

        public ICompanyService CompanyService => _companyService.Value;
        public IEmployeeService EmployeeService => _employeeService.Value;

    }
    /*with all these in place, we have to add the reference from
 * the Service project inside the main project. Since Service is 
 * already referencing Service.Contracts , our main project will have 
 * the same reference as well.
 * and... modify the ServiceExtensions class.. 
 * Then, all we have to do is to modify the Program class*/
}

using AutoMapper;
using Contracts;
using Entities.ConfigurationModels;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service
{
    public sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<ICompanyService> _companyService;
        private readonly Lazy<IEmployeeService> _employeeService;
        private readonly Lazy<IAuthenticationService> _authenticationService;

        public ServiceManager(IRepositoryManager repositoryManager,
            ILoggerManager logger,
            IMapper mapper,
            IDataShaper<EmployeeDto> dataShaper,
            UserManager<User> userManager,
            //IConfiguration configuration//replaced with ioptions version below
            //IOptions<JwtConfiguration> configuration//replaced with monitor version
            IOptionsMonitor<JwtConfiguration> configuration//extra notes about ioptions ligger nederst
            )
        {
            _companyService = new Lazy<ICompanyService>(() =>
                new CompanyService(repositoryManager, logger, mapper));
            _employeeService = new Lazy<IEmployeeService>(() =>
                new EmployeeService(repositoryManager, logger, mapper, dataShaper));
            _authenticationService = new Lazy<IAuthenticationService>(() =>
                new AuthenticationService(logger, mapper, userManager, configuration));
        }

        public ICompanyService CompanyService => _companyService.Value;
        public IEmployeeService EmployeeService => _employeeService.Value;
        public IAuthenticationService AuthenticationService => _authenticationService.Value;
    }

}

/* to enable jwtsettings value change without restarting 
           * the app (configurstion reloading),
           * All we would have to do is to replace the IOptions<JwtConfiguration> type 
           * with the IOptionsSnapshot<JwtConfiguration> or IOptionsMonitor<JwtConfiguration>
           * types inside the ServiceManager and AuthenticationService classes.
           * So the main difference between these two interfaces is that the 
           * IOptionsSnapshot service is registered as a scoped service and thus can’t
           * be injected inside the singleton service. On the other hand, 
           * IOptionsMonitor is registered as a singleton service and can be injected
           * into any service lifetime.
           * 
           * 
====> IOptions<T>:

• Is the original Options interface and it’s better than binding the whole Configuration

• Does not support configuration reloading

• Is registered as a singleton service and can be injected anywhere

• Binds the configuration values only once at the registration, 
and returns the same values every time

• Does not support named options 

======> IOptionsSnapshot<T>:

• Registered as a scoped service

• Supports configuration reloading

• Cannot be injected into singleton services

• Values reload per request

• Supports named options 

=====> IOptionsMonitor<T>:

• Registered as a singleton service

• Supports configuration reloading

• Can be injected into any service lifetime

• Values are cached and reloaded immediately

• Supports named options

 Having said that, we can see that if we don’t want to enable live reloading 
or we don’t need named options, we can simply use IOptions<T>.
If we do, we can use either IOptionsSnapshot<T> or IOptionsMonitor<T>,
but IOptionsMonitor<T> can be injected into other singleton services 
while IOptionsSnapshot<T> cannot.

         */
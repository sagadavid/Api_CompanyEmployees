using Contracts;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    internal class CompanyService : ICompanyService
    {
        //using IRepositoryManager to access the repository methods
        //from each user repository class (CompanyRepository or EmployeeRepository)
        private readonly IRepositoryManager _repoMan;
        private readonly ILoggerManager _logMan;

        public CompanyService(IRepositoryManager repoMan, ILoggerManager logMan) 
        {
            _logMan= logMan;
            _repoMan= repoMan;  
        }

    }
}

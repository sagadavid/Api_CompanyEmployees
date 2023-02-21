using Contracts;
using Service.Contracts;
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

        public EmployeeService(IRepositoryManager repoMan, ILoggerManager logMan)
        {
            _repoMan = repoMan;
            _logMan = logMan;
        }
    }
}

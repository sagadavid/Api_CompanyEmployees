using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IRepositoryManager
    {
        ICompanyRepository CompanyRepo { get; }
        IEmployeeRepository EmployeeRepo { get; }
        void Save();
    }
}

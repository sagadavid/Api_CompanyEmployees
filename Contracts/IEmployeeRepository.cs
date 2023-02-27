using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IEmployeeRepository
    {
        public IEnumerable<Employee> GetEmployees(Guid companyId, bool trackCahnges);

    }
}

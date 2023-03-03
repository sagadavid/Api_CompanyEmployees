using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects
{
    public record CompanyForUpdateDto
        (string Name, 
        string Address, 
        string Country,
        IEnumerable<EmployeeForUpdateDto> Employees);
    //so we pack employee inside dto.. no need to call it again while updating
    //just add employees in the json body in postman.. will be added while company is updated  
}

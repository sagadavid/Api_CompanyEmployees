using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects
{
    public record CompanyForCreationDto(string Name, string Address, string Country,
        IEnumerable<EmployeeForCreationDto> Employees2Create//added to create company with employee
        
        /*   postman post: https://localhost:7165/api/companies/
     {
         "name": "COMPANY+empoyeess created","address": "avenue 23, jondoeland","country":"Newland",
         "employees2create" : [
             {"name":"dravid nbola", "age":"22", "position":"praksisant"}, 
             {"name":"henriqe verge", "age":"23", "position":"praksisantee"}
             ]
     } */
        );
}

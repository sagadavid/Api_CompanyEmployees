using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

    ////VALIDATION WHILE CREATION IN INIT SETTER STYLE

    //public record CompanyForCreationDto
    //{
    //    [Required(ErrorMessage = "Name  is a required field.")]
    //    [MaxLength(30, ErrorMessage = "Maximum length is 30 characters.")]
    //    string Name { get; init; }

    //    [Required(ErrorMessage = "Address is a required field.")]
    //    [MaxLength(50, ErrorMessage = "Maximum length is 50 characters.")]
    //    string Address { get; init; }

    //    [Required(ErrorMessage = "Country is a required field.")]
    //    [MaxLength(20, ErrorMessage = "Maximum length 20 characters.")]
    //    string Country { get; init; }

    //    IEnumerable<EmployeeForCreationDto> Employees2Create { get; init; }
    //}


}

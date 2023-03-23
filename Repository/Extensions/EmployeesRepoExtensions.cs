using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Extensions
{
    public static class RepositoryEmployeeExtensions
    {
        public static IQueryable<Employee> FilterEmployees
            (this IQueryable<Employee> employees, uint minAge, uint maxAge) =>
            employees.Where(e => (e.Age >= minAge && e.Age <= maxAge));
        /*
* https://localhost:7165/api/companies/3d490a70-94ce-4d15-9494-5248280c2ce3/employees?pageNumber=2&pageSize=2&searchTerm=m
         headers/xpagination:
        {"CurrentPage":2,"TotalPages":4,"PageSize":2,"TotalCount":7,"HasPrevious":true,"HasNext":true}
         */

        public static IQueryable<Employee> SearchTerm
            (this IQueryable<Employee> employees, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm)) return employees;
            var lowerCaseTerm = searchTerm.Trim().ToLower();
            return employees.Where(e => e.Name.ToLower().Contains(lowerCaseTerm));
            
/* 
 * https://localhost:7165/api/companies/3d490a70-94ce-4d15-9494-5248280c2ce3/employees?searchTerm=ae 
 */
            
        }

    }
}

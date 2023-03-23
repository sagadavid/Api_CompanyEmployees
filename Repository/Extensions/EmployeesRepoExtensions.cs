using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;//package added for sorting
using System.Reflection;

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
        /* 
         https://localhost:5001/api/companies/companyId/employees?orderBy=name,age,desc 
        our orderByQueryString will be name,age desc .
        */
        public static IQueryable<Employee> Sort
            (this IQueryable<Employee> employees, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return employees.OrderBy(e => e.Name);
            var orderParams = orderByQueryString.Trim().Split(',');// to get the individual fields:
            var propertyInfos = typeof(Employee).GetProperties
                (BindingFlags.Public | BindingFlags.Instance);//excerpt/find property name 
            var orderQueryBuilder = new StringBuilder();//stringbuilder to build our query with each loop
            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))//find/check property name/existance in the Employee class
                    continue;
                var propertyFromQueryName = param.Split(" ")[0];
                var objectProperty = propertyInfos.FirstOrDefault(pInf =>
                pInf.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));
                if (objectProperty == null)
                    continue;
                var direction = param.EndsWith(" desc") ? "descending" : "ascending";
                orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {direction}, ");
}

            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');//removing excess commas
            if (string.IsNullOrWhiteSpace(orderQuery))
                return employees.OrderBy(e => e.Name);//one last check to see if our query indeed has something in it

            return employees.OrderBy(orderQuery);//order our query
            /*
            The standard LINQ query for this would be:
                    employees.OrderBy(e => e.Name).ThenByDescending(o => o.Age);
            This is a neat little trick to form a query when you don’t know in advance how you should sort.
             */
        }

    }
}

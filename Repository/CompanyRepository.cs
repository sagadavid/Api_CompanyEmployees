using Contracts;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class CompanyRepository:RepositoryBase<Company>, ICompanyRepository
    {
        public CompanyRepository(RepositoryContext repositoryContext):
            base(repositoryContext) {}

        public IEnumerable<Company> GetAllCompanies(bool trackChanges)=>
            FindAll(trackChanges)//we defined findall method in repositorybase class
            .OrderBy(c=>c.Name).ToList();
        
    }
}

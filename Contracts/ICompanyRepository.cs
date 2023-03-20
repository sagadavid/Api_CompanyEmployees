using Entities.Models;
using Shared.DataTransferObjects;

namespace Contracts
{
   
    public interface ICompanyRepository
    {
        //IEnumerable<Company> GetAllCompanies(bool trackChanges);
        //Company GetCompanyById(Guid companyId, bool trackChanges);
        //IEnumerable<Company> GetByIds(IEnumerable<Guid> ids, bool trackChanges);

        Task<IEnumerable<Company>> GetAllCompaniesAsync (bool trackChanges);
        Task<Company> GetCompanyByIdAsync (Guid companyId, bool trackChanges);
        Task<IEnumerable<Company>> GetByIdsAsync (IEnumerable<Guid> ids, bool trackChanges);

        //The Create and Delete method signatures are left synchronous.
        //That’s because, in these methods, we are not making any changes in the database.
        void CreateCompany(Company company);
        void DeleteCompany(Company company);
    }
}

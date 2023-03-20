using Entities.Models;
using Shared.DataTransferObjects;

namespace Contracts
{
    /*In asynchronous programming, we have three return types:
     * Task<TResult>, for an async method that returns a value.
     * Task, for an async method that does not return a value.
     * void, which we can use for an event handler.
     *      but we dont async in repobase class, to keep available for syncronous methods, sometimes async takes more time than sync*/

    public interface ICompanyRepository
    {
        //IEnumerable<Company> GetAllCompanies(bool trackChanges);
        //Company GetCompanyById(Guid companyId, bool trackChanges);
        //IEnumerable<Company> GetByIds(IEnumerable<Guid> ids, bool trackChanges);

        Task<IEnumerable<Company>> GetAllCompanies(bool trackChanges);
        Task<Company> GetCompanyById(Guid companyId, bool trackChanges);
        Task<IEnumerable<Company>> GetByIds(IEnumerable<Guid> ids, bool trackChanges);

        //The Create and Delete method signatures are left synchronous.
        //That’s because, in these methods, we are not making any changes in the database.
        void CreateCompany(Company company);
        void DeleteCompany(Company company);
    }
}

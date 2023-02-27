using Entities.Models;
using Shared.DataTransferObjects;

namespace Contracts
{
    public interface ICompanyRepository
    {
        IEnumerable<Company> GetAllCompanies(bool trackChanges);
        Company GetCompanyById(Guid companyId, bool trackChanges);
    }
}

using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface ICompanyService
    {//async modifications added
        Task <IEnumerable<CompanyDto>> GetAllCompaniesAsync(bool trackChanges);
        Task <CompanyDto> GetCompanyByIdAsync (Guid companyId, bool trackChanges);
        Task <CompanyDto> CreateCompanyAsync(CompanyForCreationDto company);
        Task <IEnumerable<CompanyDto>> GetByIdsAsync (IEnumerable<Guid> ids, bool trackChanges);
        
        Task <(IEnumerable<CompanyDto> companies, string ids)> CreateCompanyCollectionAsync 
                                          (IEnumerable<CompanyForCreationDto> companyCollection);
        //void DeleteCompany(Guid companyId, bool trackChanges);
        //void UpdateCompany (Guid companyid,CompanyForUpdateDto companyForUpdate,bool trackChanges);
        Task DeleteCompanyAsync (Guid companyId, bool trackChanges);
        Task UpdateCompanyAsync (Guid companyid, CompanyForUpdateDto companyForUpdate, bool trackChanges);

    }
}

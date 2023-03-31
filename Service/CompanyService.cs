using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Entities.Response;
using Service.Contracts;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    internal sealed class CompanyService : ICompanyService
    {
        ////using IRepositoryManager to access the repository methods
        ////from each user repository class (CompanyRepository or EmployeeRepository)
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILoggerManager _logManager;
        private readonly IMapper _mapper;

        public CompanyService(IRepositoryManager repoMan, ILoggerManager logMan,
            IMapper mapper)
        {
            _logManager = logMan;
            _repositoryManager = repoMan;
            _mapper = mapper;
        }
        /* commented due to response performance improvement
        public async Task<CompanyDto> CreateCompanyAsync(CompanyForCreationDto company)

        {
            //method input parameter, to be mapped, created, saved, returned
            var companyEntity = _mapper.Map<Company>(company);

            _repositoryManager.CompanyRepo.CreateCompany(companyEntity);

            await _repositoryManager.SaveAsync();

            var companyToReturn = _mapper.Map<CompanyDto>(companyEntity);

            return companyToReturn;

        }

        public async Task<CompanyDto> GetCompanyByIdAsync(Guid id, bool trackChanges)
        {
            var company = await GetCompanyAndCheckIfItExists(id, trackChanges);
            //we have company but return type is companydto, map company for dto
            var companyDto = _mapper.Map<CompanyDto>(company);
            return companyDto;
        }

        public async Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(bool trackChanges)
        {
            //no need for try-catch, after error hanler middleware added/

            var companies = await _repositoryManager.CompanyRepo.GetAllCompaniesAsync(trackChanges);
            ////instead of manual mapping as above, use imapper, below
            var companiesDTO = _mapper.Map<IEnumerable<CompanyDto>>(companies);
            return companiesDTO;

        }

        public async Task<IEnumerable<CompanyDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
        {
            if (ids is null)
                throw new IdParametersBadRequestException();
            var companyEntities = await _repositoryManager.CompanyRepo.GetByIdsAsync(ids, trackChanges);
            if (ids.Count() != companyEntities.Count())
                throw new CollectionByIdsBadRequestException();
            var companiesToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
            return companiesToReturn;
        }

        public async Task<(IEnumerable<CompanyDto> companies, string ids)>
            CreateCompanyCollectionAsync(IEnumerable<CompanyForCreationDto> companyCollection)
        {
            if (companyCollection is null)
                throw new CompanyCollectionBadRequest();
            var companyEntities =
                   _mapper.Map<IEnumerable<Company>>(companyCollection);
            foreach (var company in companyEntities)
            { _repositoryManager.CompanyRepo.CreateCompany(company); }
            await _repositoryManager.SaveAsync();

            var companyCollectionToReturn =
                    _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
            var ids = string.Join(",", companyCollectionToReturn.Select(c => c.Id));
            return (companies: companyCollectionToReturn, ids: ids);
        }

        public async Task DeleteCompanyAsync(Guid companyId, bool trackChanges)
        {
            var company = await GetCompanyAndCheckIfItExists(companyId, trackChanges);
            _repositoryManager.CompanyRepo.DeleteCompany(company);
            await _repositoryManager.SaveAsync();
        }

        public async Task UpdateCompanyAsync
            (Guid companyId, CompanyForUpdateDto companyForUpdate, bool trackChanges)
        {
            var company = await GetCompanyAndCheckIfItExists(companyId, trackChanges);
             _mapper.Map(companyForUpdate, company);
            await _repositoryManager.SaveAsync();

        }



        //repeated code lines turns into a new method and replaces lines of codes
        private async Task<Company> GetCompanyAndCheckIfItExists(Guid id, bool trackChanges)
        {
            var company = await _repositoryManager.CompanyRepo
                .GetCompanyByIdAsync(id, trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(id);
            return company;
        } 
        */

        public ApiBaseResponse GetAllCompanies(bool trackChanges)
        {
            var companies = _repositoryManager.CompanyRepo.GetAllCompaniesAsync(trackChanges);
            var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);
            return new ApiOkResponse<IEnumerable<CompanyDto>>(companiesDto);
        }

        public ApiBaseResponse GetCompany(Guid id, bool trackChanges)
        {
            var company = _repositoryManager.CompanyRepo.GetCompanyByIdAsync(id, trackChanges);
            if (company is null)
                return new CompanyNotFoundResponse(id);
            var companyDto = _mapper.Map<CompanyDto>(company);
            return new ApiOkResponse<CompanyDto>(companyDto);
        }
    }
}

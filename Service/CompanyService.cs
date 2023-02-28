using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
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
        //using IRepositoryManager to access the repository methods
        //from each user repository class (CompanyRepository or EmployeeRepository)
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILoggerManager _logManager;
        private readonly IMapper _mapper;

        public CompanyService(IRepositoryManager repoMan, ILoggerManager logMan,
            IMapper mapper) 
        {
            _logManager= logMan;
            _repositoryManager= repoMan;
            _mapper= mapper;
        }

        public CompanyDto CreateCompany(CompanyForCreationDto company)

        {
            //method input parameter, to be mapped, created, saved, returned
            var companyEntity = _mapper.Map<Company>(company);

            _repositoryManager.CompanyRepo.CreateCompany(companyEntity);

            _repositoryManager.Save();

            var companyToReturn = _mapper.Map<CompanyDto>(companyEntity);

            return companyToReturn;

        }

        public CompanyDto GetCompanyById(Guid id, bool trackChanges)
        {
            var company = _repositoryManager.CompanyRepo.GetCompanyById(id, trackChanges);
            //null check here
            if (company is null) 
                throw new CompanyNotFoundException(id);

            //we have company but return type is companydto, map company for dto
           var companyDto = _mapper.Map<CompanyDto>(company);
            return companyDto;
        }

        IEnumerable<CompanyDto> ICompanyService.GetAllCompanies(bool trackChanges)
        {
           /*no need for try-catch, after error hanler middleware added*/
           // try
            //{
                var companies = _repositoryManager.CompanyRepo.GetAllCompanies(trackChanges);
                
                ////var companiesDto = companies.Select(c =>
                ////new CompanyDTO(c.Id, c.Name ?? "", string.Join(' ', c.Address, c.Country)
                ////)).ToList();

                ////instead of manual mapping as above, use imapper, below
                var companiesDTO=_mapper.Map<IEnumerable<CompanyDto>>(companies);

                return companiesDTO;
            //}
            //catch (Exception ex)
            //{

            //    _logManager.LogError($"noe er feil i {nameof(ICompanyService.GetAllCompanies)} " +
            //        $"service method: {ex}");
            //    throw;
            //}
        }
    }
}

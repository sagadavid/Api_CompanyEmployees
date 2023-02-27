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
        private readonly IRepositoryManager _repoMan;
        private readonly ILoggerManager _logMan;
        private readonly IMapper _mapper;

        public CompanyService(IRepositoryManager repoMan, ILoggerManager logMan,
            IMapper mapper) 
        {
            _logMan= logMan;
            _repoMan= repoMan;
            _mapper= mapper;
        }

        public CompanyDTO GetCompanyById(Guid id, bool trackChanges)
        {
            var company = _repoMan.CompanyRepo.GetCompanyById(id, trackChanges);
            //null check here
            if (company is null) 
                throw new CompanyNotFoundException(id);

            //we have company but return type is companydto, map company for dto
           var companyDto = _mapper.Map<CompanyDTO>(company);
            return companyDto;
        }

        IEnumerable<CompanyDTO> ICompanyService.GetAllCompanies(bool trackChanges)
        {
           /*no need for try-catch, after error hanler middleware added*/
           // try
            //{
                var companies = _repoMan.CompanyRepo.GetAllCompanies(trackChanges);
                
                ////var companiesDto = companies.Select(c =>
                ////new CompanyDTO(c.Id, c.Name ?? "", string.Join(' ', c.Address, c.Country)
                ////)).ToList();

                ////instead of manual mapping as above, use imapper, below
                var companiesDTO=_mapper.Map<IEnumerable<CompanyDTO>>(companies);

                return companiesDTO;
            //}
            //catch (Exception ex)
            //{

            //    _logMan.LogError($"noe er feil i {nameof(ICompanyService.GetAllCompanies)} " +
            //        $"service method: {ex}");
            //    throw;
            //}
        }
    }
}

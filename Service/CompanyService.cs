using Contracts;
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

        public CompanyService(IRepositoryManager repoMan, ILoggerManager logMan) 
        {
            _logMan= logMan;
            _repoMan= repoMan;  
        }

        IEnumerable<CompanyDTO> ICompanyService.GetAllCompanies(bool trackChanges)
        {
            try
            {
                var companies = _repoMan.ICompanyRepo.GetAllCompanies(trackChanges);
                var companiesDto = companies.Select(c =>
                new CompanyDTO(c.Id, c.Name ?? "", string.Join(' ', c.Address, c.Country)
                )).ToList();
                return companiesDto;
            }
            catch (Exception ex)
            {

                _logMan.LogError($"noe er feil i {nameof(ICompanyService.GetAllCompanies)} " +
                    $"service method: {ex}");
                throw;
            }
        }
    }
}

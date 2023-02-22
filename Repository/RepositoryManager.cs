using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*we are going to create a repository manager class, 
 * which will create instances of repository user classes for us and 
 * then register them inside the dependency injection container. After that,
 * we can inject it inside our services with constructor injection 
 * (supported by ASP.NET Core). With the repository manager class in place, 
 * we may call any repository user class we need.
 * But we are also missing one important part. We have the Create , Update , and Delete 
 * methods in the RepositoryBase class, but they won’t make any change
 * in the database until we call the SaveChanges method. 
 * Our repository manager class will handle that as well.*/
namespace Repository
{
    //sealed: When applied to a class,the sealed modifier prevents other classes from inheriting from it.
    public sealed class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext _repositoryContext;
//Lazy initialization of an object means that its creation is deferred until it is first used. 
        /*we are leveraging the power of the Lazy class to ensure the lazy initialization
         * of our repositories. This means that our repository instances are only going 
         * to be created when we access them for the first time, and not before that.*/
        private readonly Lazy<ICompanyRepository> _companyRepository;
        private readonly Lazy<IEmployeeRepository> _employeeRepostitory;

        public RepositoryManager(RepositoryContext repositoryContext) 
        {
            _repositoryContext = repositoryContext;
            _companyRepository = new Lazy<ICompanyRepository>(() =>
            new CompanyRepository(repositoryContext));
            _employeeRepostitory = new Lazy<IEmployeeRepository>(() =>
            new EmployeeRepository(repositoryContext));
        }

        /*As you can see, we are creating properties that will expose 
         * the concrete repositories and also we have the Save() method 
         * to be used after all the modifications are finished on a certain object. 
         * This is a good practice because now we can, for example, add two companies, 
         * modify two employees, and delete one company — all in one action — and then 
         * just call the Save method once. All the changes will be applied or if something
         * fails, all the changes will be reverted:
            _repository.ICompanyRepo.Create(company);
            _repository.ICompanyRepo.Create(anotherCompany);
            _repository.IEmployeeRepo.Update(employee);
            _repository.IEmployeeRepo.Update(anotherEmployee);
         */
        public ICompanyRepository ICompanyRepo => _companyRepository.Value;

        public IEmployeeRepository IEmployeeRepo => _employeeRepostitory.Value;

        public void Save()=> _repositoryContext.SaveChanges();
    }
}

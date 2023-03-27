using Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;//We don’t have to install the library in the Repository project since we already did that in the Entities project, and Repository has the reference to Entities .
using Microsoft.EntityFrameworkCore;
using Repository.Configuration;

namespace Repository
{
    public class RepositoryContext:IdentityDbContext<User>
    {
        public RepositoryContext(DbContextOptions options)
            :base(options) 
        { 
        }

        //seed data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);//required for migration to work properly.

            //invoke ientitytypeconfiguration implemented classes to seed data/apply configuration
            modelBuilder.ApplyConfiguration(new CompanyConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
        }


        public DbSet<Company>? Companies { get; set; }
        public DbSet<Employee>? Employees { get; set; }



    }
}
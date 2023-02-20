using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Configuration;

namespace Repository
{
    public class RepositoryContext:DbContext
    {
        public RepositoryContext(DbContextOptions options)
            :base(options) 
        { 
        }

        //seed data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
    //invoke ientitytypeconfiguration implemented classes to seed data/apply configuration
            modelBuilder.ApplyConfiguration(new CompanyConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
        }


        public DbSet<Company>? Companies { get; set; }
        public DbSet<Employee>? Employees { get; set; }



    }
}
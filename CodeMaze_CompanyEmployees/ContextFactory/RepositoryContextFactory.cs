using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Repository;

namespace CodeMaze_CompanyEmployees.ContextFactory
{
    //Since our RepositoryContext class is in a Repository project
    //and not in the main one, this class will help our application
    //create a derived DbContext instance during the design time which
    //will help us with our migrations:
    public class RepositoryContextFactory : 
        IDesignTimeDbContextFactory<RepositoryContext>
    {
        //implementtion of idesigntimedbcontextfactory
        public RepositoryContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()

.SetBasePath(Directory.GetCurrentDirectory())

.AddJsonFile("appsettings.json")

.Build();

            var builder = new DbContextOptionsBuilder<RepositoryContext>()

            .UseSqlServer(configuration.GetConnectionString("sqlConnection"));

            return new RepositoryContext(builder.Options);
        }
    }
}

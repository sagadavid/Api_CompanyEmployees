using Microsoft.EntityFrameworkCore;//contains Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore.Design;//necessary
using Repository;//necessary

namespace CodeMaze_CompanyEmployees.ContextFactory
{
    //Since our RepositoryContext class is in a Repository project
    //and not in the main one,
    //this contxtfactory class will help our application
    //create a derived DbContext instance during the design time which
    //will help us with our migrations:
    public class RepositoryContextFactory : 
        IDesignTimeDbContextFactory<RepositoryContext>//We are using the IDesignTimeDbContextFactory<out TContext> interface that allows design-time services to discover implementations of this interface. Of course, the TContext parameter is our RepositoryContext class.
    {
        public RepositoryContext CreateDbContext(string[] args)//Creates a new instance of a derived context.
        {
            var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
            var builder = new DbContextOptionsBuilder<RepositoryContext>()
            .UseSqlServer(configuration.GetConnectionString("sqlConnection"),
            b => b.MigrationsAssembly("CodeMaze_CompanyEmployees"));//migrationassembly is the main projects name
                                                                    //otherwise mentioned, sucks by the belowed error
            
            //PM> add-migration dbcreation -->
            //System.IO.FileNotFoundException: Could not load file or assembly
            //'CompanyEmployeesDB, Culture=neutral, PublicKeyToken=null'.
            //The system cannot find the file specified.

            //We have to make this change because migration assembly is
            //not in our main project, but in the Repository project.
            //So, we’ve just changed the project for the migration assembly.
            //b4 migration, necesary, package Microsoft.EntityFrameworkCore.Tools
            
            return new RepositoryContext(builder.Options);
        }
    }
}

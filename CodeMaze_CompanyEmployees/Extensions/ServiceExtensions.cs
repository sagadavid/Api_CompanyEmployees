using Contracts;//necessary to mention
using LoggerService;//necessary to mention
using Repository;
using Service.Contracts;
using Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc;

namespace CodeMaze_CompanyEmployees.Extensions
{
    public static class ServiceExtensions
    {
     //ADD CORS
        public static void ConfigureCors(this IServiceCollection services)=> 
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>builder
                //.WithOrigins("https://someweb.com").WithMethods("GET").WithHeaders("accept")
                .AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
                .WithExposedHeaders("X-Pagination"));//to enable the client application to read the new X-Pagination
                                                     //header that we’ve added in our action,
                                                     //we have to modify the CORS configuration

                
            });
        public static void ConfigureIISIntegration(this IServiceCollection services) =>
            services.Configure<IISOptions>(options =>
            {
                //options.AutomaticAuthentication = true;
                //options.AuthenticationDisplayName = null;
                //options.ForwardClientCertificate = true;
            });

        public static void ConfigureLoggerService(this IServiceCollection services) =>
         services.AddSingleton<ILoggerManager, LoggerManager>();
        //result : C:\Users\SAGAWIN\source\repos\sagadavid\Api_CompanyEmployees\CodeMaze_CompanyEmployees\bin\Debug\net6.0\logs
        /*  2023-02-07 15:36:00.2264 INFO Here is info message from our values controller.
            2023-02-07 15:36:00.2739 DEBUG Here is debug message from our values controller.
            2023-02-07 15:36:00.2739 WARN Here is warn message from our values controller.
            2023-02-07 15:36:00.2739 ERROR Here is an error message from our values controller.
            2023-02-07 15:59:39.3972 INFO Here is info message from our values controller.
            2023-02-07 15:59:39.4272 DEBUG Here is debug message from our values controller.
            2023-02-07 15:59:39.4413 WARN Here is warn message from our values controller.
            2023-02-07 15:59:39.4413 ERROR Here is an error message from our values controller.*/
        //https://localhost:7165/weatherforecast
        //["value1","value2"]

        /*after repository manager and modifying service extension, 
         * The repository layer is prepared and ready to be used to 
         * fetch data from the database.*/
        public static void ConfigureRepositoryManager(this IServiceCollection services) =>
            services.AddScoped<IRepositoryManager, RepositoryManager>();

        public static void ConfigureServiceManager(this IServiceCollection services) =>
            services.AddScoped<IServiceManager, ServiceManager>();

        /*as you could see, we have the RepositoryManager service registration, 
         * which happens at runtime, and during that registration, 
         * we must have RepositoryContext registered as well in the runtime, 
         * so we could inject it into other services (like RepositoryManager service). 
         * This might be a bit confusing, so let’s see what that means for us. 
         * SO.. modify the ServiceExtensions class:
         * We are not specifying the MigrationAssembly inside the UseSqlServer method. 
         * We don’t need it in this case.
         * and.. call this method in the Program class*/
        public static void ConfigureSqlContext(this IServiceCollection services, 
            IConfiguration configuration) =>
                services.AddDbContext<RepositoryContext>(opts =>
                    opts.UseSqlServer(configuration.GetConnectionString("sqlConnection")));

     //to get custom formatte response...formatcsv()
        public static IMvcBuilder AddCustomCSVFormatter(this IMvcBuilder builder) =>
            builder.AddMvcOptions(config => config.OutputFormatters.Add(new
            CsvOutputFormatter()));

        //registering custom media type for hateoas
        public static void AddCustomMediaTypes(this IServiceCollection services)
        {
            services.Configure<MvcOptions>(config =>
            {
                var systemTextJsonOutputFormatter = config.OutputFormatters
                .OfType<SystemTextJsonOutputFormatter>()?.FirstOrDefault();
                if (systemTextJsonOutputFormatter != null)
                {
                    systemTextJsonOutputFormatter.SupportedMediaTypes
                    .Add("application/vnd.codemaze.hateoas+json");
                    /* • vnd – vendor prefix; it’s always there.
                     * • codemaze – vendor identifier; we’ve chosen codemaze, because why not?
                     * • hateoas – media type name.
                     * • json – suffix; we can use it to describe if we want json or an XML response, 
                     * for example. */
                }

                var xmlOutputFormatter = config.OutputFormatters
                .OfType<XmlDataContractSerializerOutputFormatter>()?
                .FirstOrDefault();
                if (xmlOutputFormatter != null)
                {
                    xmlOutputFormatter.SupportedMediaTypes
                    .Add("application/vnd.codemaze.hateoas+xml");
                }//We are registering two new custom media types for the JSON and XML output formatters.
                 //This ensures we don’t get a 406 Not Acceptable response.
            });

        }
    }
}

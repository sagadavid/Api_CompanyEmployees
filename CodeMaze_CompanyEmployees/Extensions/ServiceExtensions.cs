using System.Runtime.CompilerServices;

namespace CodeMaze_CompanyEmployees.Extensions
{
    public static class ServiceExtensions
    {
     //ADD CORS
        public static void ConfigureCors(this IServiceCollection services)
            => services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                builder
                //.WithOrigins("https://someweb.com")
                .AllowAnyOrigin()
                //.WithMethods("GET")
                .AllowAnyMethod()
                //.WithHeaders("accept")
                .AllowAnyHeader());
                });
        public static void ConfigureIISIntegration(this IServiceCollection services) =>
            services.Configure<IISOptions>(options =>
            { 
            
            });
        
    }
}

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
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
                });
        
    }
}

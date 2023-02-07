using Contracts;//necessary to mention
using LoggerService;//necessary to mention

namespace CodeMaze_CompanyEmployees.Extensions
{
    public static class ServiceExtensions
    {
     //ADD CORS
        public static void ConfigureCors
            (this IServiceCollection services)
            => services.AddCors(options =>
            {
                options.AddPolicy
                ("CorsPolicy", builder =>
                builder
                //.WithOrigins("https://someweb.com")
                .AllowAnyOrigin()
                //.WithMethods("GET")
                .AllowAnyMethod()
                //.WithHeaders("accept")
                .AllowAnyHeader());
                });
        public static void ConfigureIISIntegration
            (this IServiceCollection services) =>
            services.Configure<IISOptions>(options =>
            {
                //options.AutomaticAuthentication = true;
                //options.AuthenticationDisplayName = null;
                //options.ForwardClientCertificate = true;
            });

        public static void ConfigureLoggerService
            (this IServiceCollection services) =>
         services.AddSingleton<ILoggerManager, LoggerManager>();
        //result : C:\Users\SAGAWIN\source\repos\sagadavid\Api_CompanyEmployees\CodeMaze_CompanyEmployees\bin\Debug\net6.0\logs
        /*2023-02-07 15:36:00.2264 INFO Here is info message from our values controller.
2023-02-07 15:36:00.2739 DEBUG Here is debug message from our values controller.
2023-02-07 15:36:00.2739 WARN Here is warn message from our values controller.
2023-02-07 15:36:00.2739 ERROR Here is an error message from our values controller.
2023-02-07 15:59:39.3972 INFO Here is info message from our values controller.
2023-02-07 15:59:39.4272 DEBUG Here is debug message from our values controller.
2023-02-07 15:59:39.4413 WARN Here is warn message from our values controller.
2023-02-07 15:59:39.4413 ERROR Here is an error message from our values controller.*/
        //https://localhost:7165/weatherforecast
        //["value1","value2"]
    }
}

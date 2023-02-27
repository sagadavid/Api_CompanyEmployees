using CodeMaze_CompanyEmployees.Extensions;
using Contracts;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NLog;
using Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//configure logger service for logging messages
LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(),"/nlog.config"));

//developer extensions added
builder.Services.ConfigureCors();

builder.Services.ConfigureIISIntegration();

builder.Services.ConfigureLoggerService();
//builder.Services.AddSingleton<ILoggerManager, LoggerManager>();//instead

builder.Services.ConfigureRepositoryManager();

builder.Services.ConfigureServiceManager();

builder.Services.ConfigureSqlContext(builder.Configuration);

/*without modifying addcontrollers, our API wouldn’t work
     * -because we deleted controller folder and files and presented presentation project-, 
     * and wouldn’t know where to route incoming requests. 
     * But now, our app will find all 
     * of the controllers inside of the Presentation project and configure 
     * them with the framework. They are going to be treated the same as if 
     * they were defined conventionally.*/
builder.Services.AddControllers(config => {
    /*A server does not explicitly specify where it formats a response to JSON. 
     * But you can override it by changing configuration options through the Add Controllers method.
     we must tell a server to respect the Accept header. After that, we just add the AddXmlDataContractSerializerFormatters method to support XML formatters.*/
    config.RespectBrowserAcceptHeader = true;
}).AddXmlDataContractSerializerFormatters()
    .AddApplicationPart(typeof(CompanyEmployees.Presentation.AssemblyReference).Assembly);

builder.Services.AddAutoMapper(typeof(Program));


var app = builder.Build();

/*It is important to know that we have to extract the ILoggerManager service 
 * after the var app = builder.Build() code line because the Build method builds 
 * the WebApplication and registers all the services added with IOC.*/

var logger = app.Services.GetRequiredService<ILoggerManager>();
app.ConfigureExceptionHandler(logger);

// Configure the HTTP request pipeline.

//app.Environment.IsDevelopment() interferes with our error handler middleware.
//if (app.Environment.IsDevelopment())
//    app.UseDeveloperExceptionPage();
//else
//    app.UseHsts();

if (app.Environment.IsProduction())
    app.UseHsts();

app.UseHttpsRedirection();

app.UseStaticFiles();//enable static files for the request

app.UseForwardedHeaders
    (new ForwardedHeadersOptions
{ 
ForwardedHeaders=ForwardedHeaders.All//headers matching,
                    //request vs proxy
});

app.UseCors("CorsPolicy");//which we chose/defined

app.UseAuthorization();

//adds endpoints for controller actions without specifying any routes.
//so..by default, dont need to configure //app.UseRouting();
app.MapControllers();

//default route configuration for MVC
//app.UseRouting();
//app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

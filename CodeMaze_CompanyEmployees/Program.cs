using CodeMaze_CompanyEmployees.Extensions;
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
builder.Services.AddControllers()//allows registering controller
    .AddApplicationPart(typeof(CompanyEmployees.Presentation.AssemblyReference).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
    app.UseHsts();

app.UseHttpsRedirection();

app.UseStaticFiles();//enable static files for the request

app.UseForwardedHeaders
    (new ForwardedHeadersOptions
{ 
ForwardedHeaders=
ForwardedHeaders.All//headers matching,
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

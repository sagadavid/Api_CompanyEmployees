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

builder.Services.AddControllers();//allows registering controller

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

app.MapControllers();//adds endpoints from controller

app.Run();

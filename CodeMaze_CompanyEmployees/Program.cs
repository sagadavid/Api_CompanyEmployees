using CodeMaze_CompanyEmployees.Extensions;
using Contracts;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NLog;
using Microsoft.AspNetCore.Mvc.Formatters;
using Presentation.ActionFilters;
using Service.DataShaping;
using Shared.DataTransferObjects;
using AspNetCoreRateLimit;

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

builder.Services.Configure<ApiBehaviorOptions>(options =>
{options.SuppressModelStateInvalidFilter = true;});//enable custom responses,
                                                   //fx on createataction for post api

builder.Services.AddScoped<ValidationFilterAttribute>();//using Presentation.ActionFilters;

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
    config.ReturnHttpNotAcceptable = true;/*tells the server that if the client tries to negotiate for the media type the server doesn’t support, it should return the 406 Not Acceptable status code.
                                           * This will make our application more restrictive and force the API consumer to request only the types the server supports. The 406 status code is created for this purpose.*/
    
    config.InputFormatters.Insert(0, GetJsonPatchInputFormatter());//We are placing our JsonPatchInputFormatter at the index 0 in the InputFormatters list.
    config.CacheProfiles.Add("120SecondsDuration", new CacheProfile//enable cashing for multiple kinds of (for filters etc) attributes
    {Duration =120});
})
            .AddXmlDataContractSerializerFormatters()
            .AddCustomCSVFormatter()//to get custom formatted response.. formatcsv()... postman get header accept text/csv
            .AddApplicationPart(typeof(CompanyEmployees.Presentation.AssemblyReference).Assembly);

builder.Services.AddAutoMapper(typeof(Program));

NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter() =>
    new ServiceCollection().AddLogging().AddMvc()
    .AddNewtonsoftJson().Services.BuildServiceProvider()
    .GetRequiredService<IOptions<MvcOptions>>().Value.InputFormatters
    .OfType<NewtonsoftJsonPatchInputFormatter>().First();/*
                                                          * By adding a method like this in the Program class, we are creating a local function. 
                                                          * This function configures support for JSON Patch using Newtonsoft.
                                                          * Json while leaving the other formatters unchanged.*/

builder.Services.AddScoped<IDataShaper<EmployeeDto>, DataShaper<EmployeeDto>>();

builder.Services.ConfigureVersioning();//versioning after package og extenion method
builder.Services.ConfigureResponseCaching();//adding cash store
builder.Services.ConfigureHttpCacheHeaders();//for marvin.cash.header package

builder.Services.AddMemoryCache();//for aspnetcoretatelimit library to go on
builder.Services.ConfigureRateLimitingOptions();//after extension method... for aspnetcoretatelimit library to go on
builder.Services.AddHttpContextAccessor();//after extension mehtod.. for aspnetcoretatelimit library to go on

builder.Services.AddAuthentication();//calls extension
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJWT(builder.Configuration);//calls extension

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

app.UseIpRateLimiting();//after extension mehtod.. for aspnetcoretatelimit library to go on
app.UseCors("CorsPolicy");//which we chose/defined
app.UseResponseCaching();//adding cash store.. place just after cors
app.UseHttpCacheHeaders();//for marvin.cash.header package

app.UseAuthentication();
app.UseAuthorization();

//adds endpoints for controller actions without specifying any routes.
//so..by default, dont need to configure //app.UseRouting();
app.MapControllers();

//default route configuration for MVC
//app.UseRouting();
//app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

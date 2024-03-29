﻿using Contracts;//necessary to mention
using LoggerService;//necessary to mention
using Repository;
using Service.Contracts;
using Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using CompanyEmployees.Presentation.Controllers;
using Presentation.Controllers;
using Marvin.Cache.Headers;
using AspNetCoreRateLimit;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Entities.ConfigurationModels;
using Microsoft.OpenApi.Models;

namespace CodeMaze_CompanyEmployees.Extensions
{
    public static class ServiceExtensions
    {
        //ADD CORS
        public static void ConfigureCors(this IServiceCollection services) =>
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder => builder
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


        //custom types.. for root api and/or hateoas ...
        public static void AddCustomMediaTypes(this IServiceCollection services)
        {
            services.Configure<MvcOptions>(config =>
            {
                var systemTextJsonOutputFormatter = config.OutputFormatters
                        .OfType<SystemTextJsonOutputFormatter>()?.FirstOrDefault();

                if (systemTextJsonOutputFormatter != null)
                {
                    //systemTextJsonOutputFormatter.SupportedMediaTypes
                    //.Add("application/vnd.codemaze.hateoas+json");
                    systemTextJsonOutputFormatter.SupportedMediaTypes
                    .Add("application/vnd.codemaze.apiroot+json");
                }

                var xmlOutputFormatter = config.OutputFormatters
                        .OfType<XmlDataContractSerializerOutputFormatter>()?
                        .FirstOrDefault();

                if (xmlOutputFormatter != null)
                {
                    //xmlOutputFormatter.SupportedMediaTypes
                    //.Add("application/vnd.codemaze.hateoas+xml");
                    xmlOutputFormatter.SupportedMediaTypes
                    .Add("application/vnd.codemaze.apiroot+xml");
                }
            });
        }

        //
        public static void ConfigureVersioning(this IServiceCollection services)

        {

            services.AddApiVersioning(opt =>

            {

                opt.ReportApiVersions = true;//adds the API version to the response header.

                opt.AssumeDefaultVersionWhenUnspecified = true;

                opt.DefaultApiVersion = new ApiVersion(1, 0);//we configured versioning to use 1.0 as a default
                                                             //API version (opt.AssumeDefaultVersionWhenUnspecified = true;).
                                                             //Therefore, if a clientdoesn’t state the required version,
                                                             //our API will use

                opt.ApiVersionReader = new HeaderApiVersionReader("api-version");//versioning by http header
                opt.ApiVersionReader = new QueryStringApiVersionReader("api-version");//to support query string versioning 

                //If we have a lot of versions of a single controller,
                //we can assign these versions in the configuration instead
                //Now, we can remove the [ApiVersion] attribute from the controllers.
                opt.Conventions.Controller<CompaniesController>()
                        .HasApiVersion(new ApiVersion(1, 0));
                opt.Conventions.Controller<CompaniesV2Controller>()
                        .HasDeprecatedApiVersion(new ApiVersion(2, 0));

            });

        }

        //after cashing is enabled, a cash store is needed
        public static void ConfigureResponseCaching(this IServiceCollection services) =>
                services.AddResponseCaching();

        //Configuration for marvin.cashe.headers
        //globally configure expiration and validation
        public static void ConfigureHttpCacheHeaders(this IServiceCollection services) =>
        services.AddHttpCacheHeaders((expirationOpt) =>
        {
            expirationOpt.MaxAge = 65;
            expirationOpt.CacheLocation = CacheLocation.Private;
        },
        (validationOpt) =>
        {
            validationOpt.MustRevalidate = true;
        });

        //as part of aspnetcoreratelimit package to go 
        public static void ConfigureRateLimitingOptions(this IServiceCollection services)
        {
            var rateLimitRules = new List<RateLimitRule>
            {
                new RateLimitRule
                {
                    Endpoint = "*",
                    Limit = 30,
                    Period = "5m"
                }
            };

            services.Configure<IpRateLimitOptions>(opt => { opt.GeneralRules = rateLimitRules; });
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

        }

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentity<User, IdentityRole>(r =>
            {
                r.Password.RequireDigit = true;
                r.Password.RequireLowercase = false;
                r.Password.RequireUppercase = false;
                r.Password.RequireNonAlphanumeric = false;
                r.Password.RequiredLength = 10;
                r.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<RepositoryContext>()
            .AddDefaultTokenProviders();
        }

        //we are providing values for the issuer, the audience, and the secret key that
        //the server uses to generate the signature for JWT.

        public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
            //var jwtSettings = configuration.GetSection("JwtSettings");
            /* we extract the JwtSettings from the appsettings.json file and extract our environment 
             * variable (If you keep getting null for the secret key, try restarting the Visual Studio
             * or even your computer). */

            var jwtConfiguration = new JwtConfiguration();
            configuration.Bind(jwtConfiguration.Section, jwtConfiguration);

            var secretKey = Environment.GetEnvironmentVariable("EnvirKey");
            /* To create an environment variable, we have to open the cmd window as 
             * an administrator and type the following command:
             * setx SECRET "EnvirKey" /M
             * This is going to create a system environment variable with the name
             * EnvirKey and the value CodeMazeSecretKey. By using /M we specify that we want 
             * a system variable and not local. */

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    //ValidIssuer = jwtSettings["validIssuer"],
                    //ValidAudience = jwtSettings["validAudience"],

                    ValidIssuer = jwtConfiguration.ValidIssuer,
                    ValidAudience = jwtConfiguration.ValidAudience,

                    IssuerSigningKey = new SymmetricSecurityKey
                                            (Encoding.UTF32.GetBytes(secretKey))
                };//tokencreation and sign in encoding should be same.. otherwise cant use token in sign in
            });
        }

        //using IOptions
        public static void AddJwtConfiguration
            (this IServiceCollection services, IConfiguration configuration) =>
        services.Configure<JwtConfiguration>
            (configuration.GetSection("JwtSettings"));
        //services.Configure<JwtConfiguration>
        //    (configuration.GetSection("JwtSettings2"));//named options

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "davids Code Maze API",
                    Version = "v1",
                    Description = "CompanyEmployees API by CodeMaze/davidsaga",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "davidsaga",
                        Email = "epostsaga@gmail.com",
                        Url = new Uri("https://twitter.com/johndoe"),
                    },

                    License = new OpenApiLicense
                    {
                        Name = "CompanyEmployees API LICX",
                        Url = new Uri("https://example.com/license"),
                    }
                });

                s.SwaggerDoc("v2", new OpenApiInfo
                {
                    Title = "davids Code Maze API",
                    Version = "v2",
                });

                //enable xml comments on swagger
                //var xmlFile = $"{typeof(Presentation.AssemblyReference).Assembly.GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //s.IncludeXmlComments(xmlPath);

                //to be able able to unlock swagger and use authorization etc properly
                s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Place to add JWT with Bearer",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                s.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            { Type = ReferenceType.SecurityScheme, Id = "Bearer"},
                            Name = "Bearer",
                        },
                        new List<string>()
                    }
                });
                
                //https://localhost:7165/swagger/v1/swagger.json
                //https://localhost:7165/swagger/v2/swagger.json
                //https://localhost:7165/swagger/index.html
                //pasting jwt token into swagger authorize:Bearer+" "+token
            });
        }
    }
}

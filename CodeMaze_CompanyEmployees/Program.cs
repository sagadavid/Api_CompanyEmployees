using CodeMaze_CompanyEmployees.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//extensions added
builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

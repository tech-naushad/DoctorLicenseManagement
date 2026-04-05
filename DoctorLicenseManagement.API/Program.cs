using DoctorLicenseManagement.Application.Interfaces;
using DoctorLicenseManagement.Application.Services;
using DoctorLicenseManagement.Infrastructure.Data;
using DoctorLicenseManagement.Infrastructure.Repositories;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<DoctorContext>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IDoctorService, DoctorService>();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1",
        Description = "My .NET 8 Web API"
    });
});


builder.Services.AddControllers();

var app = builder.Build();

// Enable Swagger only in Development (recommended)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
};

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

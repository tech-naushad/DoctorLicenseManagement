using DoctorLicenseManagement.API.Middlewares;
using DoctorLicenseManagement.Application;
using DoctorLicenseManagement.Infrastructure.Data;
using DoctorLicenseManagement.Infrastructure.Repositories;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IDbConnectionFactory, SqlConnectionFactory>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddApplicationServices();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Doctor License Management API",
        Version = "v1",
        Description = "Doctor License Management"
    });
});
// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowClientApp", builder =>
    {
        builder.WithOrigins("http://localhost:3000")  // Allow your React app
               .AllowAnyMethod()                       // GET, POST, PUT, DELETE, etc.
               .AllowAnyHeader()                       // Any headers
               .AllowCredentials();                    // Allow cookies/auth
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

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("AllowClientApp");

app.MapControllers();

app.Run();

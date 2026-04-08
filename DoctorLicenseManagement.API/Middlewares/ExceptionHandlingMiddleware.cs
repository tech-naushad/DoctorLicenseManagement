using DoctorLicenseManagement.Application.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DoctorLicenseManagement.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (FluentValidation.ValidationException ex)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";

                var errorMessages = ex.Errors
                    .Select(e => e.ErrorMessage)
                    .ToList();

                var response = new ApiResponse
                {
                    Success = false,
                    Message = "Validation Failed",
                    Error = string.Join(", ", errorMessages)  
                };

                await context.Response.WriteAsJsonAsync(response);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                var response = new ApiResponse
                {
                    Success = false,
                    Message = "Something went wrong",
                    Error = ex.Message  
                };

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}

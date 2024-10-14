using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using System.Threading.Tasks;
using BookStoreApi.Models;

namespace BookStoreApi.middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _env;
        private static readonly List<ErrorLog> _errorLogs = [];
        public ExceptionHandlerMiddleware(RequestDelegate next, IWebHostEnvironment env)
        {
            _next = next;
            _env = env;
        }
        
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                LogError(context, ex);
                await HandleExceptionAsync(context, ex);
            }
        }
        private void LogError(HttpContext context, Exception exception)
        {
            _errorLogs.Add(new ErrorLog
            {
                Path = context.Request.Path,
                Message = exception.Message,
                StackTrace = exception.StackTrace,
                Timestamp = DateTime.UtcNow
            });
        }
        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            string errorMessage;

            if (_env.IsDevelopment())
            {

            errorMessage = JsonSerializer.Serialize(new
            {
                statusCode = response.StatusCode,
                message = "An unexpected error occurred while processing your request.",
                details = exception.Message,
                stackTrace = exception.StackTrace
                });
            }
            else
            {
                errorMessage = JsonSerializer.Serialize(new 
                {
                    statusCode = response.StatusCode,
                    message = "An unexpected error occurred. Please try again later."
                });
            }
            return response.WriteAsync(errorMessage);
        }
        public static IEnumerable<ErrorLog> GetErrorLogs()
        {
            return _errorLogs;
        }
    }
}
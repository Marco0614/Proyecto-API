using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using WebApi.Models;
using WebApi.Data;
namespace WebApi.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, DbpruebaContext dbContext)
        {
            try
            {
                // Ejecutar la siguiente middleware en la cadena de solicitud
                await _next(context);
            }
            catch (LoginException ex)
            {
                // Capturar excepciones no controladas y manejarlas
                await HandleLoginExceptionAsync(context, dbContext, ex);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, dbContext, ex); //el dbContext se puede quitar si no se quiere almacenar el error en la bd
            }
            
        }

        private async Task HandleExceptionAsync(HttpContext context, DbpruebaContext dbContext, Exception exception)
        {
            // Registrar la excepción en el log
            _logger.LogError($"Unhandled exception: {exception}");

            // Registrar el error en la base de datos
            var errorLog = new ErrorLog
            {
                Controller = context.Request.Path,
                Action = context.Request.Method,
                Message = exception.Message,
                StackTrace = exception.StackTrace,
                Timestamp = DateTime.UtcNow
            };

            dbContext.ErrorLogs.Add(errorLog);
            await dbContext.SaveChangesAsync();

            // Configurar la respuesta de error para el cliente
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            // Escribir el mensaje de error en el cuerpo de la respuesta
            await context.Response.WriteAsync($"{{ \"error\": \"{exception.Message}\" }}");
        }

        private async Task HandleLoginExceptionAsync(HttpContext context, DbpruebaContext dbContext, LoginException exception)
        {
            // Registrar la excepción de inicio de sesión en el log
            _logger.LogError($"Login exception: {exception}");

            // Registrar el error de inicio de sesión en la base de datos
            var errorLog = new ErrorLog
            {
                Controller = context.Request.Path,
                Action = context.Request.Method,
                Message = exception.Message,
                StackTrace = exception.StackTrace,
                Timestamp = DateTime.UtcNow
            };

            dbContext.ErrorLogs.Add(errorLog);
            await dbContext.SaveChangesAsync();

            // Configurar la respuesta de error de inicio de sesión para el cliente
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";

            // Escribir el mensaje de error de inicio de sesión en el cuerpo de la respuesta
            await context.Response.WriteAsync($"{{ \"error\": \"Login error: {exception.Message}\" }}");
        }
    }

    public class LoginException : Exception
    {
        public LoginException(string message) : base(message)
        {
        }
    }



}


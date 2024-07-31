using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using WebApi.Interfaces;
using WebApi.Middleware;
using WebApi.Repositories;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Agrega servicios espec�ficos aqu� si es necesario
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Creaci�n de una instancia de la clase Startup
            var startup = new Startup(builder.Configuration);

            // Llamada al m�todo ConfigureServices de la clase Startup
            startup.ConfigureServices(builder.Services);

            // Construcci�n de la aplicaci�n
            var app = builder.Build();


            app.UseMiddleware<ErrorHandlingMiddleware>();

            // Llamada al m�todo Configure de la clase Startup
            startup.Configure(app, app.Environment);

            // Ejecuci�n de la aplicaci�n
            app.Run();
        }
    }
}

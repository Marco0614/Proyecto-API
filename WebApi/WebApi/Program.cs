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

            // Agrega servicios específicos aquí si es necesario
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Creación de una instancia de la clase Startup
            var startup = new Startup(builder.Configuration);

            // Llamada al método ConfigureServices de la clase Startup
            startup.ConfigureServices(builder.Services);

            // Construcción de la aplicación
            var app = builder.Build();


            app.UseMiddleware<ErrorHandlingMiddleware>();

            // Llamada al método Configure de la clase Startup
            startup.Configure(app, app.Environment);

            // Ejecución de la aplicación
            app.Run();
        }
    }
}

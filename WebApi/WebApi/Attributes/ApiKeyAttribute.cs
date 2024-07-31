using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using WebApi.Interfaces;

namespace WebApi.Attributes
{
    public class ApiKeyAttribute : IAsyncActionFilter
    {
        // Se crea la instancia de UnitOfWork
        private readonly IUnitOfWork _unitOfWork;

        // Se inyecta la dependencia del IUnitofWork
        public ApiKeyAttribute(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Método que se ejecuta antes de una acción del (método del controlador).
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Intenta obtener el valor del encabezado "ApiKey" de la solicitud HTTP.
            if (!context.HttpContext.Request.Headers.TryGetValue("ApiKey", out var extractedApiKey))
            {
                // Si no se encuentra el encabezado "ApiKey", se establece el resultado de la solicitud
                // con un código de estado 401 (No autorizado) y un mensaje de error.
                context.Result = new ContentResult()
                {
                    StatusCode = 401, // Código de estado HTTP 401: No autorizado
                    Content = "No se envió el Api Key" // Mensaje de error
                };
                return; // Termina la ejecución del método, no se continúa con la acción del controlador
            }

            // Se hace el llamado del del parametro "ApiKey"
           // var key = await _unitOfWork.Parametro.GetParametroByNombreAsync("ApiKey");

            // Verifica si el valor del encabezado "ApiKey" coincide con el valor esperado desde la BD.
          //  if (!extractedApiKey.Equals(key!.Valor))
            {
                // Si el ApiKey es incorrecto, se establece el resultado de la solicitud
                // con un código de estado 401 (No autorizado) y un mensaje de error.
                context.Result = new ContentResult()
                {
                    StatusCode = 401, // Código de estado HTTP 401: No autorizado
                    Content = "Api Key inválido" // Mensaje de error
                };
                return; // Termina la ejecución del método, no se continúa con la acción del controlador
            }

            // Si el ApiKey es válido, continúa con la ejecución de la acción del controlador.
            await next();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using ProyectoFE.DTOs;
using ProyectoFE.RestApi;

namespace ProyectoFE.Controllers
{
    public class Login : Controller
    {
        private readonly ApiRest _apiRest;
        

        public Login(ApiRest apiRest)
        {
            _apiRest = apiRest;
            
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<ActionResult> Loginasync(string correo, string clave)
        {
            
            var logeo = await _apiRest.LoginAsync(correo, clave);
            return View(logeo);
        }

        public async Task<ActionResult> RegistrarseAsync(string correo, string clave, string nombre)
        {
            var registrar = await _apiRest.register(nombre,correo,clave);
            return View(registrar);
        }


        
    }
}

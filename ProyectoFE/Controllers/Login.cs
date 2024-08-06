using Microsoft.AspNetCore.Mvc;
using ProyectoFE.DTOs;
using ProyectoFE.RestApi;

namespace ProyectoFE.Controllers
{
    public class Login : Controller
    {
        private readonly ApiRest _apiRest;
        private readonly LoginDTO _loginDTO;

        public Login(ApiRest apiRest, LoginDTO loginDTO)
        {
            _apiRest = apiRest;
            _loginDTO = loginDTO;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<ActionResult> Loginasync(string correo, string clave)
        {
            _loginDTO.Email = correo;
            _loginDTO.Password = clave;
            var logeo = await _apiRest.LoginAsync();
            return View(logeo);
        }

        public async Task<ActionResult> RegistrarseAsync(string correo, string clave, string nombre)
        {
            var registrar = await _apiRest.register(nombre,correo,clave);
            return View(registrar);
        }


        
    }
}

using Microsoft.AspNetCore.Mvc;
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


        
    }
}

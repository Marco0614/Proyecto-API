using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ProyectoFE.DTOs;
using RestSharp;


namespace ProyectoFE.RestApi
{
    public class ApiRest
    {
        private readonly string _url;
        
        private string _token;
        private DateTime _tokenExpirationTime = DateTime.MinValue;
        private readonly object _tokenLock = new object();

        public ApiRest(IOptions<ProyectoApiFE> options)
        {
            _url = options.Value.ApiBaseUrl;
            
        }

        private async Task<bool> AuthenticateAsync(string correo, string clave)
        {
            var cliente = new RestClient(_url);
            var request = new RestRequest("Acceso", Method.Post);
            request.AddJsonBody(new LoginDTO
            {
                Email = correo,
                Password = clave
            });

            var response = await cliente.ExecuteAsync<LoginDtoResponse>(request);

            if (response.IsSuccessful && response.Data.token != null) 
            {
                _token = response.Data.token;
                _tokenExpirationTime = DateTime.UtcNow.AddMinutes(5); // Establecer la nueva fecha de expiración del token
                return true;
            }
            return false;
        }

       

        private RestRequest AddAuthentication(RestRequest request, string correo, string clave)
        {
            // Asegurarse de que el token sea válido antes de añadirlo a la solicitud
            AuthenticateAsync(correo,clave).GetAwaiter().GetResult();

            if (_token != null)
            {
                request.AddHeader("Authorization", $"Bearer {_token}");
            }
            return request;
        }


        public async Task<bool> Login(LoginDTO loginDTO,string correo, string clave)
        {
            var cliente = new RestClient(_url);
            var request = new RestRequest("Acceso/Login", Method.Post);
            request.AddJsonBody(loginDTO);
            AddAuthentication(request,correo,clave);

            var response = await cliente.ExecuteAsync<List<LoginDTO>>(request);

            if (response.IsSuccessful) //&& response.Data != null)
            {
                return response.IsSuccessful;
            }
            throw new Exception(response.ErrorMessage);
        }

    }
}

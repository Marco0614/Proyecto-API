using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
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

        public async Task<bool> LoginAsync(string correo, string clave)
        {
            var cliente = new RestClient(_url);
            var request = new RestRequest("Acceso/Login", Method.Post);
            request.AddJsonBody(new LoginDTO
            {
                Email = correo,
                Password = clave
            });

            var response = await cliente.ExecuteAsync<LoginDtoResponse>(request);

            if (response.IsSuccessful && response.Data!= null)
            {
                _token = response.Data.token;
                _tokenExpirationTime = DateTime.UtcNow.AddMinutes(60); // Establecer la nueva fecha de expiración del token
                return true;
            }
            return false;
        }

        public async Task<bool> register(string nombre, string correo, string clave)
        {
            var cliente = new RestClient(_url);
            var request = new RestRequest("Acceso/Registrarse", Method.Post);
            request.AddJsonBody(new RegisterDTO
            {
                nombre = nombre,
                correo = correo,
                clave = clave
            }
            );

            var response = await cliente.ExecuteAsync<LoginDtoResponse>(request);

            if (response.IsSuccessful)
            {
                _token = response.Data!.token;
               
            }
            throw new Exception(response.ErrorMessage);

        }



        private RestRequest AddAuthentication(RestRequest request, string correo, string clave)
        {
            // Asegurarse de que el token sea válido antes de añadirlo a la solicitud
            LoginAsync(correo, clave).GetAwaiter().GetResult();

            if (_token != null)
            {
                request.AddHeader("Authorization", $"Bearer {_token}");
            }
            return request;
        }


        

        
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Newtonsoft.Json;
using ProyectoFE.Controllers;
using ProyectoFE.DTOs;
using RestSharp;


namespace ProyectoFE.RestApi
{
    public class ApiRest
    {
        private readonly string _url;
        private readonly LoginDTO _loginDTO;
        
        private string _token;
        private DateTime _tokenExpirationTime = DateTime.MinValue;
        private readonly object _tokenLock = new object();

        public ApiRest(IOptions<ProyectoApiFE> options, LoginDTO login)
        {
            _url = options.Value.ApiBaseUrl;
            _loginDTO = login;
            
        }
     

        public async Task<bool> LoginAsync()
        {
            var cliente = new RestClient(_url);
            var request = new RestRequest("Acceso/Login", Method.Post);
            request.AddJsonBody(new LoginDTO
            {
                Email = _loginDTO.Email,
                Password = _loginDTO.Password
            });

            var response = await cliente.ExecuteAsync<LoginDtoResponse>(request);

            if (response.IsSuccessful && response.Data != null)
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

            var response = await cliente.ExecuteAsync<RegisterDTO>(request);

            if (response.IsSuccessful)
            {
                return true;

            }
            throw new Exception(response.ErrorMessage);

        }



        private async Task<RestRequest> AddAuthentication(RestRequest request)
        {
            //Asegurarse de que el token sea válido antes de añadirlo a la solicitud
            if (_token == null || DateTime.UtcNow >= _tokenExpirationTime)
            {
                await LoginAsync();
            }

            if (_token != null)
            {
                request.AddHeader("Authorization", $"Bearer {_token}");
            }
            return request;
        }

        public async Task<List<InventarioDTO>> GetInventarioAsync()
        {
            var cliente = new RestClient(_url);
            var request = new RestRequest("Inventario/Lista", Method.Get);
            AddAuthentication(request);

            var response = await cliente.ExecuteAsync<List<InventarioDTO>>(request);
            if (response.IsSuccessful)
            {
                return response.Data!;
            }
            throw new Exception(response.ErrorMessage);

        }

        public async Task<InventarioDTO> GetInventarioIDAsync(int id)
        {
            var client = new RestClient(_url);
            var request = new RestRequest($"Inventario/{id}", Method.Get);
            AddAuthentication(request);

            var response = await client.ExecuteAsync<InventarioDTO>(request);
            if (response.IsSuccessful)
            {
                return response.Data!;
            }
            throw new Exception(response.ErrorMessage);

        }

        public async Task<bool> PutInventarioAsync(InventarioDTO inventarioDTO)
        {
            var client = new RestClient(_url);
            var request = new RestRequest("Inventario", Method.Put);
            request.AddJsonBody(inventarioDTO);
            AddAuthentication(request);

            var response = await client.ExecuteAsync<InventarioDTO>(request);

            return response.IsSuccessful;
            
            

        }

        public async Task<bool> PostInventarioAsync(InventarioDTO inventarioDTO)
        {
            var client = new RestClient(_url);
            var request = new RestRequest("Inventario", Method.Post);
            request.AddJsonBody(inventarioDTO);
            AddAuthentication(request);

            var response = await client.ExecuteAsync<InventarioDTO>(request);
            
            
            return response.IsSuccessful;
            
            

        }

        public async Task<bool> DeleteInventarioAsync(int id)
        {
            var client = new RestClient(_url);
            var request = new RestRequest($"Inventario/{id}", Method.Delete);
            AddAuthentication(request);

            var response = await client.ExecuteAsync<InventarioDTO>(request);
            return response.IsSuccessful;

        }

        //Productos


        public async Task<List<ProductosDTO>> GetProductosAsync()
        {
            var cliente = new RestClient(_url);
            var request = new RestRequest("producto", Method.Get);
            AddAuthentication(request);

            var response = await cliente.ExecuteAsync<List<ProductosDTO>>(request);
            if (response.IsSuccessful)
            {
                return response.Data!;
            }
            throw new Exception(response.ErrorMessage);

        }

        public async Task<ProductosDTO> GetProductosIDAsync(int id)
        {
            var client = new RestClient(_url);
            var request = new RestRequest($"producto/{id}", Method.Get);
            AddAuthentication(request);

            var response = await client.ExecuteAsync<ProductosDTO>(request);
            if (response.IsSuccessful)
            {
                return response.Data!;
            }
            throw new Exception(response.ErrorMessage);

        }

        public async Task<bool> PutProductosAsync(ProductosDTO productosDTO)
        {
            var client = new RestClient(_url);
            var request = new RestRequest("producto", Method.Put);
            request.AddJsonBody(productosDTO);
            AddAuthentication(request);

            var response = await client.ExecuteAsync<ProductosDTO>(request);
            return response.IsSuccessful;

        }

        public async Task<bool> PostProductoAsync(ProductosDTO productosDTO)
        {
            var client = new RestClient(_url);
            var request = new RestRequest("producto", Method.Post);
            request.AddJsonBody(productosDTO);
            AddAuthentication(request);

            var response = await client.ExecuteAsync<ProductosDTO>(request);
            return response.IsSuccessful;

        }

        public async Task<bool> DeleteProductoAsync(int id)
        {
            var client = new RestClient(_url);
            var request = new RestRequest($"producto/{id}", Method.Delete);
            AddAuthentication(request);

            var response = await client.ExecuteAsync<ProductosDTO>(request);
            return response.IsSuccessful;


        }

        //Provedores

        public async Task<List<ProvedoresDTO>> GetProvedoresAsync()
        {
            var cliente = new RestClient(_url);
            var request = new RestRequest("proveedores", Method.Get);
            AddAuthentication(request);

            var response = await cliente.ExecuteAsync<List<ProvedoresDTO>>(request);
            if (response.IsSuccessful)
            {
                return response.Data!;
            }
            throw new Exception(response.ErrorMessage);

        }

        public async Task<ProvedoresDTO> GetProveedoresIDAsync(int id)
        {
            var client = new RestClient(_url);
            var request = new RestRequest($"proveedores/{id}", Method.Get);
            AddAuthentication(request);

            var response = await client.ExecuteAsync<ProvedoresDTO>(request);
            if (response.IsSuccessful)
            {
                return response.Data!;
            }
            throw new Exception(response.ErrorMessage);

        }

        public async Task<bool> PutProvedoresAsync(ProvedoresDTO provedoresDTO)
        {
            var client = new RestClient(_url);
            var request = new RestRequest("proveedores", Method.Put);
            request.AddJsonBody(provedoresDTO);
            AddAuthentication(request);

            var response = await client.ExecuteAsync<ProvedoresDTO>(request);
            return response.IsSuccessful;

        }

        public async Task<bool> PostProvedoresAsync(ProvedoresDTO provedoresDTO)
        {
            var client = new RestClient(_url);
            var request = new RestRequest("proveedores", Method.Post);
            request.AddJsonBody(provedoresDTO);
            AddAuthentication(request);

            var response = await client.ExecuteAsync<ProvedoresDTO>(request);
            return response.IsSuccessful;

        }

        public async Task<bool> DeleteProvedoresAsync(int id)
        {
            var client = new RestClient(_url);
            var request = new RestRequest($"proveedores/{id}", Method.Delete);
            AddAuthentication(request);

            var response = await client.ExecuteAsync<ProvedoresDTO>(request);
            return response.IsSuccessful;


        }

    }
}


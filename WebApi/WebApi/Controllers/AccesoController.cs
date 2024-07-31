using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Custom;
using Microsoft.AspNetCore.Authorization;
using WebApi.Models;
using WebAPI.Services;
using WebApi.Middleware;
using WebApi.DTOs;
using WebApi.Interfaces;
using WebApi.Data;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous] // Permite el acceso sin autenticación para Registrarse y Login
    [ApiController]
    public class AccesoController : ControllerBase
    {
        private readonly DbpruebaContext _dbPruebaContext;
        private readonly Configuraciones _utilidades;
        private readonly EmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;

        public AccesoController(DbpruebaContext dbPruebaContext, Configuraciones utilidades, EmailService emailService, IUnitOfWork unitOfWork)
        {
            _dbPruebaContext = dbPruebaContext;
            _utilidades = utilidades;
            _emailService = emailService;
            _unitOfWork = unitOfWork;
        }

        // POST: api/Acceso/Registrarse
        [HttpPost]
        [Route("Registrarse")]
        public async Task<IActionResult> Registrarse(UsuarioDTO objeto)
        {
            //metemos el metodo dentro de un try catch para que si salta el error se envie al middleware

            try
            {
                // Verificar si el usuario ya existe
                var existingUser = await _unitOfWork.Acceso.GetUsuarioCorreo(objeto.Correo);

                if (existingUser != null)
                {
                    throw new LoginException("El usuario ya existe.");
                }

                // Crear un nuevo usuario en la base de datos
                var modeloUsuario = new Usuario
                {
                    Nombre = objeto.Nombre,
                    Correo = objeto.Correo,
                    Clave = _utilidades.EncriptarPassword(objeto.Clave) // Encriptar la contraseña usando BCrypt
                };

                await _unitOfWork.Acceso.CreateUser(modeloUsuario);
                await _unitOfWork.SaveChangesAsync();

                if (modeloUsuario.IdUsuario == 0)
                {
                    // Preparar el cuerpo del correo electrónico de bienvenida
                    string emailBody = $@"
<html>
<head>
    <style>
        body {{
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            color: #333;
            padding: 20px;
        }}
        .container {{
            max-width: 600px;
            margin: auto;
            background-color: #fff;
            padding: 20px;
            border-radius: 10px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
        }}
        h1 {{
            color: #007bff;
            text-align: center;
        }}
        p {{
            line-height: 1.6;
        }}
        ul {{
            list-style-type: none;
            padding: 0;
        }}
        ul li {{
            background-color: #f9f9f9;
            margin: 10px 0;
            padding: 10px;
            border-radius: 5px;
        }}
        .footer {{
            margin-top: 20px;
            text-align: center;
            font-size: 0.9em;
            color: #666;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <h1>¡Bienvenido, {modeloUsuario.Nombre}!</h1>
        <p>Gracias por registrarte en nuestra plataforma.</p>
        <p>A continuación, te proporcionamos los detalles de tu cuenta:</p>
        <ul>
            <li><strong>Nombre:</strong> {modeloUsuario.Nombre}</li>
            <li><strong>Correo electrónico:</strong> {modeloUsuario.Correo}</li>
            <li><strong>Contraseña:</strong> {objeto.Clave}</li>
        </ul>
        <p>Te recordamos que al iniciar sesión, se te otorgará un token único con el que podrás interactuar con nuestra API.</p>
        <p>¡Gracias!</p>
        <p class='footer'>El equipo de tu aplicación API</p>
    </div>
</body>
</html>";



                    // Enviar correo electrónico utilizando el servicio de EmailService
                    await _emailService.SendEmailAsync(modeloUsuario.Correo, "Registro exitoso", emailBody);

                    return StatusCode(StatusCodes.Status200OK, new { isSuccess = true });
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, new { isSuccess = false });
                }
            }
            catch (LoginException ex)
            {
                throw new LoginException(ex.Message);
            }





        }

        // POST: api/Acceso/Login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginDTO objeto)
        {

            try
            {
                // Buscar el usuario por correo electrónico en la base de datos
                var usuarioEncontrado = await _unitOfWork.Acceso.GetUsuarioCorreo(objeto.Correo);

                // Verificar si el usuario existe y si la contraseña coincide
                if (usuarioEncontrado == null || !_utilidades.VerificarPassword(objeto.Clave, usuarioEncontrado.Clave))
                {
                    // Retornar un estado de éxito falso si las credenciales no son válidas
                    return StatusCode(StatusCodes.Status200OK, new { isSuccess = false, token = "" });
                }
                else
                {
                    // Generar y retornar un token JWT si las credenciales son válidas
                    return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, token = _utilidades.generarJWT(usuarioEncontrado) });
                }

            }
            catch (LoginException ex)
            {
                throw new LoginException(ex.Message);
            }
        }

        // DELETE: api/Acceso/EliminarUsuario/{id}
        [HttpDelete]
        [Route("EliminarUsuario/{id}")]
        public async Task<IActionResult> EliminarUsuario(int id)
        {
            try
            {
                var result = await _unitOfWork.Acceso.DeleteUser(id);

                if (result)
                {
                    return StatusCode(StatusCodes.Status200OK, new { isSuccess = true });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new { isSuccess = false, message = "Usuario no encontrado" });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // PUT: api/Acceso/ActualizarUsuario
        [HttpPut]
        [Route("ActualizarUsuario")]
        public async Task<IActionResult> ActualizarUsuario(UsuarioDTO objeto)
        {
            try
            {
                // Buscar el usuario por correo
                var usuario = await _unitOfWork.Acceso.GetUsuarioCorreo(objeto.Correo);

                // Verificar si el usuario existe
                if (usuario == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new { isSuccess = false, message = "Usuario no encontrado" });
                }

                // Actualizar las propiedades del usuario
                usuario.Nombre = objeto.Nombre;
                usuario.Clave = _utilidades.EncriptarPassword(objeto.Clave); // Encriptar la nueva contraseña

                // Llamar al método UpdateUser para actualizar el usuario en la base de datos
                var result = await _unitOfWork.Acceso.UpdateUser(usuario);

                // Verificar el resultado de la actualización
                if (result)
                {
                    return StatusCode(StatusCodes.Status200OK, new { isSuccess = true });
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { isSuccess = false });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
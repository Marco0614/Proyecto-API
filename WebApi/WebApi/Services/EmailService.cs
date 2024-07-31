using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Configuration;

namespace WebAPI.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Método para enviar correo electrónico asincrónico
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            // Obtener la configuración de correo electrónico desde appsettings.json
            var emailSettings = _configuration.GetSection("EmailSettings");

            // Configuración de parámetros del correo electrónico
            string fromEmail = emailSettings["From"];             // Dirección de correo del remitente
            string smtpServer = emailSettings["SmtpServer"];      // Servidor SMTP
            int port = int.Parse(emailSettings["Port"]);          // Puerto SMTP
            string username = emailSettings["Username"];          // Usuario SMTP
            string password = emailSettings["Password"];          // Contraseña SMTP

            // Creación del mensaje MIME
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Api MKJ", fromEmail));  // Remitente
            message.To.Add(new MailboxAddress("", toEmail));           // Destinatario
            message.Subject = subject;                                  // Asunto del correo

            var bodyBuilder = new BodyBuilder { HtmlBody = body };      // Cuerpo del correo HTML
            message.Body = bodyBuilder.ToMessageBody();                 // Establecer el cuerpo del mensaje

            // Cliente SMTP para enviar el correo
            using (var client = new SmtpClient())
            {
                try
                {
                    // Conectar al servidor SMTP usando TLS
                    await client.ConnectAsync(smtpServer, port, SecureSocketOptions.StartTls);

                    // Autenticarse con el servidor SMTP
                    await client.AuthenticateAsync(username, password);

                    // Enviar el mensaje
                    await client.SendAsync(message);
                }
                catch (Exception ex)
                {
                    // Captura cualquier error al enviar el correo
                    Console.WriteLine($"Error al enviar email: {ex.Message}");
                    throw;  // Relanza la excepción para manejarla más arriba si es necesario
                }
                finally
                {
                    // Desconectar del servidor SMTP
                    await client.DisconnectAsync(true);
                }
            }
        }
    }
}

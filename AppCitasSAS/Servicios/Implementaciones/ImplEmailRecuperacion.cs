using AppCitasSAS.Servicios.Interfaces;
using AppCitasSAS.Utils;
using System.Net.Mail;
using System.IO;  // Se añadió para la excepción IOException

namespace AppCitasSAS.Servicios.Implementaciones
{
    public class ImplEmailRecuperacion : IntfEmailRecuperacion
    {
        // Clase de implementación para enviar correos electrónicos relacionados con la recuperación de cuenta y contraseña

        // Método para enviar un correo de confirmación
        void IntfEmailRecuperacion.enviarEmailConfirmacion(string emailDestino, string nombreUsuario, string token)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método enviarEmailConfirmacion() de la clase ImplEmailRecuperacion");

                // URL base del dominio
                string urlDominio = "http://localhost:5187";

                // Dirección de correo electrónico de origen
                string emailOrigen = "danitbp12@gmail.com";

                // Crear la URL de confirmación con el token
                string urlDeRecuperacion = string.Format("{0}/auth/confirmar-cuenta/?token={1}", urlDominio, token);

                // Leer el contenido del archivo HTML de la plantilla
                string directorioProyecto = Directory.GetCurrentDirectory();
                string rutaArchivo = Path.Combine(directorioProyecto, "Plantillas/ConfirmarCorreo.html");
                string htmlContent = File.ReadAllText(rutaArchivo);

                // Asignar valores al HTML
                htmlContent = string.Format(htmlContent, nombreUsuario, urlDeRecuperacion);

                // Crear mensaje de correo
                MailMessage mensajeDelCorreo = new MailMessage(emailOrigen, emailDestino, "CONFIRMAR EMAIL AppCitasSAS", htmlContent);

                mensajeDelCorreo.IsBodyHtml = true;

                // Configurar el cliente SMTP
                SmtpClient smtpCliente = new SmtpClient("smtp.gmail.com");
                smtpCliente.EnableSsl = true;
                smtpCliente.UseDefaultCredentials = false;
                smtpCliente.Port = 587;
                smtpCliente.Credentials = new System.Net.NetworkCredential(emailOrigen, "exwl tblj sanf siey");

                // Enviar el correo
                smtpCliente.Send(mensajeDelCorreo);

                // Liberar recursos
                smtpCliente.Dispose();

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método enviarEmailConfirmacion() de la clase ImplEmailRecuperacion");

            }
            catch (IOException ioe)
            {
                EscribirLog.escribirEnFicheroLog("[Error ImplEmailRecuperacion - enviarEmailConfirmacion()] Error al leer el fichero html para enviar email de confirmación: " + ioe.Message);
            }
            catch (SmtpException se)
            {
                EscribirLog.escribirEnFicheroLog("[Error ImplEmailRecuperacion - enviarEmailConfirmacion()] Error con el protocolo de envío de correo electrónico: " + se.Message);
            }
        }

        // Método para enviar un correo de recuperación de contraseña
        void IntfEmailRecuperacion.enviarEmailRecuperacion(string emailDestino, string nombreUsuario, string token)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método enviarEmailRecuperacion() de la clase ImplEmailRecuperacion");

                // URL base del dominio
                string urlDominio = "http://localhost:5187";

                // Dirección de correo electrónico de origen
                string emailOrigen = "danitbp12@gmail.com";

                // Crear la URL de recuperación con el token
                string urlDeRecuperacion = string.Format("{0}/auth/recuperar/?token={1}", urlDominio, token);

                // Leer el contenido del archivo HTML de la plantilla
                string directorioProyecto = Directory.GetCurrentDirectory();
                string rutaArchivo = Path.Combine(directorioProyecto, "Plantillas/RecuperacionContraseña.html");
                string htmlContent = File.ReadAllText(rutaArchivo);

                // Asignar valores al HTML
                htmlContent = string.Format(htmlContent, nombreUsuario, urlDeRecuperacion);

                // Crear mensaje de correo
                MailMessage mensajeDelCorreo = new MailMessage(emailOrigen, emailDestino, "RESTABLECER CONTRASEÑA AppCitasSAS", htmlContent);

                mensajeDelCorreo.IsBodyHtml = true;

                // Configurar el cliente SMTP
                SmtpClient smtpCliente = new SmtpClient("smtp.gmail.com");
                smtpCliente.EnableSsl = true;
                smtpCliente.UseDefaultCredentials = false;
                smtpCliente.Port = 587;
                smtpCliente.Credentials = new System.Net.NetworkCredential(emailOrigen, "exwl tblj sanf siey");

                // Enviar el correo
                smtpCliente.Send(mensajeDelCorreo);

                // Liberar recursos
                smtpCliente.Dispose();

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método enviarEmailRecuperacion() de la clase ImplEmailRecuperacion");

            }
            catch (IOException ioe)
            {
                EscribirLog.escribirEnFicheroLog("[Error ImplEmailRecuperacion - enviarEmailRecuperacion()] Error al leer el fichero html para enviar email de recuperación: " + ioe.Message);
            }
            catch (SmtpException se)
            {
                EscribirLog.escribirEnFicheroLog("[Error ImplEmailRecuperacion - enviarEmailRecuperacion()] Error con el protocolo de envío de correo electrónico: " + se.Message);
            }
        }
    }
}

using AppCitasSAS.DTO;
using AppCitasSAS.Servicios.Interfaces;
using AppCitasSAS.Utils;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace AppCitasSAS.Controllers
{
    /// <summary>
    /// Controlador para la gestión de las acciones relacionadas con el inicio de sesión y confirmación de cuentas.
    /// </summary>
    public class LoginController : Controller
    {
        private readonly IntfPacienteServicio _pacienteServicio;

        /// <summary>
        /// Constructor que inicializa el controlador con el servicio de pacientes.
        /// </summary>
        /// <param name="pacienteServicio">Servicio de pacientes.</param>
        public LoginController(IntfPacienteServicio pacienteServicio)
        {
            _pacienteServicio = pacienteServicio;
        }

        /// <summary>
        /// Muestra la vista de inicio de sesión.
        /// </summary>
        /// <returns>Vista de inicio de sesión.</returns>
        [HttpGet]
        [Route("/auth/login")]
        public IActionResult InicioSesion()
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método InicioSesion() de la clase LoginController");

                // Crear un nuevo objeto PacienteDTO.
                PacienteDTO pacienteDTO = new PacienteDTO();

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método InicioSesion() de la clase LoginController");

                return View("~/Views/Home/login.cshtml", pacienteDTO);
            }
            catch (Exception ex)
            {
                // Manejar excepciones y registrar en el log.
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método InicioSesion() de la clase LoginController: " + ex.Message + ex.StackTrace);
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                return View("~/Views/Home/login.cshtml");
            }
        }

        /// <summary>
        /// Procesa el inicio de sesión del paciente.
        /// </summary>
        /// <param name="pacienteDTO">Datos del paciente para iniciar sesión.</param>
        /// <returns>Redirección a la página principal del paciente.</returns>
        [HttpPost]
        [Route("/auth/iniciar-sesion")]
        public IActionResult ProcesarInicioSesion(PacienteDTO pacienteDTO)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método ProcesarInicioSesion() de la clase LoginController");

                bool credencialesValidas = _pacienteServicio.verificarCredenciales(pacienteDTO.EmailPaciente, pacienteDTO.ContrasenaPaciente);

                if (credencialesValidas)
                {
                    PacienteDTO u = _pacienteServicio.buscarPorEmail(pacienteDTO.EmailPaciente);

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, pacienteDTO.EmailPaciente),
                    };
                    if (!string.IsNullOrEmpty(u.RolPaciente))
                    {
                        claims.Add(new Claim(ClaimTypes.Role, u.RolPaciente));
                    }
                    var identidadDeReclamaciones = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método ProcesarInicioSesion() de la clase LoginController");

                    if (u.RolPaciente.Contains("ROLE_ADMIN"))
                    {
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identidadDeReclamaciones));
                        return RedirectToAction("HomeEmpleado", "Paciente");
                    }
                    else
                    {
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identidadDeReclamaciones));
                        return RedirectToAction("HomeUser", "Paciente");
                    }
                }
                else
                {
                    ViewData["MensajeErrorInicioSesion"] = "Credenciales inválidas o cuenta no confirmada. Inténtelo de nuevo.";
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método ProcesarInicioSesion() de la clase LoginController. " + ViewData["MensajeErrorInicioSesion"]);
                    return View("~/Views/Home/login.cshtml");
                }
            }
            catch (Exception ex)
            {
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método ProcesarInicioSesion() de la clase LoginController: " + ex.Message + ex.StackTrace);
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                return View("~/Views/Home/login.cshtml");
            }
        }

        /// <summary>
        /// Confirma la cuenta del paciente mediante un token.
        /// </summary>
        /// <param name="token">Token de confirmación de la cuenta.</param>
        /// <returns>Vista con el resultado de la confirmación.</returns>
        [HttpGet]
        [Route("/auth/confirmar-cuenta")]
        public IActionResult ConfirmarCuenta([FromQuery] string token)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método ConfirmarCuenta() de la clase LoginController");

                bool confirmacionExitosa = _pacienteServicio.confirmarCuenta(token);

                if (confirmacionExitosa)
                {
                    ViewData["CuentaVerificada"] = "La dirección de correo ha sido confirmada correctamente";
                }
                else
                {
                    ViewData["yaEstabaVerificada"] = "El usuario ya estaba registrado y verificado";
                }

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método ConfirmarCuenta() de la clase LoginController" +
                    (ViewData["CuentaVerificada"] != null ? ". " + ViewData["CuentaVerificada"] :
                    (ViewData["yaEstabaVerificada"] != null ? ". " + ViewData["yaEstabaVerificada"] : "")));

                return View("~/Views/Home/login.cshtml");
            }
            catch (Exception ex)
            {
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método ConfirmarCuenta() de la clase LoginController: " + ex.Message + ex.StackTrace);
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                return View("~/Views/Home/login.cshtml");
            }
        }
    }
}

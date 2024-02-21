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
    public class LoginController : Controller
    {
        private readonly IntfPacienteServicio _pacienteServicio;

        public LoginController(IntfPacienteServicio pacienteServicio)
        {
            _pacienteServicio = pacienteServicio;
        }

        [HttpGet]
        [Route("/auth/login")]
        public IActionResult InicioSesion()
        {
            EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método Login() de la clase LoginController");
            try
            {
                PacienteDTO pacienteDTO = new PacienteDTO();
                return View("~/Views/Home/login.cshtml", pacienteDTO);

            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método Login() de la clase LoginController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/login.cshtml");
            }
        }


        [HttpPost]
        [Route("/auth/iniciar-sesion")]
        public IActionResult ProcesarInicioSesion(PacienteDTO pacienteDTO)
        {
            try
            {
                bool credencialesValidas = _pacienteServicio.verificarCredenciales(pacienteDTO.EmailPaciente, pacienteDTO.ContrasenaPaciente);

                if (credencialesValidas)
                {
                    PacienteDTO u = _pacienteServicio.buscarPorEmail(pacienteDTO.EmailPaciente);

                    // Al hacer login correctamente se crea una identidad de reclamaciones (claims identity) con información del usuario 
                    // y su rol, de esta manera se controla que solo los admin puedan acceder a la administracion de usuarios
                    // y se mantiene esa info del usuario autenticado durante toda la sesión en una cookie.
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
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método ProcesarInicioSesion() de la clase LoginController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/login.cshtml");
            }
        }


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
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método ConfirmarCuenta() de la clase LoginController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/login.cshtml");
            }
        }


        [HttpPost]
        public IActionResult CerrarSesion()
        {
            EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método CerrarSesion() de la clase LoginController");
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
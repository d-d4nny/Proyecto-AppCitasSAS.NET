using AppCitasSAS.DTO;
using AppCitasSAS.Servicios.Interfaces;
using AppCitasSAS.Utils;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        public IActionResult Index()
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

                    // establece una cookie en el navegador con los datos del usuario antes mencionados y se mantiene en el contexto.
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identidadDeReclamaciones));

                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método ProcesarInicioSesion() de la clase LoginController");
                    return RedirectToAction("Dashboard", "Login");
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
    }
}

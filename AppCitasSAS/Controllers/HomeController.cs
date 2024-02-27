using AppCitasSAS.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using AppCitasSAS.Utils;

namespace AppCitasSAS.Controllers
{
    /// <summary>
    /// Controlador para la gestión de las acciones en la página principal y errores.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        /// <summary>
        /// Constructor que inicializa el controlador con un logger.
        /// </summary>
        /// <param name="logger">Logger para el controlador.</param>
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Acción que muestra la vista de inicio.
        /// </summary>
        /// <returns>Vista de inicio de sesión.</returns>
        public IActionResult Index()
        {
            try
            {
                // Registro en el log de la entrada al método.
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método Index() de la clase HomeController");

                // Cerrar la sesión actual.
                HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                // Registro en el log de la salida del método.
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método Index() de la clase HomeController");

                return View("~/Views/Home/login.cshtml");
            }
            catch (Exception e)
            {
                // Manejar excepciones y registrar en el log.
                ViewData["error"] = "Ocurrió un error al mostrar la vista de Home";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método Index() de la clase HomeController: " + e.Message + e.StackTrace);

                return View("~/Views/Home/login.cshtml");
            }
        }

        /// <summary>
        /// Acción que maneja los errores y muestra la vista correspondiente.
        /// </summary>
        /// <returns>Vista de error con información sobre el error.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

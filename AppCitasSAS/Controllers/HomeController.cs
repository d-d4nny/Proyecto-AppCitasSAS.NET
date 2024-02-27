using AppCitasSAS.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using AppCitasSAS.Utils;

namespace AppCitasSAS.Controllers
{
    /// <summary>
    /// Controlador para la gesti�n de las acciones en la p�gina principal y errores.
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
        /// Acci�n que muestra la vista de inicio.
        /// </summary>
        /// <returns>Vista de inicio de sesi�n.</returns>
        public IActionResult Index()
        {
            try
            {
                // Registro en el log de la entrada al m�todo.
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el m�todo Index() de la clase HomeController");

                // Cerrar la sesi�n actual.
                HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                // Registro en el log de la salida del m�todo.
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del m�todo Index() de la clase HomeController");

                return View("~/Views/Home/login.cshtml");
            }
            catch (Exception e)
            {
                // Manejar excepciones y registrar en el log.
                ViewData["error"] = "Ocurri� un error al mostrar la vista de Home";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanz� una excepci�n en el m�todo Index() de la clase HomeController: " + e.Message + e.StackTrace);

                return View("~/Views/Home/login.cshtml");
            }
        }

        /// <summary>
        /// Acci�n que maneja los errores y muestra la vista correspondiente.
        /// </summary>
        /// <returns>Vista de error con informaci�n sobre el error.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

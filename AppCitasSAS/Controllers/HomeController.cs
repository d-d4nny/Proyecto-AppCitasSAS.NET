using AppCitasSAS.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using AppCitasSAS.Utils;

namespace AppCitasSAS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el m�todo  Index() de la clase HomeController");

                HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return View();
            }
            catch (Exception e)
            {
                ViewData["error"] = "Ocurri� un error al mostrar la vista de Home";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanz� una excepci�n en el m�todo Index() de la clase HomeController: " + e.Message + e.StackTrace);
                return View();
            }
        }   

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
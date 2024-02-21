using AppCitasSAS.DTO;
using AppCitasSAS.Servicios.Interfaces;
using AppCitasSAS.Utils;
using Microsoft.AspNetCore.Mvc;

namespace AppCitasSAS.Controllers
{
    public class RecuperarPasswordController : Controller
    {

        private readonly IntfPacienteServicio _pacienteServicio;

        public RecuperarPasswordController(IntfPacienteServicio pacienteServicio)
        {
            _pacienteServicio = pacienteServicio;
        }



        [HttpGet]
        [Route("/auth/solicitar-recuperacion")]
        public IActionResult MostrarVistaIniciarRecuperacion()
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método MostrarVistaIniciarRecuperacion() de la clase SolicitarRecuperacionController");

                PacienteDTO pacienteDTO = new PacienteDTO();
                return View("~/Views/Home/solicitarRecuperacion.cshtml", pacienteDTO);
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método MostrarVistaIniciarRecuperacion() de la clase SolicitarRecuperacionController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/solicitarRecuperacion.cshtml");
            }
        }

        [HttpPost]
        [Route("/auth/iniciar-recuperacion")]
        public IActionResult ProcesarInicioRecuperacion([Bind("EmailPaciente")] PacienteDTO pacienteDTO)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método ProcesarInicioRecuperacion() de la clase SolicitarRecuperacionController");

                bool envioConExito = _pacienteServicio.iniciarProcesoRecuperacion(pacienteDTO.EmailPaciente);

                if (envioConExito)
                {
                    ViewData["MensajeExitoMail"] = "Proceso de recuperación OK";
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método ProcesarInicioRecuperacion() de la clase SolicitarRecuperacionController. " + ViewData["MensajeExitoMail"]);
                    return View("~/Views/Home/login.cshtml");
                }
                else
                {
                    ViewData["MensajeErrorMail"] = "No se inició el proceso de recuperación, cuenta de correo electrónico no encontrada.";
                }
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método ProcesarInicioRecuperacion() de la clase SolicitarRecuperacionController. " + ViewData["MensajeErrorMail"]);
                return View("~/Views/Home/solicitarRecuperacion.cshtml");
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método ProcesarInicioRecuperacion() de la clase SolicitarRecuperacionController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/solicitarRecuperacion.cshtml");
            }
        }



        [HttpGet]
        [Route("/auth/recuperar")]
        public IActionResult MostrarVistaRecuperar([FromQuery(Name = "token")] string token)
        {

            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método MostrarVistaRecuperar() de la clase RecuperarPasswordController");

                PacienteDTO paciente = _pacienteServicio.obtenerPacientePorToken(token);

                if (paciente != null)
                {
                    ViewData["PacienteDTO"] = paciente;
                }
                else
                {
                    ViewData["MensajeErrorTokenValidez"] = "El enlace de recuperación no es válido o el usuario no se ha encontrado";
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método MostrarVistaRecuperar() de la clase RecuperarPasswordController. " + ViewData["MensajeErrorTokenValidez"]);
                    return View("~/Views/Home/solicitarRecuperacion.cshtml");
                }
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método MostrarVistaRecuperar() de la clase RecuperarPasswordController");
                return View("~/Views/Home/recuperar.cshtml");
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método MostrarVistaRecuperar() de la clase RecuperarPasswordController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/solicitarRecuperacion.cshtml");
            }
        }


        [HttpPost]
        [Route("/auth/recuperar")]
        public IActionResult ProcesarRecuperacionContraseña(PacienteDTO pacienteDTO)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método ProcesarRecuperacionContraseña() de la clase RecuperarPasswordController");

                PacienteDTO pacienteExistente = _pacienteServicio.obtenerPacientePorToken(pacienteDTO.Token);

                if (pacienteExistente == null)
                {
                    ViewData["MensajeErrorTokenValidez"] = "El enlace de recuperación no es válido";
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método ProcesarRecuperacionContraseña() de la clase RecuperarPasswordController. " + ViewData["MensajeErrorTokenValidez"]);
                    return View("~/Views/Home/solicitarRecuperacion.cshtml");
                }

                if (pacienteExistente.ExpiracionToken.HasValue && pacienteExistente.ExpiracionToken.Value < DateTime.Now)
                {
                    ViewData["MensajeErrorTokenExpirado"] = "El enlace de recuperación ha expirado";
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método ProcesarRecuperacionContraseña() de la clase RecuperarPasswordController. " + ViewData["MensajeErrorTokenExpirado"]);
                    return View("~/Views/Home/solicitarRecuperacion.cshtml");
                }

                bool modificadaPassword = _pacienteServicio.modificarContraseñaConToken(pacienteDTO);

                if (modificadaPassword)
                {
                    ViewData["ContraseñaModificadaExito"] = "Contraseña modificada OK";
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método ProcesarRecuperacionContraseña() de la clase RecuperarPasswordController. " + ViewData["ContraseñaModificadaExito"]);
                    return View("~/Views/Home/login.cshtml");
                }
                else
                {
                    ViewData["ContraseñaModificadaError"] = "Error al cambiar de contraseña";
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método ProcesarRecuperacionContraseña() de la clase RecuperarPasswordController. " + ViewData["ContraseñaModificadaError"]);
                    return View("~/Views/Home/solicitarRecuperacion.cshtml");
                }
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método ProcesarRecuperacionContraseña() de la clase RecuperarPasswordController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/solicitarRecuperacion.cshtml");
            }
        }
    }
}

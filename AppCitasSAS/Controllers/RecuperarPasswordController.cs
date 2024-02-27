using AppCitasSAS.DTO;
using AppCitasSAS.Servicios.Interfaces;
using AppCitasSAS.Utils;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AppCitasSAS.Controllers
{
    /// <summary>
    /// Controlador para gestionar la recuperación de contraseña de los pacientes.
    /// </summary>
    public class RecuperarPasswordController : Controller
    {
        private readonly IntfPacienteServicio _pacienteServicio;

        /// <summary>
        /// Constructor del controlador RecuperarPasswordController.
        /// </summary>
        /// <param name="pacienteServicio">Servicio de paciente para la recuperación de contraseña.</param>
        public RecuperarPasswordController(IntfPacienteServicio pacienteServicio)
        {
            _pacienteServicio = pacienteServicio;
        }

        /// <summary>
        /// Muestra la vista para iniciar el proceso de recuperación de contraseña.
        /// </summary>
        /// <returns>Vista de solicitud de recuperación de contraseña.</returns>
        [HttpGet]
        [Route("/auth/solicitar-recuperacion")]
        public IActionResult MostrarVistaIniciarRecuperacion()
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método MostrarVistaIniciarRecuperacion() de la clase RecuperarPasswordController");

                PacienteDTO pacienteDTO = new PacienteDTO();
                return View("~/Views/Home/solicitarRecuperacion.cshtml", pacienteDTO);
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método MostrarVistaIniciarRecuperacion() de la clase RecuperarPasswordController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/solicitarRecuperacion.cshtml");
            }
        }

        /// <summary>
        /// Procesa el inicio del proceso de recuperación de contraseña.
        /// </summary>
        /// <param name="pacienteDTO">Datos del paciente, especialmente el email para iniciar la recuperación.</param>
        /// <returns>Vista de inicio de sesión si la recuperación es exitosa, de lo contrario, vuelve a la vista de solicitud de recuperación.</returns>
        [HttpPost]
        [Route("/auth/iniciar-recuperacion")]
        public IActionResult ProcesarInicioRecuperacion([Bind("EmailPaciente")] PacienteDTO pacienteDTO)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método ProcesarInicioRecuperacion() de la clase RecuperarPasswordController");

                // Intenta iniciar el proceso de recuperación
                bool envioConExito = _pacienteServicio.iniciarProcesoRecuperacion(pacienteDTO.EmailPaciente);

                if (envioConExito)
                {
                    ViewData["MensajeExitoMail"] = "Proceso de recuperación OK";
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método ProcesarInicioRecuperacion() de la clase RecuperarPasswordController. " + ViewData["MensajeExitoMail"]);
                    return View("~/Views/Home/login.cshtml");
                }
                else
                {
                    ViewData["MensajeErrorMail"] = "No se inició el proceso de recuperación, cuenta de correo electrónico no encontrada.";
                }
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método ProcesarInicioRecuperacion() de la clase RecuperarPasswordController. " + ViewData["MensajeErrorMail"]);
                return View("~/Views/Home/solicitarRecuperacion.cshtml");
            }
            catch (Exception e)
            {
                // Control de excepciones
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método ProcesarInicioRecuperacion() de la clase RecuperarPasswordController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/solicitarRecuperacion.cshtml");
            }
        }

        /// <summary>
        /// Muestra la vista para recuperar la contraseña con el token proporcionado.
        /// </summary>
        /// <param name="token">Token de recuperación para validar y recuperar la contraseña.</param>
        /// <returns>Vista de recuperación de contraseña si el token es válido, de lo contrario, vuelve a la vista de solicitud de recuperación.</returns>
        [HttpGet]
        [Route("/auth/recuperar")]
        public IActionResult MostrarVistaRecuperar([FromQuery(Name = "token")] string token)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método MostrarVistaRecuperar() de la clase RecuperarPasswordController");

                // Obtiene el paciente asociado al token
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
                // Control de excepciones
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método MostrarVistaRecuperar() de la clase RecuperarPasswordController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/solicitarRecuperacion.cshtml");
            }
        }

        /// <summary>
        /// Procesa la recuperación de la contraseña con el token proporcionado.
        /// </summary>
        /// <param name="pacienteDTO">Datos del paciente, incluida la nueva contraseña y el token de recuperación.</param>
        /// <returns>Vista de inicio de sesión si la recuperación es exitosa, de lo contrario, vuelve a la vista de solicitud de recuperación.</returns>
        [HttpPost]
        [Route("/auth/recuperar")]
        public IActionResult ProcesarRecuperacionContraseña(PacienteDTO pacienteDTO)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método ProcesarRecuperacionContraseña() de la clase RecuperarPasswordController");

                // Verifica la existencia del paciente y validez del token
                PacienteDTO pacienteExistente = _pacienteServicio.obtenerPacientePorToken(pacienteDTO.Token);

                if (pacienteExistente == null)
                {
                    ViewData["MensajeErrorTokenValidez"] = "El enlace de recuperación no es válido";
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método ProcesarRecuperacionContraseña() de la clase RecuperarPasswordController. " + ViewData["MensajeErrorTokenValidez"]);
                    return View("~/Views/Home/solicitarRecuperacion.cshtml");
                }

                // Verifica la vigencia del token
                if (pacienteExistente.ExpiracionToken.HasValue && pacienteExistente.ExpiracionToken.Value < DateTime.Now)
                {
                    ViewData["MensajeErrorTokenExpirado"] = "El enlace de recuperación ha expirado";
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método ProcesarRecuperacionContraseña() de la clase RecuperarPasswordController. " + ViewData["MensajeErrorTokenExpirado"]);
                    return View("~/Views/Home/solicitarRecuperacion.cshtml");
                }

                // Intenta modificar la contraseña con el token
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
                // Control de excepciones
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método ProcesarRecuperacionContraseña() de la clase RecuperarPasswordController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/solicitarRecuperacion.cshtml");
            }
        }
    }
}

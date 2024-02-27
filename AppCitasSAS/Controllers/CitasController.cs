using AppCitasSAS.DTO;
using AppCitasSAS.Servicios.Interfaces;
using AppCitasSAS.Utils;
using DAL.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using System.Security.Principal;

namespace AppCitasSAS.Controllers
{
    /// <summary>
    /// Controlador para la gestión de citas médicas.
    /// </summary>
    public class CitasController : Controller
    {
        private readonly IntfCitasServicio _citaServicio;
        private readonly IntfPacienteServicio _pacienteServicio;
        private readonly IntfDoctorServicio _doctorServicio;
        private readonly IntfCitasToDao _citaToDao;
        private readonly IntfCitasToDto _citaToDto;

        /// <summary>
        /// Constructor que inicializa instancias de servicios e interfaces necesarios.
        /// </summary>
        public CitasController(IntfCitasServicio citaServicio, IntfPacienteServicio pacienteServicio, IntfDoctorServicio doctorServicio, IntfCitasToDao citaToDao, IntfCitasToDto citaToDto)
        {
            _citaServicio = citaServicio;
            _pacienteServicio = pacienteServicio;
            _doctorServicio = doctorServicio;
            _citaToDao = citaToDao;
            _citaToDto = citaToDto;
        }

        /// <summary>
        /// Muestra el formulario para crear una nueva cita médica.
        /// </summary>
        /// <returns>Vista para la creación de citas médicas.</returns>
        [Authorize]
        [HttpGet]
        [Route("/privada/crear-cita")]
        public IActionResult MostrarFormNuevaCita()
        {
            try
            {
                // Registro en el log de la entrada al método.
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método MostrarFormNuevaCita() de la clase CitasController");

                // Obtener la lista de doctores y el paciente actual.
                List<DoctoresDTO> doctores = _doctorServicio.buscarTodos();
                PacienteDTO paciente = _pacienteServicio.buscarPorEmail(User.Identity.Name);

                // Crear una nueva cita y configurar ViewBag para la vista.
                CitasDTO nuevaCita = new CitasDTO();
                ViewBag.Doctores = doctores;
                ViewBag.Pacientes = paciente;

                // Registro en el log de la salida del método.
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método MostrarFormNuevaCita() de la clase CitasController");

                return View("~/Views/Home/crearCita.cshtml", nuevaCita);
            }
            catch (Exception e)
            {
                // Manejar excepciones y registrar en el log.
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método MostrarFormNuevaCita() de la clase CitasController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/homePaciente.cshtml");
            }
        }

        /// <summary>
        /// Registra una nueva cita médica.
        /// </summary>
        /// <param name="citaDTO">Datos de la cita a registrar.</param>
        /// <returns>Redirección a la página principal del usuario paciente.</returns>
        [Authorize]
        [HttpPost]
        [Route("/privada/crear-cita")]
        public IActionResult RegistrarCitaPost(CitasDTO citaDTO)
        {
            try
            {
                // Registro en el log de la entrada al método.
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método RegistrarCitaPost() de la clase CitasController");

                // Obtener el paciente actual.
                PacienteDTO u = _pacienteServicio.buscarPorEmail(User.Identity.Name);

                // Asignar el Id del paciente a la cita y registrarla.
                citaDTO.IdPacienteDTO = u.IdPaciente;
                _citaServicio.registrar(citaDTO);

                return RedirectToAction("HomeUser", "Paciente");
            }
            catch (Exception e)
            {
                // Manejar excepciones y registrar en el log.
                ViewData["error"] = "Error al procesar la solicitud";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método RegistrarCitaPost() de la clase CitasController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/homePaciente.cshtml");
            }
        }

        /// <summary>
        /// Elimina una cita médica por su ID.
        /// </summary>
        /// <param name="id">ID de la cita a eliminar.</param>
        /// <returns>Redirección a la página principal del usuario paciente.</returns>
        [Authorize]
        [HttpGet]
        [Route("/privada/eliminar-cita/{id}")]
        public IActionResult eliminarCita(long id)
        {
            try
            {
                // Obtener la cita por ID.
                CitasDTO cita = _citaServicio.buscarPorId(id);

                if (cita != null)
                {
                    // Eliminar la cita y obtener la lista actualizada de citas del paciente.
                    _citaServicio.eliminar(id);
                    List<CitasDTO> citas = _citaServicio.ObtenerCitasDePaciente(cita.IdPacienteDTO);

                    if (citas != null && citas.Count > 0)
                    {
                        ViewBag.Cita = citas;
                    }

                    ViewData["eliminacionCorrecta"] = "La cita se ha eliminado correctamente";
                }

                // Registro en el log de la salida del método.
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método eliminarCita() de la clase CitasController. " + ViewData["eliminacionCorrecta"]);

                return RedirectToAction("HomeUser", "Paciente");
            }
            catch (Exception e)
            {
                // Manejar excepciones y registrar en el log.
                ViewData["error"] = "Error al procesar la solicitud";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método eliminarCita() de la clase CitasController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/homePaciente.cshtml");
            }
        }

        /// <summary>
        /// Cancela una cita médica por su ID.
        /// </summary>
        /// <param name="id">ID de la cita a cancelar.</param>
        /// <returns>Redirección a la página principal del usuario empleado.</returns>
        [Authorize]
        [HttpGet]
        [Route("/privada/cancelar-cita/{id}")]
        public IActionResult cancelarCita(long id)
        {
            try
            {
                // Obtener la cita por ID.
                CitasDTO cita = _citaServicio.buscarPorId(id);

                if (cita != null)
                {
                    // Cancelar la cita y obtener la lista actualizada de todas las citas.
                    _citaServicio.cancelarCita(id);
                    ViewBag.Citas = _citaServicio.buscarTodos();

                    ViewData["eliminacionCorrecta"] = "La cita se ha cancelado correctamente";
                }

                // Registro en el log de la salida del método.
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método cancelarCita() de la clase CitasController. " + ViewData["eliminacionCorrecta"]);

                return RedirectToAction("HomeEmpleado", "Paciente");
            }
            catch (Exception e)
            {
                // Manejar excepciones y registrar en el log.
                ViewData["error"] = "Error al procesar la solicitud";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método cancelarCita() de la clase CitasController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/homePaciente.cshtml");
            }
        }

        /// <summary>
        /// Completa una cita médica por su ID.
        /// </summary>
        /// <param name="id">ID de la cita a completar.</param>
        /// <returns>Redirección a la página principal del usuario empleado.</returns>
        [Authorize]
        [HttpGet]
        [Route("/privada/completar-cita/{id}")]
        public IActionResult completarCita(long id)
        {
            try
            {
                // Obtener la cita por ID.
                CitasDTO cita = _citaServicio.buscarPorId(id);

                if (cita != null)
                {
                    // Completar la cita y obtener la lista actualizada de todas las citas.
                    _citaServicio.completarCita(id);
                    ViewBag.Cita = _citaServicio.buscarTodos();

                    ViewData["eliminacionCorrecta"] = "La cita se ha completado correctamente";
                }

                // Registro en el log de la salida del método.
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método completarCita() de la clase CitasController. " + ViewData["completar Correcta"]);

                return RedirectToAction("HomeEmpleado", "Paciente");
            }
            catch (Exception e)
            {
                // Manejar excepciones y registrar en el log.
                ViewData["error"] = "Error al procesar la solicitud";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método completarCita() de la clase CitasController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/homePaciente.cshtml");
            }
        }
    }
}

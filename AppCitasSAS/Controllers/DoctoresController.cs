using AppCitasSAS.DTO;
using AppCitasSAS.Servicios.Interfaces;
using AppCitasSAS.Utils;
using DAL.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppCitasSAS.Controllers
{
    /// <summary>
    /// Controlador para la gestión de doctores.
    /// </summary>
    public class DoctoresController : Controller
    {
        private readonly IntfDoctorServicio _doctorServicio;
        private readonly IntfDoctorToDao _doctorToDao;
        private readonly IntfDoctorToDto _doctorToDto;
        private readonly IntfConsultaTurnoServicio _turnoServicio;

        /// <summary>
        /// Constructor que inicializa instancias de servicios e interfaces necesarios.
        /// </summary>
        public DoctoresController(IntfDoctorServicio doctorServicio, IntfDoctorToDao doctorToDao, IntfDoctorToDto doctorToDto, IntfConsultaTurnoServicio turnoServicio)
        {
            _doctorServicio = doctorServicio;
            _doctorToDao = doctorToDao;
            _doctorToDto = doctorToDto;
            _turnoServicio = turnoServicio;
        }

        /// <summary>
        /// Muestra el formulario para crear un nuevo doctor.
        /// </summary>
        /// <returns>Vista para la creación de doctores.</returns>
        [Authorize]
        [HttpGet]
        [Route("/privada/crear-doctor")]
        public IActionResult MostrarFormNuevoDoctor()
        {
            try
            {
                // Registro en el log de la entrada al método.
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método MostrarFormNuevaDoctor() de la clase DoctoresController");

                // Obtener la lista de turnos.
                List<ConsultaTurnoDTO> turnos = _turnoServicio.buscarTodos();

                // Crear un nuevo doctor y configurar ViewBag para la vista.
                DoctoresDTO nuevoDoctor = new DoctoresDTO();
                ViewBag.Turnos = turnos;

                // Registro en el log de la salida del método.
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método MostrarFormNuevaDoctor() de la clase DoctoresController");

                return View("~/Views/Home/crearDoctor.cshtml", nuevoDoctor);
            }
            catch (Exception e)
            {
                // Manejar excepciones y registrar en el log.
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método MostrarFormNuevaDoctor() de la clase DoctoresController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/homeEmpleado.cshtml");
            }
        }

        /// <summary>
        /// Registra un nuevo doctor.
        /// </summary>
        /// <param name="doctoresDTO">Datos del doctor a registrar.</param>
        /// <returns>Redirección a la página principal del usuario empleado.</returns>
        [Authorize]
        [HttpPost]
        [Route("/privada/crear-doctor")]
        public IActionResult RegistrarDoctorPost(DoctoresDTO doctoresDTO)
        {
            try
            {
                // Registro en el log de la entrada al método.
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método RegistrarDoctorPost() de la clase DoctoresController");

                // Registrar el nuevo doctor.
                _doctorServicio.registrar(doctoresDTO);

                return RedirectToAction("HomeEmpleado", "Paciente");
            }
            catch (Exception e)
            {
                // Manejar excepciones y registrar en el log.
                ViewData["error"] = "Error al procesar la solicitud";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método RegistrarDoctorPost() de la clase DoctoresController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/homeEmpleado.cshtml");
            }
        }

        /// <summary>
        /// Muestra el formulario para editar un doctor.
        /// </summary>
        /// <param name="id">ID del doctor a editar.</param>
        /// <returns>Vista para la edición de doctores.</returns>
        [Authorize]
        [HttpGet]
        [Route("/privada/editar-doctor/{id}")]
        public IActionResult MostrarFormularioEdicionDoctor(long id)
        {
            try
            {
                // Registro en el log de la entrada al método.
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método MostrarFormularioEdicionDoctor() de la clase DoctoresController");

                // Obtener la lista de turnos.
                List<ConsultaTurnoDTO> turnos = _turnoServicio.buscarTodos();

                // Obtener el doctor por ID.
                DoctoresDTO doctor = _doctorServicio.buscarPorId(id);

                ViewBag.Turnos = turnos;

                if (doctor == null)
                {
                    // Registro en el log de la salida del método si el doctor no se encuentra.
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método MostrarFormularioEdicionDoctor() de la clase DoctoresController. No se encontró al doctor con id " + id);
                    return View("~/Views/Home/homeEmpleado.cshtml");
                }

                // Registro en el log de la salida del método.
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método MostrarFormularioEdicionDoctor() de la clase DoctoresController.");

                return View("~/Views/Home/editarDoctor.cshtml", doctor);
            }
            catch (Exception e)
            {
                // Manejar excepciones y registrar en el log.
                ViewData["error"] = "Ocurrió un error al obtener el doctor para editar";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método MostrarFormularioEdicionDoctor() de la clase DoctoresController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/homeEmpleado.cshtml");
            }
        }

        /// <summary>
        /// Procesa el formulario de edición de un doctor.
        /// </summary>
        /// <param name="id">ID del doctor a editar.</param>
        /// <param name="nombreCompleto">Nuevo nombre completo del doctor.</param>
        /// <param name="especialidad">Nueva especialidad del doctor.</param>
        /// <param name="IdConsultaTurno">Nuevo ID del turno del doctor.</param>
        /// <returns>Redirección a la página principal del usuario empleado.</returns>
        [Authorize]
        [HttpPost]
        [Route("/privada/procesar-editarDoctor")]
        public IActionResult ProcesarFormularioEdicionDoctor(long id, string nombreCompleto, string especialidad, long IdConsultaTurno)
        {
            try
            {
                // Registro en el log de la entrada al método.
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método ProcesarFormularioEdicionDoctor() de la clase DoctoresController");

                // Obtener el doctor por ID y actualizar sus datos.
                DoctoresDTO doctor = _doctorServicio.buscarPorId(id);
                doctor.NombreCompletoDoctor = nombreCompleto;
                doctor.EspecialidadDoctor = especialidad;
                doctor.IdConsultaTurno = IdConsultaTurno;

                // Actualizar el doctor.
                _doctorServicio.actualizarDoctor(doctor);

                ViewData["EdicionCorrecta"] = "El doctor se ha editado correctamente";

                // Registro en el log de la salida del método.
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método ProcesarFormularioEdicionDoctor() de la clase DoctoresController. " + ViewData["EdicionCorrecta"]);

                return RedirectToAction("HomeEmpleado", "Paciente");
            }
            catch (Exception e)
            {
                // Manejar excepciones y registrar en el log.
                ViewData["Error"] = "Ocurrió un error al editar el doctor";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método ProcesarFormularioEdicionDoctorc() de la clase DoctoresController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/homeEmpleado.cshtml");
            }
        }

        /// <summary>
        /// Elimina un doctor por su ID.
        /// </summary>
        /// <param name="id">ID del doctor a eliminar.</param>
        /// <returns>Redirección a la página principal del usuario empleado.</returns>
        [Authorize]
        [HttpGet]
        [Route("/privada/eliminar-doctor/{id}")]
        public IActionResult eliminarDoctor(long id)
        {
            try
            {
                // Obtener el doctor por ID.
                DoctoresDTO doctor = _doctorServicio.buscarPorId(id);

                if (doctor != null)
                {
                    // Eliminar el doctor y configurar ViewBag.
                    _doctorServicio.eliminar(id);
                    ViewBag.Doctores = doctor;

                    ViewData["eliminacionCorrecta"] = "El doctor se ha eliminado correctamente";
                }

                // Registro en el log de la salida del método.
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método eliminarDoctor() de la clase DoctoresController. " + ViewData["eliminacionCorrecta"]);

                return RedirectToAction("HomeEmpleado", "Paciente");
            }
            catch (Exception e)
            {
                // Manejar excepciones y registrar en el log.
                ViewData["error"] = "Ocurrió un error al eliminar el doctor";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método eliminarDoctor() de la clase DoctoresController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/homeEmpleado.cshtml");
            }
        }
    }
}

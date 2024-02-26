using AppCitasSAS.DTO;
using AppCitasSAS.Servicios.Interfaces;
using AppCitasSAS.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppCitasSAS.Controllers
{
    public class TurnosController : Controller
    {
        private readonly IntfConsultaTurnoServicio _turnoServicio;
        private readonly IntfConsultaTurnoToDao _turnoToDao;
        private readonly IntfConsultaTurnoToDto _turnoToDto;

        public TurnosController (IntfConsultaTurnoServicio turnoServicio, IntfConsultaTurnoToDao turnoToDao, IntfConsultaTurnoToDto turnoToDto)
        {
            _turnoServicio = turnoServicio;
            _turnoToDao = turnoToDao;
            _turnoToDto = turnoToDto;
        }

        [Authorize]
        [HttpGet]
        [Route("/privada/crear-turno")]
        public IActionResult MostrarFormNuevoTurno()
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método MostrarFormNuevoTurno() de la clase TurnosController");

                ConsultaTurnoDTO nuevoTurno = new ConsultaTurnoDTO();

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método MostrarFormNuevoTurno() de la clase TurnosController");
                return View("~/Views/Home/crearTurno.cshtml", nuevoTurno);
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método MostrarFormNuevoTurno() de la clase TurnosController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/homeEmpleado.cshtml");
            }
        }

        [Authorize]
        [HttpPost]
        [Route("/privada/crear-turno")]
        public IActionResult RegistrarTurnoPost(ConsultaTurnoDTO consultaTurnoDTO)
        {
            try
            {

                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método RegistrarTurnoPost() de la clase TurnosController");

                _turnoServicio.registrar(consultaTurnoDTO);

                return RedirectToAction("HomeEmpleado", "Paciente");
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método RegistrarTurnoPost() de la clase TurnosController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/homeEmpleado.cshtml");
            }
        }


        [Authorize]
        [HttpGet]
        [Route("/privada/editar-turno/{id}")]
        public IActionResult MostrarFormularioEdicionTurno(long id)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método MostrarFormularioEdicionTurno() de la clase TurnosController");

                ConsultaTurnoDTO turno = _turnoServicio.buscarPorId(id);

                if (turno == null)
                {
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método MostrarFormularioEdicionTurno() de la clase TurnosController. No se encontró el turno con id " + id);
                    return View("~/Views/Home/homeEmpleado.cshtml");
                }
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método MostrarFormularioEdicionTurno() de la clase TurnosController.");
                return View("~/Views/Home/editarTurno.cshtml", turno);
            }
            catch (Exception e)
            {
                ViewData["error"] = "Ocurrió un error al obtener el turno para editar";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método MostrarFormularioEdicionTurno() de la clase TurnosController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/homeEmpleado.cshtml");
            }
        }

        [Authorize]
        [HttpPost]
        [Route("/privada/procesar-editarTurno")]
        public IActionResult ProcesarFormularioEdicionTurno(long id, int numConsulta, TimeOnly tramoInicio, TimeOnly tramoFin)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método ProcesarFormularioEdicionTurno() de la clase TurnosController");

                ConsultaTurnoDTO turno = _turnoServicio.buscarPorId(id);
                turno.NumConsulta = numConsulta;
                turno.TramoHoraTurnoInicio = tramoInicio;
                turno.TramoHoraTurnoFin = tramoFin;


                _turnoServicio.actualizarTurno(turno);

                ViewData["EdicionCorrecta"] = "El turno se ha editado correctamente";

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método ProcesarFormularioEdicionTurno() de la clase TurnosController. " + ViewData["EdicionCorrecta"]);

                return RedirectToAction("HomeEmpleado", "Paciente");
            }
            catch (Exception e)
            {
                ViewData["Error"] = "Ocurrió un error al editar el turno";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método ProcesarFormularioEdicionTurno() de la clase TurnosController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/homeEmpleado.cshtml");
            }
        }



        [Authorize]
        [HttpGet]
        [Route("/privada/eliminar-turno/{id}")]
        public IActionResult eliminarTurno(long id)
        {

            ConsultaTurnoDTO turno = _turnoServicio.buscarPorId(id);


            if (turno != null)
            {
                _turnoServicio.eliminar(id);
                ViewBag.Turnos = turno;

                ViewData["eliminacionCorrecta"] = "El turno se ha eliminado correctamente";
            }
            EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método eliminarTurno() de la clase DoctoresController. " + ViewData["eliminacionCorrecta"]);
            return RedirectToAction("HomeEmpleado", "Paciente");
        }
    }
}

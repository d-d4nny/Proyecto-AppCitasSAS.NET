using AppCitasSAS.DTO;
using AppCitasSAS.Servicios.Interfaces;
using AppCitasSAS.Utils;
using DAL.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppCitasSAS.Controllers
{
	public class DoctoresController : Controller
	{
		
		private readonly IntfDoctorServicio _doctorServicio;
		private readonly IntfDoctorToDao _doctorToDao;
		private readonly IntfDoctorToDto _doctorToDto;
		private readonly IntfConsultaTurnoServicio _turnoServicio;

		public DoctoresController (IntfDoctorServicio doctorServicio, IntfDoctorToDao doctorToDao, IntfDoctorToDto doctorToDto, IntfConsultaTurnoServicio turnoServicio)
		{
			_doctorServicio = doctorServicio;
			_doctorToDao = doctorToDao;
			_doctorToDto = doctorToDto;
			_turnoServicio = turnoServicio;
		}

		[Authorize]
		[HttpGet]
		[Route("/privada/crear-doctor")]
		public IActionResult MostrarFormNuevoDoctor()
		{
			try
			{
				EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método MostrarFormNuevaDoctor() de la clase DoctoresController");

				List<ConsultaTurnoDTO> turnos = _turnoServicio.buscarTodos();

				DoctoresDTO nuevoDoctor = new DoctoresDTO();

				ViewBag.Turnos = turnos;

				EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método MostrarFormNuevaDoctor() de la clase DoctoresController");
				return View("~/Views/Home/crearDoctor.cshtml", nuevoDoctor);
			}
			catch (Exception e)
			{
				ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
				EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método MostrarFormNuevaDoctor() de la clase DoctoresController: " + e.Message + e.StackTrace);
				return View("~/Views/Home/homeEmpleado.cshtml");
			}
		}

		[Authorize]
		[HttpPost]
		[Route("/privada/crear-doctor")]
		public IActionResult RegistrarDoctorPost(DoctoresDTO doctoresDTO)
		{
			try
			{

				EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método RegistrarDoctorPost() de la clase DoctoresController");

				_doctorServicio.registrar(doctoresDTO);

				return RedirectToAction("HomeEmpleado", "Paciente");
			}
			catch (Exception e)
			{
				ViewData["error"] = "Error al procesar la solicitud";
				EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método RegistrarDoctorPost() de la clase DoctoresController: " + e.Message + e.StackTrace);
				return View("~/Views/Home/homeEmpleado.cshtml");
			}
		}


		[Authorize]
		[HttpGet]
		[Route("/privada/editar-doctor/{id}")]
		public IActionResult MostrarFormularioEdicionDoctor(long id)
		{
			try
			{
				EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método MostrarFormularioEdicionDoctor() de la clase DoctoresController");

				List<ConsultaTurnoDTO> turnos = _turnoServicio.buscarTodos();

				DoctoresDTO doctor = _doctorServicio.buscarPorId(id);

				ViewBag.Turnos = turnos;

				if (doctor == null)
				{
					EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método MostrarFormularioEdicionDoctor() de la clase DoctoresController. No se encontró al doctor con id " + id);
					return View("~/Views/Home/homeEmpleado.cshtml");
				}
				EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método MostrarFormularioEdicionDoctor() de la clase DoctoresController.");
				return View("~/Views/Home/editarDoctor.cshtml", doctor);
			}
			catch (Exception e)
			{
				ViewData["error"] = "Ocurrió un error al obtener el doctor para editar";
				EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método MostrarFormularioEdicionDoctor() de la clase DoctoresController: " + e.Message + e.StackTrace);
				return View("~/Views/Home/homeEmpleado.cshtml");
			}
		}

		[Authorize]
		[HttpPost]
		[Route("/privada/procesar-editarDoctor")]
		public IActionResult ProcesarFormularioEdicionDoctor(long id, string nombreCompleto, string especialidad, long IdConsultaTurno)
		{
			try
			{
				EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método ProcesarFormularioEdicionDoctor() de la clase DoctoresController");

				DoctoresDTO doctor = _doctorServicio.buscarPorId(id);
				doctor.NombreCompletoDoctor = nombreCompleto;
				doctor.EspecialidadDoctor = especialidad;
				doctor.IdConsultaTurno = IdConsultaTurno;


				_doctorServicio.actualizarDoctor(doctor);

				ViewData["EdicionCorrecta"] = "El doctor se ha editado correctamente";

				EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método ProcesarFormularioEdicionDoctor() de la clase DoctoresController. " + ViewData["EdicionCorrecta"]);

				return RedirectToAction("HomeEmpleado", "Paciente");
			}
			catch (Exception e)
			{
				ViewData["Error"] = "Ocurrió un error al editar el doctor";
				EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método ProcesarFormularioEdicionDoctorc() de la clase DoctoresController: " + e.Message + e.StackTrace);
				return View("~/Views/Home/homeEmpleado.cshtml");
			}
		}



		[Authorize]
		[HttpGet]
		[Route("/privada/eliminar-doctor/{id}")]
		public IActionResult eliminarDoctor(long id)
		{

			DoctoresDTO doctor = _doctorServicio.buscarPorId(id);


			if (doctor != null)
			{
				_doctorServicio.eliminar(id);
				ViewBag.Doctores = doctor;

				ViewData["eliminacionCorrecta"] = "El doctor se ha eliminado correctamente";
			}
			EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método eliminarDoctor() de la clase DoctoresController. " + ViewData["eliminacionCorrecta"]);
			return RedirectToAction("HomeEmpleado", "Paciente");
		}
	}
}

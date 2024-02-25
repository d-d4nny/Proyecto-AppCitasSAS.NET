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
    public class CitasController : Controller
    {

        private readonly IntfCitasServicio _citaServicio;
        private readonly IntfPacienteServicio _pacienteServicio;
        private readonly IntfDoctorServicio _doctorServicio;
        private readonly IntfCitasToDao _citaToDao;
        private readonly IntfCitasToDto _citaToDto;

        public CitasController(IntfCitasServicio citaServicio, IntfPacienteServicio pacienteServicio, IntfDoctorServicio doctorServicio, IntfCitasToDao citaToDao, IntfCitasToDto citaToDto)
        {
            _citaServicio = citaServicio;
            _pacienteServicio = pacienteServicio;
            _doctorServicio = doctorServicio;
            _citaToDao = citaToDao;
            _citaToDto = citaToDto;
        }

        [Authorize]
        [HttpGet]
        [Route("/privada/crear-cita")]
        public IActionResult MostrarFormNuevaCita()
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método MostrarFormNuevaCita() de la clase CitasController");

                List<DoctoresDTO> doctores = _doctorServicio.buscarTodos();

                PacienteDTO paciente = _pacienteServicio.buscarPorEmail(User.Identity.Name);


                CitasDTO nuevaCita = new CitasDTO();

                ViewBag.Doctores = doctores;
                ViewBag.Pacientes = paciente;

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método MostrarFormNuevaCita() de la clase CitasController");
                return View("~/Views/Home/crearCita.cshtml", nuevaCita);
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método MostrarFormNuevaCita() de la clase CitasController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/homePaciente.cshtml");
            }
        }

        [Authorize]
        [HttpPost]
        [Route("/privada/crear-cita")]
        public IActionResult RegistrarCitaPost(CitasDTO citaDTO)
        {
            try
            {

                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método RegistrarCitaPost() de la clase CitasController");

                PacienteDTO u = _pacienteServicio.buscarPorEmail(User.Identity.Name);

                citaDTO.IdPacienteDTO = u.IdPaciente;

                _citaServicio.registrar(citaDTO);

                return RedirectToAction("HomeUser", "Paciente");
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método RegistrarCitaPost() de la clase CitasController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/homePaciente.cshtml");
            }
        }

		[Authorize]
		[HttpGet]
		[Route("/privada/eliminar-cita/{id}")]
		public IActionResult eliminarCita(long idCita)
        {

            CitasDTO cita = _citaServicio.buscarPorId(idCita);


            if (cita != null)
            {
                _citaServicio.eliminar(idCita);
                List<CitasDTO> citas = _citaServicio.ObtenerCitasDePaciente(cita.IdPacienteDTO);
                if (citas != null && citas.Count > 0)
                {
					ViewBag.Cita = citas;
				}
                ViewData["eliminacionCorrecta"] = "La cita se ha eliminado correctamente";
            }
            EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método EliminarMoto() de la clase MisMotosController. " + ViewData["eliminacionCorrecta"]);
            return RedirectToAction("HomeUser", "Paciente");
        }


	}
}

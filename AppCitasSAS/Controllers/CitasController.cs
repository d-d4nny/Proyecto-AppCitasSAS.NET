using AppCitasSAS.DTO;
using AppCitasSAS.Servicios.Interfaces;
using AppCitasSAS.Utils;
using Microsoft.AspNetCore.Mvc;

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

        public IActionResult MostrarFormNuevaCita()
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método MostrarFormNuevaCita() de la clase CitasController");

                string emailDelPaciente = User.Identity.Name;
                PacienteDTO pacienteSesionActual = _pacienteServicio.buscarPorEmail(emailDelPaciente);
                CitasDTO nuevaCita = new CitasDTO();
                nuevaCita.IdPacienteDTO = pacienteSesionActual.IdPaciente;

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método MostrarFormNuevaCita() de la clase CitasController");
                return View("~/Views/Home/crarCita.cshtml", nuevaCita);
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método MostrarFormNuevaCita() de la clase CitasController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/homePaciente.cshtml");
            }
        }
    }
}

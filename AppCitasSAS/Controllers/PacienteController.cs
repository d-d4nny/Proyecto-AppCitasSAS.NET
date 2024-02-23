using AppCitasSAS.DTO;
using AppCitasSAS.Servicios.Interfaces;
using AppCitasSAS.Utils;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AppCitasSAS.Controllers
{
    public class PacienteController : Controller
    {

        private readonly IntfPacienteServicio _pacienteServicio;
        private readonly IntfCitasServicio _citaServicio;
        private readonly IntfCitasToDao _citaToDao;
        private readonly IntfCitasToDto _citaToDto;

        public PacienteController(IntfPacienteServicio pacienteServicio, IntfCitasServicio citaServicio, IntfCitasToDao citaToDao, IntfCitasToDto citasToDto)
        {
            _pacienteServicio = pacienteServicio;
            _citaServicio = citaServicio;
            _citaToDao = citaToDao;
            _citaToDto = citasToDto;
        }

        [HttpGet]
        [Route("/auth/registrar")]
        public IActionResult RegistrarGet()
        {

            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método RegistrarGet() de la clase PacienteController");

                PacienteDTO pacienteDTO = new PacienteDTO();
                return View("~/Views/Home/registro.cshtml", pacienteDTO);

            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método RegistrarGet() de la clase PacienteController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/registro.cshtml");
            }
        }

        [HttpPost]
        [Route("/auth/registrar")]
        public IActionResult RegistrarPost(PacienteDTO pacienteDTO)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método RegistrarPost() de la clase PacienteController");

                PacienteDTO nuevoPaciente = _pacienteServicio.registrar(pacienteDTO);

                if (nuevoPaciente.EmailPaciente == "EmailNoConfirmado")
                {
                    ViewData["EmailNoConfirmado"] = "Ya existe un usuario registrado con ese email pero con la cuenta sin verificar";
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método RegistrarPost() de la clase PacienteController. " + ViewData["EmailNoConfirmado"]);
                    return View("~/Views/Home/login.cshtml");

                }
                else if (nuevoPaciente.EmailPaciente == "EmailRepetido")
                {
                    ViewData["EmailRepetido"] = "Ya existe un usuario con ese email registrado en el sistema";
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método RegistrarPost() de la clase PacienteController. " + ViewData["EmailRepetido"]);
                    return View("~/Views/Home/registro.cshtml");
                }
                else
                {
                    ViewData["MensajeRegistroExitoso"] = "Registro del nuevo usuario OK";
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método RegistrarPost() de la clase PacienteController. " + ViewData["MensajeRegistroExitoso"]);
                    return View("~/Views/Home/login.cshtml");
                }


            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método RegistrarPost() de la clase  PacienteController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/registro.cshtml");
            }
        }

        [Authorize]
        [HttpGet]
        [Route("/privada/Pacientes")]
        public IActionResult HomeUser()
        {
			EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método HomeUser() de la clase PacienteController");

			PacienteDTO u = _pacienteServicio.buscarPorEmail(User.Identity.Name);

            List<CitasDTO> citas = _citaServicio.ObtenerCitasDePaciente(u.IdPaciente);

            ViewBag.Cita = citas;
			ViewBag.PacienteDTO = u;

			return View("~/Views/Home/homePaciente.cshtml", u);
		}


        [Authorize(Roles = "ROLE_ADMIN")]
        [HttpGet("/privada/Administracion")]
        public IActionResult HomeEmpleado()
        {
            //var citas = _citasServicio.BuscarTodos();
            //var doctores = _doctoresServicio.BuscarTodos();
            var pacientes = _pacienteServicio.buscarTodos();
            //var consultaTurnos = _consultaTurnoServicio.BuscarTodos();

            //ViewBag.Citas = citas;
            //ViewBag.Doctores = doctores;
            ViewBag.Pacientes = pacientes;
            //ViewBag.ConsultaTurnos = consultaTurnos;

            return View("~/Views/Home/homeEmpleado.cshtml");
        }


        [Authorize]
        [HttpGet]
        [Route("/privada/editar-usuario/{id}")]
        public IActionResult MostrarFormularioEdicion(long id)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método MostrarFormularioEdicion() de la clase PacienteController");

                PacienteDTO pacienteDTO = _pacienteServicio.buscarPorId(id);

                if (pacienteDTO == null)
                {
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método MostrarFormularioEdicion() de la clase PacienteController. No se encontró al usuario con id " + id);
                    return View("~/Views/Home/homePaciente.cshtml");
                }
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método MostrarFormularioEdicion() de la clase PacienteController.");
                return View("~/Views/Home/editarPaciente.cshtml", pacienteDTO);
            }
            catch (Exception e)
            {
                ViewData["error"] = "Ocurrió un error al obtener el usuario para editar";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método MostrarFormularioEdicion() de la clase PacienteController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/homePaciente.cshtml");
            }
        }

        [Authorize]
        [HttpPost]
        [Route("/privada/procesar-editar")]
        public IActionResult ProcesarFormularioEdicion(long id, string nombre, string dni, string telefono, string direccion, string rol, IFormFile foto)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método ProcesarFormularioEdicion() de la clase PacienteController");

                PacienteDTO pacienteDTO = _pacienteServicio.buscarPorId(id);
                pacienteDTO.NombreCompletoPaciente = nombre;
                pacienteDTO.TlfPaciente = telefono;
                pacienteDTO.DniPaciente = dni;
                pacienteDTO.DireccionPaciente = direccion;

                if (rol.Equals("Administrador"))
                {
                    pacienteDTO.RolPaciente = "ROLE_ADMIN";
                }
                else
                {
                    pacienteDTO.RolPaciente = rol;
                }

                if (foto != null && foto.Length > 0)
                {
                    byte[] fotoBytes;
                    using (var memoryStream = new MemoryStream())
                    {
                        foto.CopyTo(memoryStream);
                        fotoBytes = memoryStream.ToArray();
                    }
                    pacienteDTO.ProfilePicture = fotoBytes;
                }
                else
                {
                    PacienteDTO pacienteActualDTO = _pacienteServicio.buscarPorId(id);
                    byte[] fotoActual = pacienteActualDTO.ProfilePicture;
                    pacienteDTO.ProfilePicture = fotoActual;
                }

                _pacienteServicio.actualizarPaciente(pacienteDTO);

                ViewData["EdicionCorrecta"] = "El Usuario se ha editado correctamente";
                ViewBag.Usuarios = _pacienteServicio.buscarTodos();

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método ProcesarFormularioEdicion() de la clase PacienteController. " + ViewData["EdicionCorrecta"]);

                if (pacienteDTO.RolPaciente.Contains("ROLE_ADMIN"))
                {
                    return RedirectToAction("HomeEmpleado", "Paciente");
                }
                else
                {
                    return RedirectToAction("HomeUser", "Paciente");
                }
            }
            catch (Exception e)
            {
                ViewData["Error"] = "Ocurrió un error al editar el usuario";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método ProcesarFormularioEdicion() de la clase PacienteController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/dashboard.cshtml");
            }
        }


        [Authorize(Roles = "ROLE_ADMIN")]
        [HttpGet]
        [Route("/privada/eliminar/{id}")]
        public IActionResult EliminarPaciente(long id)
        {
            PacienteDTO paciente = _pacienteServicio.buscarPorId(id);
            List<PacienteDTO> pacientes = _pacienteServicio.buscarTodos();

            if (User.IsInRole("ROLE_ADMIN") && paciente.RolPaciente.Equals("ROLE_ADMIN"))
            {
                TempData["noSePuedeEliminar"] = "No se puede eliminar a un admin";
                TempData["usuarios"] = pacientes;
                return RedirectToAction("Administracion");
            }

            _pacienteServicio.eliminar(id);
            return RedirectToAction("Administracion");
        }
    }
}

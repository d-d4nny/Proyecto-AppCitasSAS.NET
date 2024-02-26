using AppCitasSAS.DTO;
using AppCitasSAS.Servicios.Interfaces;
using AppCitasSAS.Utils;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DAL.Entidades;

namespace AppCitasSAS.Controllers
{
    public class PacienteController : Controller
    {

        private readonly IntfPacienteServicio _pacienteServicio;
        private readonly IntfCitasServicio _citaServicio;
        private readonly IntfDoctorServicio _doctorServicio;
        private readonly IntfConsultaTurnoServicio _turnoServicio;

        public PacienteController(IntfPacienteServicio pacienteServicio, IntfCitasServicio citaServicio, IntfDoctorServicio doctorServicio, IntfConsultaTurnoServicio turnoServicio)
        {
            _pacienteServicio = pacienteServicio;
            _citaServicio = citaServicio;
            _doctorServicio = doctorServicio;
            _turnoServicio = turnoServicio;

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
			List<CitasDTO> citas = _citaServicio.buscarTodos();
            List<DoctoresDTO> doctores = _doctorServicio.buscarTodos();
            List<ConsultaTurnoDTO> turnos = _turnoServicio.buscarTodos();
            List<PacienteDTO> pacientes = _pacienteServicio.buscarTodos();

            ViewBag.Citas = citas;
            ViewBag.Doctores = doctores;
            ViewBag.Pacientes = pacientes;
            ViewBag.Turnos = turnos;

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
                PacienteDTO p = _pacienteServicio.buscarPorEmail(User.Identity.Name);

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

                if (p.RolPaciente.Contains("ROLE_ADMIN"))
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

			string emailUsuarioActual = User.Identity.Name;


			if (emailUsuarioActual == paciente.EmailPaciente)
			{
				ViewData["noTePuedesEliminar"] = "Un administrador no puede eliminarse a sí mismo";
				EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método EliminarUsuario() de la clase AdministracionUsuariosController. " + ViewData["noTePuedesEliminar"]);
				return RedirectToAction("HomeEmpleado");
			}
			else if (User.IsInRole("ROLE_ADMIN") && paciente.RolPaciente == "ROLE_ADMIN")
			{
				ViewData["noSePuedeEliminar"] = "No se puede eliminar al último administrador del sistema";
				EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método EliminarUsuario() de la clase AdministracionUsuariosController. " + ViewData["noSePuedeEliminar"]);
				return RedirectToAction("HomeEmpleado");
			}

			List<CitasDTO> citas = _citaServicio.ObtenerCitasDePaciente(id);
			foreach (CitasDTO cita in citas)
			{
				_citaServicio.eliminar(cita.IdCita); // Adjust this line based on your actual method to delete a cita
			}

			_pacienteServicio.eliminar(id);
            return RedirectToAction("HomeEmpleado");
        }

        [HttpPost]
        public IActionResult CerrarSesion()
        {
            EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método CerrarSesion() de la clase PacienteController");
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}

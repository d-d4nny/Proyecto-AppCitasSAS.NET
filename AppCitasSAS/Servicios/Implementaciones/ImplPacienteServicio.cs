using AppCitasSAS.DTO;
using AppCitasSAS.Servicios.Interfaces;
using AppCitasSAS.Utils;
using DAL.Entidades;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using System.Security.Cryptography;

namespace AppCitasSAS.Servicios.Implementaciones
{
    public class ImplPacienteServicio : IntfPacienteServicio
    {
        private readonly AppCitasSasContext _contexto;
        private readonly IntfEncriptar _servicioEncriptar;
        private readonly IntfPacienteToDao _convertirAdao;
        private readonly IntfPacienteToDto _convertirAdto;
        private readonly IntfEmailRecuperacion _servicioEmail;

        public ImplPacienteServicio(AppCitasSasContext contexto, IntfEncriptar servicioEncriptar, IntfPacienteToDao convertirAdao, IntfPacienteToDto convertirAdto, IntfEmailRecuperacion servicioEmail)
        {
            _contexto = contexto;
            _servicioEncriptar = servicioEncriptar;
            _convertirAdao = convertirAdao;
            _convertirAdto = convertirAdto;
            _servicioEmail = servicioEmail;
        }

        public PacienteDTO registrar(PacienteDTO pacienteDTO)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método registrarPaciente() de la clase PacienteServicioImpl");

                var pacienteExistente = _contexto.Pacientes.FirstOrDefault(u => u.EmailPaciente == pacienteDTO.EmailPaciente && !u.CuentaConfirmada);

                if (pacienteExistente != null)
                {
                    pacienteDTO.EmailPaciente = "EmailNoConfirmado";
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método registrarPaciente() de la clase PacienteServicioImpl");
                    return pacienteDTO;
                }

                var emailExistente = _contexto.Pacientes.FirstOrDefault(u => u.EmailPaciente == pacienteDTO.EmailPaciente && u.CuentaConfirmada);

                if (emailExistente != null)
                {
                    pacienteDTO.EmailPaciente = "EmailRepetido";
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método registrarPaciente() de la clase PacienteServicioImpl");
                    return pacienteDTO;
                }

                pacienteDTO.ContrasenaPaciente = _servicioEncriptar.Encriptar(pacienteDTO.ContrasenaPaciente);
                Paciente pacienteDao = _convertirAdao.pacienteToDao(pacienteDTO);
                pacienteDao.RolPaciente = "ROLE_USER";
                string token = generarToken();
                pacienteDao.TokenRecuperacion = token;

                _contexto.Pacientes.Add(pacienteDao);
                _contexto.SaveChanges();

                string nombreUsuario = pacienteDao.NombreCompletoPaciente;
                _servicioEmail.enviarEmailConfirmacion(pacienteDTO.EmailPaciente, nombreUsuario, token);

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método registrarPaciente() de la clase PacienteServicioImpl");

                return pacienteDTO;
            }
            catch (DbUpdateException dbe)
            {
                EscribirLog.escribirEnFicheroLog("[Error PacienteServicioImpl - registrarPaciente()] Error de persistencia al actualizar la bbdd: " + dbe.Message);
                return null;
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog("[Error PacienteServicioImpl - registrarPaciente()] Error al registrar un usuario: " + e.Message);
                return null;
            }


        }

        private string generarToken()
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método generarToken() de la clase PacienteServicioImpl");

                using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
                {
                    byte[] tokenBytes = new byte[30];
                    rng.GetBytes(tokenBytes);

                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método generarToken() de la clase PacienteServicioImpl");

                    return BitConverter.ToString(tokenBytes).Replace("-", "").ToLower();
                }
            }
            catch (ArgumentException ae)
            {
                EscribirLog.escribirEnFicheroLog("[Error PacienteServicioImpl -  generarToken()] Error al generar un token de usuario " + ae.Message);
                return null;
            }

        }


        public bool confirmarCuenta(string token)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método confirmarCuenta() de la clase PacienteServicioImpl");

                Paciente? pacienteExistente = _contexto.Pacientes.Where(u => u.TokenRecuperacion == token).FirstOrDefault();

                if (pacienteExistente != null && !pacienteExistente.CuentaConfirmada)
                {
                    // Entra en esta condición si el usuario existe y su cuenta no se ha confirmado
                    pacienteExistente.CuentaConfirmada = true;
                    pacienteExistente.TokenRecuperacion = null;
                    _contexto.Pacientes.Update(pacienteExistente);
                    _contexto.SaveChanges();
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método confirmarCuenta() de la clase PacienteServicioImpl. Cuenta confirmada OK.");

                    return true;
                }
                else
                {
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método confirmarCuenta() de la clase PacienteServicioImpl. La cuenta no existe o ya está confirmada");
                    return false;
                }
            }
            catch (ArgumentException ae)
            {
                EscribirLog.escribirEnFicheroLog("[Error PacienteServicioImpl - confirmarCuenta()] Error al confirmar la cuenta " + ae.Message);
                return false;
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog("[Error PacienteServicioImpl - confirmarCuenta()] Error al confirmar la cuenta " + e.Message);
                return false;
            }
        }


        public bool iniciarProcesoRecuperacion(string emailUsuario)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método iniciarProcesoRecuperacion() de la clase PacienteServicioImpl");

                Paciente? pacienteExistente = _contexto.Pacientes.FirstOrDefault(u => u.EmailPaciente == emailUsuario);

                if (pacienteExistente != null)
                {
                    // Generar el token y establecer la fecha de expiración
                    string token = generarToken();
                    DateTime fechaExpiracion = DateTime.Now.AddMinutes(1);

                    // Actualizar el usuario con el nuevo token y la fecha de expiración
                    pacienteExistente.TokenRecuperacion = token;
                    pacienteExistente.ExpiracionToken = fechaExpiracion;

                    // Actualizar el usuario en la base de datos
                    _contexto.Pacientes.Update(pacienteExistente);
                    _contexto.SaveChanges();

                    // Enviar el correo de recuperación
                    string nombreUsuario = pacienteExistente.NombreCompletoPaciente;
                    _servicioEmail.enviarEmailRecuperacion(emailUsuario, nombreUsuario, token);

                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método iniciarProcesoRecuperacion() de la clase PacienteServicioImpl.");

                    return true;
                }
                else
                {
                    EscribirLog.escribirEnFicheroLog("[INFO] El usuario con email " + emailUsuario + " no existe");
                    return false;
                }
            }
            catch (DbUpdateException dbe)
            {
                Console.WriteLine("[Error PacienteServicioImpl - iniciarProcesoRecuperacion()] Error de persistencia al actualizar la bbdd: " + dbe.Message);
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[Error PacienteServicioImpl - iniciarProcesoRecuperacion()] Error al iniciar el proceso de recuperación: " + ex.Message);
                return false;
            }
        }

        public PacienteDTO obtenerPacientePorToken(string token)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método obtenerPacientePorToken() de la clase PacienteServicioImpl");

                Paciente? pacienteExistente = _contexto.Pacientes.FirstOrDefault(u => u.TokenRecuperacion == token);

                if (pacienteExistente != null)
                {
                    PacienteDTO paciente = _convertirAdto.pacienteToDto(pacienteExistente);
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método obtenerPacientePorToken() de la clase PacienteServicioImpl.");
                    return paciente;
                }
                else
                {
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método obtenerPacientePorToken() de la clase PacienteServicioImpl. No existe el usuario con el token " + token);
                    return null;
                }
            }
            catch (ArgumentNullException e)
            {
                EscribirLog.escribirEnFicheroLog("[Error PacienteServicioImpl - obtenerPacientePorToken()] Error al obtener usuario por token " + e.Message);
                return null;
            }
        }

        public bool modificarContraseñaConToken(PacienteDTO paciente)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método modificarContraseñaConToken() de la clase PacienteServicioImpl");

                Paciente? pacienteExistente = _contexto.Pacientes.FirstOrDefault(u => u.TokenRecuperacion == paciente.Token);

                if (pacienteExistente != null)
                {
                    string nuevaContraseña = _servicioEncriptar.Encriptar(paciente.ContrasenaPaciente);
                    pacienteExistente.ContraseñaPaciente = nuevaContraseña;
                    pacienteExistente.TokenRecuperacion = null; // Se establece como null para invalidar el token ya consumido al cambiar la contraseña
                    _contexto.Pacientes.Update(pacienteExistente);
                    _contexto.SaveChanges();

                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método modificarContraseñaConToken() de la clase PacienteServicioImpl.");
                    return true;
                }
            }
            catch (DbUpdateException dbe)
            {
                EscribirLog.escribirEnFicheroLog("[Error PacienteServicioImpl - modificarContraseñaConToken()] Error de persistencia al actualizar la bbdd: " + dbe.Message);
            }
            catch (ArgumentNullException e)
            {
                EscribirLog.escribirEnFicheroLog("[Error PacienteServicioImpl - verificarCredenciales()] Error al modificar contraseña del usuario: " + e.Message);
                return false;
            }
            return false;
        }

        public bool verificarCredenciales(string emailUsuario, string claveUsuario)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método verificarCredenciales() de la clase PacienteServicioImpl");

                string contraseñaEncriptada = _servicioEncriptar.Encriptar(claveUsuario);
                Paciente? pacienteExistente = _contexto.Pacientes.FirstOrDefault(u => u.EmailPaciente == emailUsuario && u.ContraseñaPaciente == contraseñaEncriptada);
                if (pacienteExistente == null)
                {
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método verificarCredenciales() de la clase PacienteServicioImpl. Username no encontrado");
                    return false;
                }
                if (!pacienteExistente.CuentaConfirmada)
                {
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método verificarCredenciales() de la clase PacienteServicioImpl. El usuario no tiene la cuenta confirmada");
                    return false;
                }

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método verificarCredenciales() de la clase PacienteServicioImpl");

                return true;
            }
            catch (ArgumentNullException e)
            {
                EscribirLog.escribirEnFicheroLog("[Error PacienteServicioImpl - verificarCredenciales()] Error al comprobar las credenciales del usuario: " + e.Message);
                return false;
            }

        }

        public PacienteDTO obtenerUsuarioPorEmail(string email)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método obtenerUsuarioPorEmail() de la clase PacienteServicioImpl");

                PacienteDTO pacienteDTO = new PacienteDTO();
                var paciente = _contexto.Pacientes.FirstOrDefault(u => u.EmailPaciente == email);

                if (paciente != null)
                {
                    pacienteDTO = _convertirAdto.pacienteToDto(paciente);
                }

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método obtenerUsuarioPorEmail() de la clase PacienteServicioImpl");

                return pacienteDTO;
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("[Error PacienteServicioImpl - obtenerUsuarioPorEmail()] Error al obtener el usuario por email: " + e.Message);
                return null;
            }
        }

        public List<PacienteDTO> buscarTodos()
        {
            EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método obtenerTodosLosUsuarios() de la clase PacienteServicioImpl");

            return _convertirAdto.listPacienteToDto(_contexto.Pacientes.ToList());
        }

        public PacienteDTO buscarPorId(long id)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método buscarPorId() de la clase PacienteServicioImpl");

                Paciente? paciente = _contexto.Pacientes.FirstOrDefault(u => u.IdPaciente == id);
                if (paciente != null)
                {
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método buscarPorId() de la clase PacienteServicioImpl");

                    return _convertirAdto.pacienteToDto(paciente);
                }
            }
            catch (ArgumentException iae)
            {
                EscribirLog.escribirEnFicheroLog("[Error PacienteServicioImpl - buscarPorId()] Al buscar el usuario por su id " + iae.Message);
            }
            return null;
        }

        public void eliminar(long id)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método eliminar() de la clase PacienteServicioImpl");

                Paciente? paciente = _contexto.Pacientes.Find(id);
                if (paciente != null)
                {
                    _contexto.Pacientes.Remove(paciente);
                    _contexto.SaveChanges();
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método eliminar() de la clase PacienteServicioImpl. Usuario eliminado OK");
                }
            }
            catch (DbUpdateException dbe)
            {
                EscribirLog.escribirEnFicheroLog("[Error PacienteServicioImpl - eliminar()] Error de persistencia al eliminar el usuario por su id " + dbe.Message);
            }
        }

        public void actualizarPaciente(PacienteDTO pacienteModificado)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método actualizarUsuario() de la clase UsuarioServicioImpl");

                Paciente? pacienteActual = _contexto.Pacientes.Find(pacienteModificado.IdPaciente);

                if (pacienteActual != null)
                {
                    pacienteActual.NombreCompletoPaciente = pacienteModificado.NombreCompletoPaciente;
                    pacienteActual.TlfPaciente = pacienteModificado.TlfPaciente;
                    pacienteActual.RolPaciente = pacienteModificado.RolPaciente;
                    pacienteActual.ProfilePicture = pacienteModificado.ProfilePicture;

                    _contexto.Pacientes.Update(pacienteActual);
                    _contexto.SaveChanges();
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método actualizarUsuario() de la clase UsuarioServicioImpl. Usuario actualizado OK");
                }
                else
                {
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método actualizarUsuario() de la clase UsuarioServicioImpl. Usuario no encontrado");
                }
            }
            catch (DbUpdateException dbe)
            {
                EscribirLog.escribirEnFicheroLog("[Error UsuarioServicioImpl - actualizarUsuario()] Error de persistencia al modificar el usuario " + dbe.Message);
            }
        }


        public bool estaLaCuentaConfirmada(string emailPaciente)
        {

            EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método estaLaCuentaConfirmada() de la clase PacienteServicioImpl");

            try
            {
                Paciente pacienteExistente = _contexto.Pacientes.FirstOrDefault(p => p.EmailPaciente == emailPaciente);

                if (pacienteExistente != null && pacienteExistente.CuentaConfirmada)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog("[Error PacienteServicioImpl - EstaLaCuentaConfirmada()] Error al comprobar si la cuenta ya ha sido confirmada: " + e.Message);
            }

            return false;
        }


        public PacienteDTO buscarPorEmail(string emailPaciente)
        {
            EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método buscarPorEmail() de la clase PacienteServicioImpl");

            List<PacienteDTO> listaPacientes = buscarTodos();

            return listaPacientes.FirstOrDefault(p => p.EmailPaciente == emailPaciente);
        }


        public bool buscarPorDni(string dniPaciente)
        {
            EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método buscarPorDni() de la clase PacienteServicioImpl");

            return _contexto.Pacientes.Any(p => p.DniPaciente == dniPaciente);
        }
    }
}

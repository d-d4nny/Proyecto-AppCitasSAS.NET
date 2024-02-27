using AppCitasSAS.DTO;
using AppCitasSAS.Servicios.Interfaces;
using AppCitasSAS.Utils;
using DAL.Entidades;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Registra un nuevo paciente en el sistema.
        /// </summary>
        /// <param name="pacienteDTO">Datos del paciente a registrar.</param>
        /// <returns>Objeto PacienteDTO que representa al paciente registrado.</returns>
        public PacienteDTO registrar(PacienteDTO pacienteDTO)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método registrarPaciente() de la clase PacienteServicioImpl");

                // Verifica si hay un paciente existente con el mismo email sin confirmar
                var pacienteExistente = _contexto.Pacientes.FirstOrDefault(u => u.EmailPaciente == pacienteDTO.EmailPaciente && !u.CuentaConfirmada);

                if (pacienteExistente != null)
                {
                    pacienteDTO.EmailPaciente = "EmailNoConfirmado";
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método registrarPaciente() de la clase PacienteServicioImpl");
                    return pacienteDTO;
                }

                // Verifica si hay un paciente existente con el mismo email confirmado
                var emailExistente = _contexto.Pacientes.FirstOrDefault(u => u.EmailPaciente == pacienteDTO.EmailPaciente && u.CuentaConfirmada);

                if (emailExistente != null)
                {
                    pacienteDTO.EmailPaciente = "EmailRepetido";
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método registrarPaciente() de la clase PacienteServicioImpl");
                    return pacienteDTO;
                }

                // Encripta la contraseña y crea el objeto paciente para persistir en la base de datos
                pacienteDTO.ContrasenaPaciente = _servicioEncriptar.Encriptar(pacienteDTO.ContrasenaPaciente);
                Paciente pacienteDao = _convertirAdao.pacienteToDao(pacienteDTO);
                string token = generarToken();
                pacienteDao.TokenRecuperacion = token;

                // Persiste el paciente en la base de datos
                _contexto.Pacientes.Add(pacienteDao);
                _contexto.SaveChanges();

                // Envia el correo de confirmación
                string nombreUsuario = pacienteDao.NombreCompletoPaciente;
                _servicioEmail.enviarEmailConfirmacion(pacienteDTO.EmailPaciente, nombreUsuario, token);

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método registrarPaciente() de la clase PacienteServicioImpl");

                return pacienteDTO;
            }
            catch (DbUpdateException dbe)
            {
                // Manejo de excepciones específicas de la base de datos
                EscribirLog.escribirEnFicheroLog("[Error PacienteServicioImpl - registrarPaciente()] Error de persistencia al actualizar la bbdd: " + dbe.Message);
                return null;
            }
            catch (Exception e)
            {
                // Manejo de excepciones generales
                EscribirLog.escribirEnFicheroLog("[Error PacienteServicioImpl - registrarPaciente()] Error al registrar un usuario: " + e.Message);
                return null;
            }
        }

        // Métodos privados

        /// <summary>
        /// Genera un token aleatorio.
        /// </summary>
        /// <returns>Token generado.</returns>
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
                // Manejo de excepciones específicas del método generarToken
                EscribirLog.escribirEnFicheroLog("[Error PacienteServicioImpl -  generarToken()] Error al generar un token de usuario " + ae.Message);
                return null;
            }
        }



        /// <summary>
        /// Confirma la cuenta de un usuario utilizando el token proporcionado.
        /// </summary>
        /// <param name="token">Token de confirmación.</param>
        /// <returns>True si la cuenta se confirmó correctamente, False en caso contrario.</returns>
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
                // Manejo de excepciones específicas de confirmarCuenta
                EscribirLog.escribirEnFicheroLog("[Error PacienteServicioImpl - confirmarCuenta()] Error al confirmar la cuenta " + ae.Message);
                return false;
            }
            catch (Exception e)
            {
                // Manejo de excepciones generales
                EscribirLog.escribirEnFicheroLog("[Error PacienteServicioImpl - confirmarCuenta()] Error al confirmar la cuenta " + e.Message);
                return false;
            }
        }

        /// <summary>
        /// Inicia el proceso de recuperación de contraseña para un usuario.
        /// </summary>
        /// <param name="emailUsuario">Email del usuario.</param>
        /// <returns>True si el proceso se inició correctamente, False en caso contrario.</returns>
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
                // Manejo de excepciones específicas de la base de datos
                Console.WriteLine("[Error PacienteServicioImpl - iniciarProcesoRecuperacion()] Error de persistencia al actualizar la bbdd: " + dbe.Message);
                return false;
            }
            catch (Exception ex)
            {
                // Manejo de excepciones generales
                Console.WriteLine("[Error PacienteServicioImpl - iniciarProcesoRecuperacion()] Error al iniciar el proceso de recuperación: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Obtiene un paciente por su token de recuperación.
        /// </summary>
        /// <param name="token">Token de recuperación del paciente.</param>
        /// <returns>Objeto PacienteDTO que representa al paciente encontrado, o null si no se encuentra.</returns>
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
                // Manejo de excepciones específicas del método obtenerPacientePorToken
                EscribirLog.escribirEnFicheroLog("[Error PacienteServicioImpl - obtenerPacientePorToken()] Error al obtener usuario por token " + e.Message);
                return null;
            }
        }

        /// <summary>
        /// Modifica la contraseña de un paciente utilizando el token de recuperación.
        /// </summary>
        /// <param name="paciente">Datos del paciente con nueva contraseña y token.</param>
        /// <returns>True si la modificación fue exitosa, False en caso contrario.</returns>
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
                // Manejo de excepciones específicas de la base de datos
                EscribirLog.escribirEnFicheroLog("[Error PacienteServicioImpl - modificarContraseñaConToken()] Error de persistencia al actualizar la bbdd: " + dbe.Message);
            }
            catch (ArgumentNullException e)
            {
                // Manejo de excepciones generales
                EscribirLog.escribirEnFicheroLog("[Error PacienteServicioImpl - verificarCredenciales()] Error al modificar contraseña del usuario: " + e.Message);
                return false;
            }
            return false;
        }

        /// <summary>
        /// Verifica las credenciales de un paciente.
        /// </summary>
        /// <param name="emailUsuario">Email del paciente.</param>
        /// <param name="claveUsuario">Contraseña del paciente.</param>
        /// <returns>True si las credenciales son válidas, False en caso contrario.</returns>
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
                // Manejo de excepciones específicas del método verificarCredenciales
                EscribirLog.escribirEnFicheroLog("[Error PacienteServicioImpl - verificarCredenciales()] Error al comprobar las credenciales del usuario: " + e.Message);
                return false;
            }

        }

        /// <summary>
        /// Obtiene un paciente por su dirección de correo electrónico.
        /// </summary>
        /// <param name="email">Dirección de correo electrónico del paciente.</param>
        /// <returns>Objeto PacienteDTO que representa al paciente encontrado, o null si no se encuentra.</returns>
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

        /// <summary>
        /// Busca y devuelve todos los pacientes en formato de lista de DTO.
        /// </summary>
        /// <returns>Lista de objetos PacienteDTO que representan a los pacientes encontrados.</returns>
        public List<PacienteDTO> buscarTodos()
        {
            EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método obtenerTodosLosUsuarios() de la clase PacienteServicioImpl");

            return _convertirAdto.listPacienteToDto(_contexto.Pacientes.ToList());
        }

        /// <summary>
        /// Busca un paciente por su ID.
        /// </summary>
        /// <param name="id">ID del paciente a buscar.</param>
        /// <returns>Objeto PacienteDTO que representa al paciente encontrado, o null si no se encuentra.</returns>
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
                // Manejo de excepciones específicas del método buscarPorId
                EscribirLog.escribirEnFicheroLog("[Error PacienteServicioImpl - buscarPorId()] Al buscar el usuario por su id " + iae.Message);
            }
            return null;
        }

        /// <summary>
        /// Elimina un paciente por su ID.
        /// </summary>
        /// <param name="id">ID del paciente a eliminar.</param>
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
                // Manejo de excepciones específicas de la base de datos
                EscribirLog.escribirEnFicheroLog("[Error PacienteServicioImpl - eliminar()] Error de persistencia al eliminar el usuario por su id " + dbe.Message);
            }
        }

        /// <summary>
        /// Actualiza la información de un paciente.
        /// </summary>
        /// <param name="pacienteModificado">Datos actualizados del paciente.</param>
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
                    pacienteActual.DniPaciente = pacienteModificado.DniPaciente;
                    pacienteActual.DireccionPaciente = pacienteModificado.DireccionPaciente;
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
                // Manejo de excepciones específicas de la base de datos
                EscribirLog.escribirEnFicheroLog("[Error UsuarioServicioImpl - actualizarUsuario()] Error de persistencia al modificar el usuario " + dbe.Message);
            }
        }

        /// <summary>
        /// Verifica si la cuenta del paciente asociada al correo electrónico proporcionado está confirmada.
        /// </summary>
        /// <param name="emailPaciente">Correo electrónico del paciente.</param>
        /// <returns>True si la cuenta está confirmada, False de lo contrario.</returns>
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

        /// <summary>
        /// Busca y devuelve un paciente por su dirección de correo electrónico.
        /// </summary>
        /// <param name="emailPaciente">Dirección de correo electrónico del paciente.</param>
        /// <returns>Objeto PacienteDTO que representa al paciente encontrado, o null si no se encuentra.</returns>
        public PacienteDTO buscarPorEmail(string emailPaciente)
        {
            EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método buscarPorEmail() de la clase PacienteServicioImpl");

            List<PacienteDTO> listaPacientes = buscarTodos();

            return listaPacientes.FirstOrDefault(p => p.EmailPaciente == emailPaciente);
        }

        /// <summary>
        /// Verifica si existe un paciente con el DNI proporcionado.
        /// </summary>
        /// <param name="dniPaciente">Número de DNI del paciente.</param>
        /// <returns>True si existe un paciente con el DNI, False de lo contrario.</returns>
        public bool buscarPorDni(string dniPaciente)
        {
            EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método buscarPorDni() de la clase PacienteServicioImpl");

            return _contexto.Pacientes.Any(p => p.DniPaciente == dniPaciente);
        }
    }
}

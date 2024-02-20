using AppCitasSAS.DTO;
using AppCitasSAS.Servicios.Interfaces;
using AppCitasSAS.Utils;
using DAL.Entidades;
using Microsoft.EntityFrameworkCore;
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

        public PacienteDTO registrarPaciente(PacienteDTO pacienteDTO)
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
    }
}

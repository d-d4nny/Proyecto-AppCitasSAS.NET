using AppCitasSAS.DTO;
using DAL.Entidades;
using System;
using System.Collections.Generic;

namespace AppCitasSAS.Servicios.Interfaces
{
    public interface IntfPacienteServicio
    {
        /// <summary>
        /// Registra un nuevo paciente en el sistema.
        /// </summary>
        /// <param name="pacienteDTO">DTO del paciente a registrar.</param>
        /// <returns>DTO del paciente registrado.</returns>
        public PacienteDTO registrar(PacienteDTO pacienteDTO);

        /// <summary>
        /// Busca un paciente por su identificador único.
        /// </summary>
        /// <param name="id">Identificador único del paciente.</param>
        /// <returns>DTO del paciente encontrado.</returns>
        public PacienteDTO buscarPorId(long id);

        /// <summary>
        /// Busca un paciente por su dirección de correo electrónico.
        /// </summary>
        /// <param name="emailPaciente">Dirección de correo electrónico del paciente.</param>
        /// <returns>DTO del paciente encontrado.</returns>
        public PacienteDTO buscarPorEmail(string emailPaciente);

        /// <summary>
        /// Verifica si existe un paciente con el DNI especificado.
        /// </summary>
        /// <param name="dni">DNI del paciente.</param>
        /// <returns>True si existe un paciente con el DNI especificado, False en caso contrario.</returns>
        public bool buscarPorDni(string dni);

        /// <summary>
        /// Busca todos los pacientes registrados en el sistema.
        /// </summary>
        /// <returns>Lista de DTOs de pacientes.</returns>
        public List<PacienteDTO> buscarTodos();

        /// <summary>
        /// Actualiza la información de un paciente en el sistema.
        /// </summary>
        /// <param name="pacienteModificado">DTO del paciente con la información actualizada.</param>
        public void actualizarPaciente(PacienteDTO pacienteModificado);

        /// <summary>
        /// Obtiene un paciente utilizando un token de acceso.
        /// </summary>
        /// <param name="token">Token de acceso del paciente.</param>
        /// <returns>DTO del paciente encontrado.</returns>
        public PacienteDTO obtenerPacientePorToken(string token);

        /// <summary>
        /// Inicia el proceso de recuperación de contraseña para un paciente.
        /// </summary>
        /// <param name="emailPaciente">Dirección de correo electrónico del paciente.</param>
        /// <returns>True si se inicia el proceso correctamente, False en caso contrario.</returns>
        public bool iniciarProcesoRecuperacion(string emailPaciente);

        /// <summary>
        /// Modifica la contraseña de un paciente utilizando un token de acceso.
        /// </summary>
        /// <param name="paciente">DTO del paciente con la nueva contraseña y el token de acceso.</param>
        /// <returns>True si se modifica la contraseña correctamente, False en caso contrario.</returns>
        public bool modificarContraseñaConToken(PacienteDTO paciente);

        /// <summary>
        /// Elimina un paciente del sistema por su identificador único.
        /// </summary>
        /// <param name="id">Identificador único del paciente a eliminar.</param>
        public void eliminar(long id);

        /// <summary>
        /// Confirma la cuenta de un paciente utilizando un token de confirmación.
        /// </summary>
        /// <param name="token">Token de confirmación.</param>
        /// <returns>True si se confirma la cuenta correctamente, False en caso contrario.</returns>
        public bool confirmarCuenta(string token);

        /// <summary>
        /// Verifica si la cuenta de un paciente está confirmada.
        /// </summary>
        /// <param name="email">Dirección de correo electrónico del paciente.</param>
        /// <returns>True si la cuenta está confirmada, False en caso contrario.</returns>
        public bool estaLaCuentaConfirmada(string email);

        /// <summary>
        /// Verifica las credenciales de inicio de sesión de un paciente.
        /// </summary>
        /// <param name="emailPaciente">Dirección de correo electrónico del paciente.</param>
        /// <param name="clavePaciente">Clave de inicio de sesión del paciente.</param>
        /// <returns>True si las credenciales son válidas, False en caso contrario.</returns>
        public bool verificarCredenciales(string emailPaciente, string clavePaciente);
    }
}

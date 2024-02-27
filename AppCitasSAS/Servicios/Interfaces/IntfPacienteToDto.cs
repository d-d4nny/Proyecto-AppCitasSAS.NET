using AppCitasSAS.DTO;
using DAL.Entidades;
using System.Collections.Generic;

namespace AppCitasSAS.Servicios.Interfaces
{
    public interface IntfPacienteToDto
    {
        /// <summary>
        /// Convierte una entidad de paciente a un DTO de paciente.
        /// </summary>
        /// <param name="u">Entidad de paciente a convertir.</param>
        /// <returns>DTO de paciente correspondiente.</returns>
        public PacienteDTO pacienteToDto(Paciente u);

        /// <summary>
        /// Convierte una lista de entidades de pacientes a una lista de DTOs de pacientes.
        /// </summary>
        /// <param name="listaPaciente">Lista de entidades de pacientes a convertir.</param>
        /// <returns>Lista de DTOs de pacientes correspondientes.</returns>
        public List<PacienteDTO> listPacienteToDto(List<Paciente> listaPaciente);
    }
}

using AppCitasSAS.DTO;
using DAL.Entidades;
using System.Collections.Generic;

namespace AppCitasSAS.Servicios.Interfaces
{
    public interface IntfPacienteToDao
    {
        /// <summary>
        /// Convierte un DTO de paciente a una entidad de paciente para el acceso a datos.
        /// </summary>
        /// <param name="pacienteDTO">DTO del paciente a convertir.</param>
        /// <returns>Entidad de paciente correspondiente.</returns>
        public Paciente pacienteToDao(PacienteDTO pacienteDTO);

        /// <summary>
        /// Convierte una lista de DTOs de pacientes a una lista de entidades de pacientes para el acceso a datos.
        /// </summary>
        /// <param name="listaPacienteDTO">Lista de DTOs de pacientes a convertir.</param>
        /// <returns>Lista de entidades de pacientes correspondientes.</returns>
        public List<Paciente> listPacienteToDao(List<PacienteDTO> listaPacienteDTO);
    }
}

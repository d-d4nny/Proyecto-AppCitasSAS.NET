using AppCitasSAS.DTO;
using DAL.Entidades;
using System.Collections.Generic;

namespace AppCitasSAS.Servicios.Interfaces
{
    public interface IntfDoctorToDao
    {
        /// <summary>
        /// Convierte un DTO de doctor a una entidad de doctor para el acceso a datos.
        /// </summary>
        /// <param name="doctoresDTO">DTO del doctor a convertir.</param>
        /// <returns>Entidad de doctor correspondiente.</returns>
        public Doctore doctoresToDao(DoctoresDTO doctoresDTO);

        /// <summary>
        /// Convierte una lista de DTOs de doctores a una lista de entidades de doctores para el acceso a datos.
        /// </summary>
        /// <param name="listaDoctoresDTO">Lista de DTOs de doctores a convertir.</param>
        /// <returns>Lista de entidades de doctores correspondientes.</returns>
        public List<Doctore> listDoctoresToDao(List<DoctoresDTO> listaDoctoresDTO);
    }
}

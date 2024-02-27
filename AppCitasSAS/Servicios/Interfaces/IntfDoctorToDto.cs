using AppCitasSAS.DTO;
using DAL.Entidades;
using System;
using System.Collections.Generic;

namespace AppCitasSAS.Servicios.Interfaces
{
    public interface IntfDoctorToDto
    {
        /// <summary>
        /// Convierte una entidad de doctor a un DTO de doctor.
        /// </summary>
        /// <param name="u">Entidad de doctor a convertir.</param>
        /// <returns>DTO de doctor correspondiente.</returns>
        public DoctoresDTO doctoresToDto(Doctore u);

        /// <summary>
        /// Convierte una lista de entidades de doctores a una lista de DTOs de doctores.
        /// </summary>
        /// <param name="listaDoctores">Lista de entidades de doctores a convertir.</param>
        /// <returns>Lista de DTOs de doctores correspondientes.</returns>
        public List<DoctoresDTO> listDoctoresToDto(List<Doctore> listaDoctores);
    }
}

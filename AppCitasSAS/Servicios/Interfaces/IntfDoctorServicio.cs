using AppCitasSAS.DTO;
using DAL.Entidades;
using System;
using System.Collections.Generic;

namespace AppCitasSAS.Servicios.Interfaces
{
    public interface IntfDoctorServicio
    {
        /// <summary>
        /// Registra un nuevo doctor en el sistema.
        /// </summary>
        /// <param name="doctoresDTO">DTO del doctor a registrar.</param>
        /// <returns>DTO del doctor registrado.</returns>
        public DoctoresDTO registrar(DoctoresDTO doctoresDTO);

        /// <summary>
        /// Actualiza la información de un doctor en el sistema.
        /// </summary>
        /// <param name="doctorModificado">DTO del doctor con la información actualizada.</param>
        public void actualizarDoctor(DoctoresDTO doctorModificado);

        /// <summary>
        /// Busca un doctor por su identificador único.
        /// </summary>
        /// <param name="id">Identificador único del doctor.</param>
        /// <returns>DTO del doctor encontrado.</returns>
        public DoctoresDTO buscarPorId(long id);

        /// <summary>
        /// Busca todos los doctores registrados en el sistema.
        /// </summary>
        /// <returns>Lista de DTOs de doctores.</returns>
        public List<DoctoresDTO> buscarTodos();

        /// <summary>
        /// Elimina un doctor del sistema por su identificador único.
        /// </summary>
        /// <param name="id">Identificador único del doctor a eliminar.</param>
        public void eliminar(long id);
    }
}

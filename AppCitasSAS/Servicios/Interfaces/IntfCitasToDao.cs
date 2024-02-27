using AppCitasSAS.DTO;
using DAL.Entidades;

namespace AppCitasSAS.Servicios.Interfaces
{
    public interface IntfCitasToDao
    {
        /// <summary>
        /// Convierte un objeto CitasDTO a un objeto de tipo Cita.
        /// </summary>
        /// <param name="citasDTO">Objeto CitasDTO a convertir.</param>
        /// <returns>Objeto de tipo Cita.</returns>
        public Cita citasToDao(CitasDTO citasDTO);

        /// <summary>
        /// Convierte una lista de objetos CitasDTO a una lista de objetos de tipo Cita.
        /// </summary>
        /// <param name="listaCitasDTO">Lista de objetos CitasDTO a convertir.</param>
        /// <returns>Lista de objetos de tipo Cita.</returns>
        public List<Cita> listCitasToDao(List<CitasDTO> listaCitasDTO);
    }
}

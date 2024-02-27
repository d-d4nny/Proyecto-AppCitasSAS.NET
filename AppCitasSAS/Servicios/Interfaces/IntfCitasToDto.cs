using AppCitasSAS.DTO;
using DAL.Entidades;

namespace AppCitasSAS.Servicios.Interfaces
{
    public interface IntfCitasToDto
    {
        /// <summary>
        /// Convierte un objeto de tipo Cita a un objeto CitasDTO.
        /// </summary>
        /// <param name="u">Objeto de tipo Cita a convertir.</param>
        /// <returns>Objeto CitasDTO resultante de la conversión.</returns>
        public CitasDTO citasToDto(Cita u);

        /// <summary>
        /// Convierte una lista de objetos de tipo Cita a una lista de objetos CitasDTO.
        /// </summary>
        /// <param name="listaCitas">Lista de objetos de tipo Cita a convertir.</param>
        /// <returns>Lista de objetos CitasDTO resultante de la conversión.</returns>
        public List<CitasDTO> listCitasToDto(List<Cita> listaCitas);
    }
}

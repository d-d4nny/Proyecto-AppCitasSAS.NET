using AppCitasSAS.DTO;
using DAL.Entidades;

namespace AppCitasSAS.Servicios.Interfaces
{
    public interface IntfConsultaTurnoToDto
    {
        /// <summary>
        /// Convierte un objeto de tipo ConsultasTurno a un objeto de tipo ConsultaTurnoDTO.
        /// </summary>
        /// <param name="u">Objeto ConsultasTurno a convertir.</param>
        /// <returns>Objeto ConsultaTurnoDTO resultante de la conversión.</returns>
        public ConsultaTurnoDTO consultaTurnoToDto(ConsultasTurno u);

        /// <summary>
        /// Convierte una lista de objetos de tipo ConsultasTurno a una lista de objetos ConsultaTurnoDTO.
        /// </summary>
        /// <param name="listaConsultaTurno">Lista de objetos ConsultasTurno a convertir.</param>
        /// <returns>Lista de objetos ConsultaTurnoDTO resultante de la conversión.</returns>
        public List<ConsultaTurnoDTO> listConsultaTurnoToDto(List<ConsultasTurno> listaConsultaTurno);
    }
}

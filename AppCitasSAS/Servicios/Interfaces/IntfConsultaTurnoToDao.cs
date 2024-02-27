using AppCitasSAS.DTO;
using DAL.Entidades;

namespace AppCitasSAS.Servicios.Interfaces
{
    public interface IntfConsultaTurnoToDao
    {
        /// <summary>
        /// Convierte un objeto de tipo ConsultaTurnoDTO a un objeto de tipo ConsultasTurno.
        /// </summary>
        /// <param name="consultaTurnoDTO">Objeto ConsultaTurnoDTO a convertir.</param>
        /// <returns>Objeto ConsultasTurno resultante de la conversión.</returns>
        public ConsultasTurno consultaTurnoToDao(ConsultaTurnoDTO consultaTurnoDTO);

        /// <summary>
        /// Convierte una lista de objetos de tipo ConsultaTurnoDTO a una lista de objetos ConsultasTurno.
        /// </summary>
        /// <param name="listaConsultaTurnoDTO">Lista de objetos ConsultaTurnoDTO a convertir.</param>
        /// <returns>Lista de objetos ConsultasTurno resultante de la conversión.</returns>
        public List<ConsultasTurno> listConsultaTurnoToDao(List<ConsultaTurnoDTO> listaConsultaTurnoDTO);
    }
}

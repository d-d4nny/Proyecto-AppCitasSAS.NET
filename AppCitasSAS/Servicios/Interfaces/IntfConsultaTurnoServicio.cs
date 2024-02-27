using AppCitasSAS.DTO;
using DAL.Entidades;

namespace AppCitasSAS.Servicios.Interfaces
{
    public interface IntfConsultaTurnoServicio
    {
        /// <summary>
        /// Registra una nueva consulta de turno.
        /// </summary>
        /// <param name="consultaTurnoDTO">Objeto ConsultaTurnoDTO a registrar.</param>
        /// <returns>Objeto ConsultaTurnoDTO registrado.</returns>
        public ConsultaTurnoDTO registrar(ConsultaTurnoDTO consultaTurnoDTO);

        /// <summary>
        /// Actualiza la información de un turno.
        /// </summary>
        /// <param name="turnoModificado">Objeto ConsultaTurnoDTO con la información actualizada.</param>
        public void actualizarTurno(ConsultaTurnoDTO turnoModificado);

        /// <summary>
        /// Busca una consulta de turno por su ID.
        /// </summary>
        /// <param name="id">ID de la consulta de turno a buscar.</param>
        /// <returns>Objeto ConsultaTurnoDTO encontrado.</returns>
        public ConsultaTurnoDTO buscarPorId(long id);

        /// <summary>
        /// Obtiene todas las consultas de turno registradas.
        /// </summary>
        /// <returns>Lista de objetos ConsultaTurnoDTO.</returns>
        public List<ConsultaTurnoDTO> buscarTodos();

        /// <summary>
        /// Elimina una consulta de turno por su ID.
        /// </summary>
        /// <param name="id">ID de la consulta de turno a eliminar.</param>
        public void eliminar(long id);
    }
}

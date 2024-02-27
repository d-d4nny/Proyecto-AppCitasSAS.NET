using AppCitasSAS.DTO;
using DAL.Entidades;
using System.Runtime.InteropServices;

namespace AppCitasSAS.Servicios.Interfaces
{
    public interface IntfCitasServicio
    {
        /// <summary>
        /// Registra una nueva cita.
        /// </summary>
        /// <param name="citasDTO">Objeto CitasDTO con la información de la cita a registrar.</param>
        /// <returns>Objeto CitasDTO registrado.</returns>
        public CitasDTO registrar(CitasDTO citasDTO);

        /// <summary>
        /// Obtiene todas las citas de un paciente dado su ID.
        /// </summary>
        /// <param name="IdPaciente">ID del paciente.</param>
        /// <returns>Lista de objetos CitasDTO correspondientes a las citas del paciente.</returns>
        public List<CitasDTO> ObtenerCitasDePaciente(long IdPaciente);

        /// <summary>
        /// Busca una cita por su ID.
        /// </summary>
        /// <param name="id">ID de la cita a buscar.</param>
        /// <returns>Objeto CitasDTO correspondiente a la cita encontrada.</returns>
        public CitasDTO buscarPorId(long id);

        /// <summary>
        /// Obtiene todas las citas registradas.
        /// </summary>
        /// <returns>Lista de objetos CitasDTO correspondientes a todas las citas.</returns>
        public List<CitasDTO> buscarTodos();

        /// <summary>
        /// Elimina una cita por su ID.
        /// </summary>
        /// <param name="id">ID de la cita a eliminar.</param>
        public void eliminar(long id);

        /// <summary>
        /// Cancela una cita por su ID.
        /// </summary>
        /// <param name="idCita">ID de la cita a cancelar.</param>
        public void cancelarCita(long idCita);

        /// <summary>
        /// Completa una cita por su ID.
        /// </summary>
        /// <param name="idCita">ID de la cita a completar.</param>
        public void completarCita(long idCita);
    }
}

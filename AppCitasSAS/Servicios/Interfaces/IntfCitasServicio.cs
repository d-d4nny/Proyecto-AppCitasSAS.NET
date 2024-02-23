using AppCitasSAS.DTO;
using DAL.Entidades;
using System.Runtime.InteropServices;

namespace AppCitasSAS.Servicios.Interfaces
{
    public interface IntfCitasServicio
    {
        public CitasDTO registrar(CitasDTO citasDTO);

		public List<CitasDTO> ObtenerCitasDePaciente(long IdPaciente);

		public CitasDTO buscarPorId(long id);


        public List<CitasDTO> buscarTodos();


        public void eliminar(long id);


        public void cancelarCita(long idCita);


        public void completarCita(long idCita);
    }
}

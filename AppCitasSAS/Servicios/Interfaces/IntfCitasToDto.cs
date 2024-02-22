using AppCitasSAS.DTO;
using DAL.Entidades;

namespace AppCitasSAS.Servicios.Interfaces
{
    public interface IntfCitasToDto
    {

        public CitasDTO citasToDto(Cita u);

        public List<CitasDTO> listCitasToDto(List<Cita> listaCitas);
    }
}

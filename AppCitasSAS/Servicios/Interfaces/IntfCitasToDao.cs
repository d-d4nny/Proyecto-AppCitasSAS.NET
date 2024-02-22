using AppCitasSAS.DTO;
using DAL.Entidades;

namespace AppCitasSAS.Servicios.Interfaces
{
    public interface IntfCitasToDao
    {
        public Cita citasToDao(CitasDTO citasDTO);

        public List<Cita> listCitasToDao(List<CitasDTO> listaCitasDTO);
    }
}

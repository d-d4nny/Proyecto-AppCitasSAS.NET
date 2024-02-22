using AppCitasSAS.DTO;
using DAL.Entidades;

namespace AppCitasSAS.Servicios.Interfaces
{
    public interface IntfDoctorServicio
    {
        public DoctoresDTO registrar(DoctoresDTO doctoresDTO);


        public DoctoresDTO buscarPorId(long id);


        public List<DoctoresDTO> buscarTodos();


        public void eliminar(long id);
    }
}

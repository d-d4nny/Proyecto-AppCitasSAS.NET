using AppCitasSAS.DTO;
using DAL.Entidades;

namespace AppCitasSAS.Servicios.Interfaces
{
    public interface IntfConsultaTurnoServicio
    {
        public ConsultaTurnoDTO registrar(ConsultaTurnoDTO consultaTurnoDTO);


        public ConsultaTurnoDTO buscarPorId(long id);


        public List<ConsultaTurnoDTO> buscarTodos();


        public void eliminar(long id);
    }
}

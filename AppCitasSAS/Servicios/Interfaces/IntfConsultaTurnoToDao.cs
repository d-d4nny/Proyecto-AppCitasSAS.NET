using AppCitasSAS.DTO;
using DAL.Entidades;

namespace AppCitasSAS.Servicios.Interfaces
{
    public interface IntfConsultaTurnoToDao
    {
        public ConsultasTurno consultaTurnoToDao(ConsultaTurnoDTO consultaTurnoDTO);


        public List<ConsultasTurno> listConsultaTurnoToDao(List<ConsultaTurnoDTO> listaConsultaTurnoDTO);
    }
}
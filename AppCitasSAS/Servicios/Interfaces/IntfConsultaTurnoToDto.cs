using AppCitasSAS.DTO;
using DAL.Entidades;

namespace AppCitasSAS.Servicios.Interfaces
{
    public interface IntfConsultaTurnoToDto
    {
        public ConsultaTurnoDTO consultaTurnoToDto(ConsultasTurno u);


        public List<ConsultaTurnoDTO> listConsultaTurnoToDto(List<ConsultasTurno> listaConsultaTurno);
    }
}

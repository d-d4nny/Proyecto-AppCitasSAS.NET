using AppCitasSAS.DTO;
using DAL.Entidades;

namespace AppCitasSAS.Servicios.Interfaces
{
    public interface IntfDoctorToDto
    {

        public DoctoresDTO doctoresToDto(Doctore u);


        public List<DoctoresDTO> listDoctoresToDto(List<Doctore> listaDoctores);
    }
}

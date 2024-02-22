using AppCitasSAS.DTO;
using DAL.Entidades;

namespace AppCitasSAS.Servicios.Interfaces
{
    public interface IntfDoctorToDao
    {

        public Doctore doctoresToDao(DoctoresDTO doctoresDTO);


        public List<Doctore> listDoctoresToDao(List<DoctoresDTO> listaDoctoresDTO);
    }
}

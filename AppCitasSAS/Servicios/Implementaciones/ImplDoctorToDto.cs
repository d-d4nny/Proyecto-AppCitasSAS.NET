using AppCitasSAS.DTO;
using AppCitasSAS.Servicios.Interfaces;
using AppCitasSAS.Utils;
using DAL.Entidades;

namespace AppCitasSAS.Servicios.Implementaciones
{
    public class ImplDoctorToDto : IntfDoctorToDto
    {

        public DoctoresDTO doctoresToDto(Doctore u)
        {
            try
            {
                DoctoresDTO dto = new DoctoresDTO();

                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método doctorToDto() de la clase DoctorToDtoImpl");


                dto.IdDoctor = u.IdDoctor;
                dto.NombreCompletoDoctor = u.NombreCompletoDoctor;
                dto.EspecialidadDoctor = u.EspecialidadDoctor;
                dto.IdConsultaTurno = (long)u.IdConsultaTurno;

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método doctorToDto() de la clase DoctorToDtoImpl");
                return dto;
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog($"[ERROR DoctorToDtoImpl - doctorToDto()] - Error al convertir doctor DAO a doctorDTO (return null): {e}");
                return null;
            }
        }

        public List<DoctoresDTO> listDoctoresToDto(List<Doctore> listaDoctor)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método listDoctoresToDto() de la clase DoctorToDtoImpl");
                List<DoctoresDTO> listaDto = new List<DoctoresDTO>();
                foreach (Doctore u in listaDoctor) 
                {
                    listaDto.Add(doctoresToDto(u));
                }
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método listDoctoresToDto() de la clase DoctorToDtoImpl");
                return listaDto;
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog($"[ERROR DoctorToDtoImpl - listDoctoresToDto()] - Error al convertir lista de doctor DAO a lista de doctorDTO (return null): {e}");
            }
            return null;
        }
    }
}

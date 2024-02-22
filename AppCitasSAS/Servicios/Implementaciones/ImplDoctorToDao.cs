using AppCitasSAS.DTO;
using AppCitasSAS.Servicios.Interfaces;
using AppCitasSAS.Utils;
using DAL.Entidades;

namespace AppCitasSAS.Servicios.Implementaciones
{
    public class ImplDoctorToDao : IntfDoctorToDao
    {
        public Doctore doctoresToDao(DoctoresDTO doctoresDTO)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método doctoresToDao() de la clase ImplDoctorToDao");

                Doctore dao = new Doctore();

                dao.IdDoctor = doctoresDTO.IdDoctor;
                dao.NombreCompletoDoctor = doctoresDTO.NombreCompletoDoctor;
                dao.EspecialidadDoctor = doctoresDTO.EspecialidadDoctor;
                dao.IdConsultaTurno = doctoresDTO.IdConsultaTurno;

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método doctoresToDao() de la clase ImplDoctorToDao");
                return dao;
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog($"\n[ERROR ImplDoctorToDao - doctoresToDao()] - Al convertir DoctorDTO a DAO (return null): {e}");
            }

            return null;
        }

        public List<Doctore> listDoctoresToDao(List<DoctoresDTO> listaDoctoresDTO)
        {
            List<Doctore> listaDoctoresDao = new List<Doctore>();

            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método listDoctoresToDao() de la clase ImplDoctoresToDao");

                foreach (DoctoresDTO doctoresDTO in listaDoctoresDTO)
                {
                    listaDoctoresDao.Add(doctoresToDao(doctoresDTO));   
                }
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método listDoctoresToDao() de la clase ImplDoctoresToDao");
                return listaDoctoresDao;
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog($"\n[ERROR ImplDoctoresToDao - listDoctoresToDao()] - Al convertir lista de doctorDTO a lista de doctorDAO (return null): {e}");
            }
            return null;
        }
    }
}

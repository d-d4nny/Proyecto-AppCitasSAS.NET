using AppCitasSAS.DTO;
using AppCitasSAS.Servicios.Interfaces;
using AppCitasSAS.Utils;
using DAL.Entidades;

namespace AppCitasSAS.Servicios.Implementaciones
{
    public class ImplCitasToDao : IntfCitasToDao
    {
        public Cita citasToDao (CitasDTO citaDTO)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método citasToDao() de la clase ImplCitasToDao");

                Cita cita = new Cita();

                cita.IdCita = citaDTO.IdCita;
                cita.FechaCita = citaDTO.FechaCita; 
                cita.HoraCita = citaDTO.HoraCita;
                cita.MotivoCita = citaDTO.MotivoCita;
                cita.EstadoCita = citaDTO.EstadoCita;
                cita.IdPaciente = citaDTO.IdPacienteDTO;
                cita.IdDoctor = citaDTO.IdDoctoresDTO;

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método citasToDao() de la clase ImplCitasToDao");
                return cita;
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog($"\n[ERROR ImplCitasToDao - citasToDao()] - Al convertir CitaDTO a DAO (return null): {e}");
            }

            return null;
        }

        public List<Cita> listCitasToDao(List<CitasDTO> listaCitasDTO) 
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método listCitasToDao() de la clase ImplCitasToDao");

                List<Cita> listaCitaDao = new List<Cita>();
                foreach (CitasDTO citasDTO in listaCitasDTO)
                {
                    listaCitaDao.Add(citasToDao(citasDTO));
                }

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método listCitasToDao() de la clase ImplCitasToDao");

                return listaCitaDao;
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog($"\n[ERROR ImplCitasToDao - listCitasToDao()] - Al convertir lista de citasDTO a lista de citasDAO (return null): {e}");
            }
            return null;
        }
    }
}

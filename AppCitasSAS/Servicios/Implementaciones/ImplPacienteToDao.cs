using AppCitasSAS.DTO;
using AppCitasSAS.Servicios.Interfaces;
using AppCitasSAS.Utils;
using DAL.Entidades;

namespace AppCitasSAS.Servicios.Implementaciones
{
    public class ImplPacienteToDao : IntfPacienteToDao
    {
        public Paciente pacienteToDao(PacienteDTO pacienteDTO)
        {
            try
            {
                Paciente pacienteDao = new Paciente();

                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método pacienteToDao() de la clase ImplsPacienteToDao");

                pacienteDao.NombreCompletoPaciente = pacienteDTO.NombreCompletoPaciente;
                pacienteDao.DniPaciente = pacienteDTO.DniPaciente;
                pacienteDao.TlfPaciente = pacienteDTO.TlfPaciente;
                pacienteDao.EmailPaciente = pacienteDTO.EmailPaciente;
                pacienteDao.ContraseñaPaciente = pacienteDTO.ContrasenaPaciente;
                pacienteDao.GeneroPaciente = pacienteDTO.GeneroPaciente;
                pacienteDao.DireccionPaciente = pacienteDTO.DireccionPaciente;
                pacienteDao.RolPaciente = pacienteDTO.RolPaciente;
                pacienteDao.ProfilePicture = pacienteDTO.ProfilePicture;
                pacienteDao.CuentaConfirmada = pacienteDTO.CuentaConfirmada;
                pacienteDao.ExpiracionToken = pacienteDTO.ExpiracionToken;


                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método pacienteToDao() de la clase ImplsPacienteToDao");

                return pacienteDao;

            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog($"\n[ERROR ImplPacienteToDao - PacienteToDao()] - Al convertir pacienteDTO a pacienteDAO (return null): {e}");
                return null;
            }
        }

        public List<Paciente> listPacienteToDao(List<PacienteDTO> listaPacienteDTO)
        {
            List<Paciente> listaPacienteDao = new List<Paciente>();

            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método listPacienteToDao() de la clase ImplPacienteToDao");

                foreach (PacienteDTO pacienteDTO in listaPacienteDTO)
                {
                    listaPacienteDao.Add(pacienteToDao(pacienteDTO));
                }

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método listPacienteToDao() de la clase ImplPacienteToDao");

                return listaPacienteDao;

            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog($"\n[ERROR ImplPacienteToDao - ListPacienteToDao()] - Al convertir lista de pacienteDTO a lista de pacienteDAO (return null): {e}");
            }
            return null;
        }
    }
}

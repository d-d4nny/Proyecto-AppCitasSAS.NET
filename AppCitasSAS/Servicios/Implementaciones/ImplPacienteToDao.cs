using AppCitasSAS.DTO;
using AppCitasSAS.Servicios.Interfaces;
using AppCitasSAS.Utils;
using DAL.Entidades;

namespace AppCitasSAS.Servicios.Implementaciones
{
    public class ImplPacienteToDao : IntfPacienteToDao
    {
        /// <summary>
        /// Convierte un objeto PacienteDTO a un objeto Paciente.
        /// </summary>
        /// <param name="pacienteDTO">Objeto PacienteDTO a convertir.</param>
        /// <returns>Objeto Paciente convertido.</returns>
        public Paciente pacienteToDao(PacienteDTO pacienteDTO)
        {
            try
            {
                Paciente pacienteDao = new Paciente();

                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método pacienteToDao() de la clase ImplPacienteToDao");

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

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método pacienteToDao() de la clase ImplPacienteToDao");

                return pacienteDao;
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog($"\n[ERROR ImplPacienteToDao - PacienteToDao()] - Al convertir pacienteDTO a pacienteDAO (return null): {e}");
                return null;
            }
        }

        /// <summary>
        /// Convierte una lista de objetos PacienteDTO a una lista de objetos Paciente.
        /// </summary>
        /// <param name="listaPacienteDTO">Lista de PacienteDTO a convertir.</param>
        /// <returns>Lista de Paciente convertida.</returns>
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

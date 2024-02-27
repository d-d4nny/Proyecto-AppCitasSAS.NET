using AppCitasSAS.DTO;
using AppCitasSAS.Servicios.Interfaces;
using AppCitasSAS.Utils;
using DAL.Entidades;

namespace AppCitasSAS.Servicios.Implementaciones
{
    public class ImplPacienteToDto : IntfPacienteToDto
    {
        /// <summary>
        /// Convierte un objeto Paciente a un objeto PacienteDTO.
        /// </summary>
        /// <param name="u">Objeto Paciente a convertir.</param>
        /// <returns>Objeto PacienteDTO convertido.</returns>
        public PacienteDTO pacienteToDto(Paciente u)
        {
            try
            {
                PacienteDTO dto = new PacienteDTO();

                dto.IdPaciente = u.IdPaciente;
                dto.NombreCompletoPaciente = u.NombreCompletoPaciente;
                dto.DniPaciente = u.DniPaciente;
                dto.TlfPaciente = u.TlfPaciente;
                dto.EmailPaciente = u.EmailPaciente;
                dto.DireccionPaciente = u.DireccionPaciente;
                dto.RolPaciente = u.RolPaciente;
                dto.GeneroPaciente = u.GeneroPaciente;
                dto.ContrasenaPaciente = u.ContraseñaPaciente;
                dto.Token = u.TokenRecuperacion;
                dto.ExpiracionToken = u.ExpiracionToken;
                dto.CuentaConfirmada = u.CuentaConfirmada;

                if (u.ProfilePicture != null)
                {
                    dto.ProfilePicture = u.ProfilePicture;
                }

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método pacienteToDto() de la clase ImplPacienteToDto");
                return dto;
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog($"[ERROR PacienteToDtoImpl - PacienteToDto()] - Error al convertir paciente DAO a pacienteDTO (return null): {e}");
                return null;
            }
        }

        /// <summary>
        /// Convierte una lista de objetos Paciente a una lista de objetos PacienteDTO.
        /// </summary>
        /// <param name="listaPaciente">Lista de Paciente a convertir.</param>
        /// <returns>Lista de PacienteDTO convertida.</returns>
        public List<PacienteDTO> listPacienteToDto(List<Paciente> listaPaciente)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método listPacienteToDto() de la clase ImplPacienteToDto");
                List<PacienteDTO> listaDto = new List<PacienteDTO>();
                foreach (Paciente u in listaPaciente)
                {
                    listaDto.Add(pacienteToDto(u));
                }
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método listPacienteToDto() de la clase ImplPacienteToDto");
                return listaDto;
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog($"[ERROR PacienteToDtoImpl - ListPacienteToDto()] - Error al convertir lista de paciente DAO a lista de pacienteDTO (return null): {e}");
            }
            return null;
        }
    }
}

using AppCitasSAS.DTO;
using AppCitasSAS.Servicios.Interfaces;
using AppCitasSAS.Utils;
using DAL.Entidades;

namespace AppCitasSAS.Servicios.Implementaciones
{
    public class ImplPacienteToDto : IntfPacienteToDto
    {
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


                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método usuarioToDto() de la clase ConvertirAdtoImpl");
                return dto;
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog($"[ERROR PacienteToDtoImpl - PacienteToDto()] - Error al convertir paciente DAO a pacienteDTO (return null): {e}");
                return null;
            }
        }
           
        public List<PacienteDTO> listPacienteToDto(List<Paciente> listaPaciente)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método listaPacienteToDto() de la clase ImplPacienteToDto");
                List<PacienteDTO> listaDto = new List<PacienteDTO>();
                foreach (Paciente u in listaPaciente)
                {
                    listaDto.Add(pacienteToDto(u));
                }
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método listaPacienteToDto() de la clase ImplPacienteToDto");
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

using AppCitasSAS.DTO;
using DAL.Entidades;

namespace AppCitasSAS.Servicios.Interfaces
{
    public interface IntfPacienteToDto
    {
        public PacienteDTO pacienteToDto(Paciente u);

        public List<PacienteDTO> listPacienteToDto(List<Paciente> listaPaciente);
    }
}

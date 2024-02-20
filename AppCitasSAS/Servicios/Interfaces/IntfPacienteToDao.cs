using AppCitasSAS.DTO;
using DAL.Entidades;

namespace AppCitasSAS.Servicios.Interfaces
{
    public interface IntfPacienteToDao
    {
        public Paciente pacienteToDao(PacienteDTO pacienteDTO);

        public List<Paciente> listPacienteToDao(List<PacienteDTO> listaPacienteDTO);
    }
}

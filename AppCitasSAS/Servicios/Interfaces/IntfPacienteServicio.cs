using AppCitasSAS.DTO;
using DAL.Entidades;

namespace AppCitasSAS.Servicios.Interfaces
{
    public interface IntfPacienteServicio
    {
        public PacienteDTO registrar(PacienteDTO pacienteDTO);


        public PacienteDTO buscarPorId(long id);


        public Paciente buscarPorEmail(String emailPaciente);


        public Boolean buscarPorDni(String dni);


        public List<PacienteDTO> buscarTodos();


        public void actualizarPaciente(PacienteDTO pacienteModificado);


        public PacienteDTO obtenerUsuarioPorToken(String token);


        public bool iniciarResetPassConEmail(String emailPaciente);


        public bool modificarContrasenaConToken(PacienteDTO paciente);


        public Paciente eliminar(long id);


        public bool confirmarCuenta(String token);


        public bool estaLaCuentaConfirmada(String email);


        bool verificarCredenciales(string emailPaciente, string clavePaciente);
    }
}

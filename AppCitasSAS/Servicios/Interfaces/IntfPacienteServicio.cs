using AppCitasSAS.DTO;
using DAL.Entidades;

namespace AppCitasSAS.Servicios.Interfaces
{
    public interface IntfPacienteServicio
    {
        public PacienteDTO registrar(PacienteDTO pacienteDTO); 


        public PacienteDTO buscarPorId(long id);


        public PacienteDTO buscarPorEmail(String emailPaciente);


        public Boolean buscarPorDni(String dni);


        public List<PacienteDTO> buscarTodos();


        public void actualizarPaciente(PacienteDTO pacienteModificado);


        public PacienteDTO obtenerPacientePorToken(String token);


        public bool iniciarProcesoRecuperacion(String emailPaciente);


        public bool modificarContraseñaConToken(PacienteDTO paciente);


        public void eliminar(long id);


        public bool confirmarCuenta(String token); 


        public bool estaLaCuentaConfirmada(String email);


        public bool verificarCredenciales(string emailPaciente, string clavePaciente);
    }
}

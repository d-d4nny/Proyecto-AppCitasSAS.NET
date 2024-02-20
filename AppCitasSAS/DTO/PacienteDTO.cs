using DAL.Entidades;

namespace AppCitasSAS.DTO
{
    public class PacienteDTO
    {
        // ATRIBUTOS
        public long IdPaciente { get; set; }
        public string NombreCompletoPaciente { get; set; }
        public string DniPaciente { get; set; }
        public string TlfPaciente { get; set; }
        public string EmailPaciente { get; set; }
        public string ContrasenaPaciente { get; set; }
        public string GeneroPaciente { get; set; }
        public string DireccionPaciente { get; set; }
        public byte[] ProfilePicture { get; set; }
        public string RolPaciente { get; set; }
        public string Token { get; set; }
        public DateTime? ExpiracionToken { get; set; }
        public bool CuentaConfirmada { get; set; }
        public List<InformeDTO> InformesDePaciente { get; set; } = new List<InformeDTO>();
        public List<CitasDTO> CitasDePaciente { get; set; } = new List<CitasDTO>();
        public string Password { get; set; }
        public string Password2 { get; set; }

        // CONSTRUCTORES
        public PacienteDTO()
        {
        }

        public PacienteDTO(string nombreCompletoPaciente, string dniPaciente, string tlfPaciente,
            string emailPaciente, string contrasenaPaciente, string generoPaciente,
            string direccionPaciente)
        {
            NombreCompletoPaciente = nombreCompletoPaciente;
            DniPaciente = dniPaciente;
            TlfPaciente = tlfPaciente;
            EmailPaciente = emailPaciente;
            ContrasenaPaciente = contrasenaPaciente;
            GeneroPaciente = generoPaciente;
            DireccionPaciente = direccionPaciente;
        }

        public PacienteDTO(long idPaciente, string nombreCompletoPaciente, string dniPaciente, string tlfPaciente,
            string emailPaciente, string contrasenaPaciente, string generoPaciente,
            string direccionPaciente, byte[] profilePicture, string rolPaciente, string token, DateTime expiracionToken, bool cuentaConfirmada,
            List<InformeDTO> informesDePaciente, List<CitasDTO> citasDePaciente, string password, string password2)
        {
            IdPaciente = idPaciente;
            NombreCompletoPaciente = nombreCompletoPaciente;
            DniPaciente = dniPaciente;
            TlfPaciente = tlfPaciente;
            EmailPaciente = emailPaciente;
            ContrasenaPaciente = contrasenaPaciente;
            GeneroPaciente = generoPaciente;
            DireccionPaciente = direccionPaciente;
            ProfilePicture = profilePicture;
            RolPaciente = rolPaciente;
            Token = token;
            CuentaConfirmada = cuentaConfirmada;
            ExpiracionToken = expiracionToken;
            InformesDePaciente = informesDePaciente;
            CitasDePaciente = citasDePaciente;
            Password = password;
            Password2 = password2;
        }

        // METODOS

        public override string ToString()
        {
            return $"PacienteDTO [IdPaciente={IdPaciente}, NombreCompletoPaciente={NombreCompletoPaciente}, " +
                $"DniPaciente={DniPaciente}, TlfPaciente={TlfPaciente}, EmailPaciente={EmailPaciente}, " +
                $"ContrasenaPaciente={ContrasenaPaciente}, GeneroPaciente={GeneroPaciente}, " +
                $"DireccionPaciente={DireccionPaciente}, ProfilePicture={ProfilePicture}, " +
                $"RolPaciente={RolPaciente}, Token={Token}, ExpiracionToken={ExpiracionToken}, " +
                $"CuentaConfirmada={CuentaConfirmada}, InformesDePaciente={InformesDePaciente}, " +
                $"CitasDePaciente={CitasDePaciente}, Password={Password}, Password2={Password2}]";
        }
    }
}

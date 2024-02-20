using DAL.Entidades;
using System.Numerics;

namespace AppCitasSAS.DTO
{
    public class CitasDTO
    {
        // ATRIBUTOS
        public long IdCita { get; set; }
        public DateTime FechaCita { get; set; }
        public TimeSpan HoraCita { get; set; }
        public string MotivoCita { get; set; }
        public string EstadoCita { get; set; }
        public PacienteDTO PacienteDTO { get; set; }
        public DoctoresDTO DoctoresDTO { get; set; }


        // CONSTRUCTORES
        public CitasDTO()
        {
        }

        public CitasDTO(DateTime fechaCita, TimeSpan horaCita, string motivoCita, string estadoCita)
        {
            FechaCita = fechaCita;
            HoraCita = horaCita;
            MotivoCita = motivoCita;
            EstadoCita = estadoCita;
        }

        public CitasDTO(long idCita, DateTime fechaCita, TimeSpan horaCita, string motivoCita, string estadoCita, PacienteDTO pacienteDTO,
            PacienteDTO doctoresDTO)
        {
            IdCita = idCita;
            FechaCita = fechaCita;
            HoraCita = horaCita;
            MotivoCita = motivoCita;
            EstadoCita = estadoCita;
            PacienteDTO = pacienteDTO;
            PacienteDTO = doctoresDTO;
        }

        // METODOS

        public override string ToString()
        {
            return $"CitasDTO [IdCita={IdCita}, FechaCita={FechaCita}, HoraCita={HoraCita}, MotivoCita={MotivoCita}, " +
                $"EstadoCita={EstadoCita}, Paciente={PacienteDTO}, Doctor={DoctoresDTO}]";
        }
    }
}

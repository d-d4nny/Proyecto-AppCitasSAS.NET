using DAL.Entidades;
using System.Numerics;

namespace AppCitasSAS.DTO
{
    public class CitasDTO
    {
        // ATRIBUTOS
        public long IdCita { get; set; }
        public DateOnly FechaCita { get; set; }
        public TimeOnly HoraCita { get; set; }
        public string MotivoCita { get; set; }
        public string EstadoCita { get; set; }
        public long IdPacienteDTO { get; set; }
        public long IdDoctoresDTO { get; set; }


        // CONSTRUCTORES
        public CitasDTO()
        {
        }

        public CitasDTO(DateOnly fechaCita, TimeOnly horaCita, string motivoCita, string estadoCita)
        {
            FechaCita = fechaCita;
            HoraCita = horaCita;
            MotivoCita = motivoCita;
            EstadoCita = estadoCita;
        }

        public CitasDTO(long idCita, DateOnly fechaCita, TimeOnly horaCita, string motivoCita, string estadoCita, long idPacienteDTO,
            long idDoctoresDTO)
        {
            IdCita = idCita;
            FechaCita = fechaCita;
            HoraCita = horaCita;
            MotivoCita = motivoCita;
            EstadoCita = estadoCita;
            IdPacienteDTO = idPacienteDTO;
            IdDoctoresDTO = idDoctoresDTO;
        }

        // METODOS

        public override string ToString()
        {
            return $"CitasDTO [IdCita={IdCita}, FechaCita={FechaCita}, HoraCita={HoraCita}, MotivoCita={MotivoCita}, " +
                $"EstadoCita={EstadoCita}, Paciente={IdPacienteDTO}, Doctor={IdDoctoresDTO}]";
        }
    }
}

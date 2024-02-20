using System;
using System.Collections.Generic;

namespace DAL.Entidades;

public partial class Cita
{
    public long IdCita { get; set; }

    public string EstadoCita { get; set; } = null!;

    public DateOnly FechaCita { get; set; }

    public TimeOnly HoraCita { get; set; }

    public string MotivoCita { get; set; } = null!;

    public long? IdDoctor { get; set; }

    public long? IdPaciente { get; set; }

    public virtual Doctore? IdDoctorNavigation { get; set; }

    public virtual Paciente? IdPacienteNavigation { get; set; }
}

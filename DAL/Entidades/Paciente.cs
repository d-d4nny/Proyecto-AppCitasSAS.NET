using System;
using System.Collections.Generic;

namespace DAL.Entidades;

public partial class Paciente
{
    public long IdPaciente { get; set; }

    public string ContraseñaPaciente { get; set; } = null!;

    public string? DireccionPaciente { get; set; }

    public string DniPaciente { get; set; } = null!;

    public string EmailPaciente { get; set; } = null!;

    public DateTime? ExpiracionToken { get; set; }

    public string? GeneroPaciente { get; set; }

    public string NombreCompletoPaciente { get; set; } = null!;

    public byte[]? ProfilePicture { get; set; }

    public string RolPaciente { get; set; } = null!;

    public string? TlfPaciente { get; set; }

    public string? TokenRecuperacion { get; set; }

    public bool CuentaConfirmada { get; set; }

    public virtual ICollection<Cita> Cita { get; set; } = new List<Cita>();

    public virtual ICollection<Informe> Informes { get; set; } = new List<Informe>();
}

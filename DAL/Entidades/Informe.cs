using System;
using System.Collections.Generic;

namespace DAL.Entidades;

public partial class Informe
{
    public long IdInforme { get; set; }

    public string? DescInforme { get; set; }

    public DateTime? FchInforme { get; set; }

    public string NombreInforme { get; set; } = null!;

    public long? IdPaciente { get; set; }

    public virtual Paciente? IdPacienteNavigation { get; set; }
}

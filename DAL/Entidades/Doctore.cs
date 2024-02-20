using System;
using System.Collections.Generic;

namespace DAL.Entidades;

public partial class Doctore
{
    public long IdDoctor { get; set; }

    public string EspecialidadDoctor { get; set; } = null!;

    public string NombreCompletoDoctor { get; set; } = null!;

    public long? IdConsultaTurno { get; set; }

    public virtual ICollection<Cita> Cita { get; set; } = new List<Cita>();

    public virtual ConsultasTurno? IdConsultaTurnoNavigation { get; set; }
}

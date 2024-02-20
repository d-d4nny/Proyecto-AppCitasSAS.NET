using System;
using System.Collections.Generic;

namespace DAL.Entidades;

public partial class ConsultasTurno
{
    public long IdConsultaTurno { get; set; }

    public int NumConsulta { get; set; }

    public TimeOnly? TramoHoraTurnoFin { get; set; }

    public TimeOnly? TramoHoraTurnoInicio { get; set; }

    public virtual ICollection<Doctore> Doctores { get; set; } = new List<Doctore>();
}

namespace AppCitasSAS.DTO
{
    public class ConsultaTurnoDTO
    {
        // ATRIBUTOS
        public long IdConsultaTurno { get; set; }
        public int NumConsulta { get; set; }
        public TimeSpan TramoHoraTurnoInicio { get; set; }
        public TimeSpan TramoHoraTurnoFin { get; set; }
        public List<DoctoresDTO> DoctoresConConsultaTurno { get; set; } = new List<DoctoresDTO>();


        // METODOS
        public ConsultaTurnoDTO()
        {
        }

        public ConsultaTurnoDTO(int numConsulta, TimeSpan tramoHoraTurnoInicio, TimeSpan tramoHoraTurnoFin)
        {
            NumConsulta = numConsulta;
            TramoHoraTurnoInicio = tramoHoraTurnoInicio;
            TramoHoraTurnoFin = tramoHoraTurnoFin;
        }

        public ConsultaTurnoDTO(long idConsultaTurno, int numConsulta, TimeSpan tramoHoraTurnoInicio,
            TimeSpan tramoHoraTurnoFin, List<DoctoresDTO> doctoresConConsultaTurno)
        {
            IdConsultaTurno = idConsultaTurno;
            NumConsulta = numConsulta;
            TramoHoraTurnoInicio = tramoHoraTurnoInicio;
            TramoHoraTurnoFin = tramoHoraTurnoFin;
            DoctoresConConsultaTurno = doctoresConConsultaTurno;
        }

        // CONSTRUCTORES
        public override string ToString()
        {
            return $"ConsultaTurnoDTO [IdConsultaTurno={IdConsultaTurno}, NumConsulta={NumConsulta}, " +
                $"TramoHoraTurnoInicio={TramoHoraTurnoInicio}, TramoHoraTurnoFin={TramoHoraTurnoFin}, " +
                $"DoctoresConConsultaTurno={DoctoresConConsultaTurno}]";
        }
    }
}

using DAL.Entidades;

namespace AppCitasSAS.DTO
{
    public class DoctoresDTO
    {
        // ATRIBUTOS
        public long IdDoctor { get; set; }
        public string NombreCompletoDoctor { get; set; }
        public string EspecialidadDoctor { get; set; }

        public ConsultaTurnoDTO ConsultaTurnoDTO { get; set; }

        // METODOS
        public DoctoresDTO()
        {
        }

        public DoctoresDTO(string nombreCompletoDoctor, string especialidadDoctor)
        {
            NombreCompletoDoctor = nombreCompletoDoctor;
            EspecialidadDoctor = especialidadDoctor;
        }

        public DoctoresDTO(long idDoctor, string nombreCompletoDoctor, string especialidadDoctor, ConsultaTurnoDTO consultaTurnoDTO)
        {
            IdDoctor = idDoctor;
            NombreCompletoDoctor = nombreCompletoDoctor;
            EspecialidadDoctor = especialidadDoctor;
            ConsultaTurnoDTO = consultaTurnoDTO;
        }

        // CONSTRUCTORES
        public override string ToString()
        {
            return $"DoctoresDTO [IdDoctor={IdDoctor}, NombreCompletoDoctor={NombreCompletoDoctor}, " +
                $"EspecialidadDoctor={EspecialidadDoctor}, ConsultaTurno={ConsultaTurnoDTO}]";
        }
    }
}

using DAL.Entidades;

namespace AppCitasSAS.DTO
{
    public class DoctoresDTO
    {
        // ATRIBUTOS
        public long IdDoctor { get; set; }
        public string NombreCompletoDoctor { get; set; }
        public string EspecialidadDoctor { get; set; }
        public long IdConsultaTurno { get; set; }


        // METODOS
        public DoctoresDTO()
        {
        }

        public DoctoresDTO(string nombreCompletoDoctor, string especialidadDoctor)
        {
            NombreCompletoDoctor = nombreCompletoDoctor;
            EspecialidadDoctor = especialidadDoctor;
        }

        public DoctoresDTO(long idDoctor, string nombreCompletoDoctor, string especialidadDoctor, long idConsultaTurno)
        {
            IdDoctor = idDoctor;
            NombreCompletoDoctor = nombreCompletoDoctor;
            EspecialidadDoctor = especialidadDoctor;
            IdConsultaTurno = idConsultaTurno;
        }

        // CONSTRUCTORES
        public override string ToString()
        {
            return $"DoctoresDTO [IdDoctor={IdDoctor}, NombreCompletoDoctor={NombreCompletoDoctor}, " +
                $"EspecialidadDoctor={EspecialidadDoctor}, IdConsultaTurno={IdConsultaTurno}]";
        }
    }
}

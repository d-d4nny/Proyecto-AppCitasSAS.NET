using DAL.Entidades;
using System.Globalization;

namespace AppCitasSAS.DTO
{
    public class InformeDTO
    {
        // ATRIBUTOS
        public long IdInforme { get; set; }
        public string NombreInforme { get; set; }
        public string DescInforme { get; set; }
        public Calendar FchInforme { get; set; }
        public PacienteDTO PacienteDTO { get; set; }


        // METODOS
        public InformeDTO()
        {
        }

        public InformeDTO(string nombreInforme, string descInforme, Calendar fchInforme)
        {
            NombreInforme = nombreInforme;
            DescInforme = descInforme;
            FchInforme = fchInforme;
        }

        public InformeDTO(long idInforme, string nombreInforme, string descInforme, Calendar fchInforme, PacienteDTO pacienteDTO)
        {
            IdInforme = idInforme;
            NombreInforme = nombreInforme;
            DescInforme = descInforme;
            FchInforme = fchInforme;
            PacienteDTO = pacienteDTO;
        }

        // CONSTRUCTORES

        public override string ToString()
        {
            return $"InformeDTO [IdInforme={IdInforme}, NombreInforme={NombreInforme}, DescInforme={DescInforme}, " +
                $"FchInforme={FchInforme}, Paciente={PacienteDTO}]";
        }
    }
}

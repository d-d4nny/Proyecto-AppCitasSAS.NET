using AppCitasSAS.DTO;
using AppCitasSAS.Servicios.Interfaces;
using AppCitasSAS.Utils;
using DAL.Entidades;

namespace AppCitasSAS.Servicios.Implementaciones
{
    public class ImplConsultaTurnoToDao : IntfConsultaTurnoToDao
    {
        public ConsultasTurno consultaTurnoToDao(ConsultaTurnoDTO consultaTurnoDTO)
        {
            try
            {
                ConsultasTurno consultaTurnoDao = new ConsultasTurno();

                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método consultaTurnoToDao() de la clase ImplConsultaTurnoToDao");

                consultaTurnoDao.IdConsultaTurno = consultaTurnoDTO.IdConsultaTurno;
                consultaTurnoDao.NumConsulta = consultaTurnoDTO.NumConsulta;
                consultaTurnoDao.TramoHoraTurnoInicio = consultaTurnoDTO.TramoHoraTurnoInicio;
                consultaTurnoDao.TramoHoraTurnoFin = consultaTurnoDTO.TramoHoraTurnoFin;

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método consultaTurnoToDao() de la clase ImplConsultaTurnoToDao");

                return consultaTurnoDao;
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog($"\n[ERROR ImplConsultaTurnoToDao - consultaTurnoToDao()] - Al convertir consultaTurnoDTO a consultaTurnoDAO (return null): {e}");
                return null;
            }
        }

        public List<ConsultasTurno> listConsultaTurnoToDao(List<ConsultaTurnoDTO> listaConsultaTurnoDTO)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método listConsultaTurnoToDao() de la clase ImplConsultaTurnoToDao");

                List<ConsultasTurno> listaConsultaTurnoDao = new List<ConsultasTurno>();
                foreach(ConsultaTurnoDTO consultaTurnoDTO in listaConsultaTurnoDTO)
                {
                    listaConsultaTurnoDao.Add(consultaTurnoToDao(consultaTurnoDTO));
                }

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método listConsultaTurnoToDao() de la clase ImplConsultaTurnoToDao");

                return listaConsultaTurnoDao;
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog($"\n[ERROR ImplConsultaTurnoToDao - listConsultaTurnoToDao()] - Al convertir lista de listaConsultaTurnoDTO a lista de listaConsultaTurnoDAO (return null): {e}");
            }
            return null;
        }
    }
}

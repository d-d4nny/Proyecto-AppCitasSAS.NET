using AppCitasSAS.DTO;
using AppCitasSAS.Servicios.Interfaces;
using AppCitasSAS.Utils;
using DAL.Entidades;
using System;
using System.Collections.Generic;

namespace AppCitasSAS.Servicios.Implementaciones
{
    public class ImplConsultaTurnoToDao : IntfConsultaTurnoToDao
    {
        // Método para convertir un DTO de ConsultaTurno a una entidad ConsultasTurno
        /// <param name="consultaTurnoDTO">DTO de la consulta de turno a convertir</param>
        /// <returns>Entidad ConsultasTurno convertida desde el DTO</returns>
        public ConsultasTurno consultaTurnoToDao(ConsultaTurnoDTO consultaTurnoDTO)
        {
            try
            {
                ConsultasTurno consultaTurnoDao = new ConsultasTurno();

                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método consultaTurnoToDao() de la clase ImplConsultaTurnoToDao");

                // Asignar valores desde el DTO a la entidad ConsultasTurno
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

        // Método para convertir una lista de DTOs de ConsultaTurno a una lista de entidades ConsultasTurno
        /// <param name="listaConsultaTurnoDTO">Lista de DTOs de consultas de turno a convertir</param>
        /// <returns>Lista de entidades ConsultasTurno convertidas desde los DTOs</returns>
        public List<ConsultasTurno> listConsultaTurnoToDao(List<ConsultaTurnoDTO> listaConsultaTurnoDTO)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método listConsultaTurnoToDao() de la clase ImplConsultaTurnoToDao");

                List<ConsultasTurno> listaConsultaTurnoDao = new List<ConsultasTurno>();

                // Convertir cada DTO a entidad y agregar a la lista
                foreach (ConsultaTurnoDTO consultaTurnoDTO in listaConsultaTurnoDTO)
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

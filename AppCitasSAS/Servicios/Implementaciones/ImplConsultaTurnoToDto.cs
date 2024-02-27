using AppCitasSAS.DTO;
using AppCitasSAS.Servicios.Interfaces;
using AppCitasSAS.Utils;
using DAL.Entidades;
using System;
using System.Collections.Generic;

namespace AppCitasSAS.Servicios.Implementaciones
{
    public class ImplConsultaTurnoToDto : IntfConsultaTurnoToDto
    {
        // Método para convertir una entidad ConsultasTurno a un DTO ConsultaTurno
        /// <param name="u">Entidad ConsultasTurno a convertir</param>
        /// <returns>DTO ConsultaTurno convertido desde la entidad</returns>
        public ConsultaTurnoDTO consultaTurnoToDto(ConsultasTurno u)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método consultaTurnoToDTO() de la clase ImplConsultaTurnoToDto");

                ConsultaTurnoDTO dto = new ConsultaTurnoDTO();

                // Asignar valores desde la entidad a DTO
                dto.IdConsultaTurno = u.IdConsultaTurno;
                dto.NumConsulta = u.NumConsulta;
                dto.TramoHoraTurnoInicio = (TimeOnly)u.TramoHoraTurnoInicio;
                dto.TramoHoraTurnoFin = (TimeOnly)u.TramoHoraTurnoFin;

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método consultaTurnoToDto() de la clase ImplConsultaTurnoToDto");
                return dto;
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog($"[ERROR ImplConsultaTurnoToDto - consultaTurnoToDTO()] - Error al convertir consultaTurno DAO a consultaTurnoDTO (return null): {e}");
                return null;
            }
        }

        // Método para convertir una lista de entidades ConsultasTurno a una lista de DTOs ConsultaTurno
        /// <param name="listaConsultaTurno">Lista de entidades ConsultasTurno a convertir</param>
        /// <returns>Lista de DTOs ConsultaTurno convertidos desde las entidades</returns>
        public List<ConsultaTurnoDTO> listConsultaTurnoToDto(List<ConsultasTurno> listaConsultaTurno)
        {
            try
            {
                List<ConsultaTurnoDTO> listaDto = new List<ConsultaTurnoDTO>();

                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método listConsultaTurnoToDto() de la clase ImplConsultaTurnoToDto");

                // Convertir cada entidad a DTO y agregar a la lista
                foreach (ConsultasTurno turno in listaConsultaTurno)
                {
                    listaDto.Add(consultaTurnoToDto(turno));
                }
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método listConsultaTurnoToDto() de la clase ImplConsultaTurnoToDto");
                return listaDto;
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog($"[ERROR ImplConsultaTurnoToDto - listConsultaTurnoToDto()] - Al convertir lista de entidades ConsultaTurno a DTO (return null): {e}");
            }
            return null;
        }
    }
}

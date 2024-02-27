using AppCitasSAS.DTO;
using AppCitasSAS.Servicios.Interfaces;
using AppCitasSAS.Utils;
using DAL.Entidades;
using System;
using System.Collections.Generic;

namespace AppCitasSAS.Servicios.Implementaciones
{
    public class ImplCitasToDto : IntfCitasToDto
    {
        // Método para convertir una entidad Cita a un DTO CitasDTO
        /// <param name="u">Entidad Cita a convertir</param>
        /// <returns>DTO de la cita resultante de la conversión</returns>
        public CitasDTO citasToDto(Cita u)
        {
            try
            {
                CitasDTO dto = new CitasDTO();

                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método citasToDto() de la clase ImplCitasToDto");

                // Configuración del DTO a partir de la entidad Cita
                dto.IdCita = u.IdCita;
                dto.FechaCita = u.FechaCita;
                dto.HoraCita = u.HoraCita;
                dto.MotivoCita = u.MotivoCita;
                dto.EstadoCita = u.EstadoCita;
                dto.IdPacienteDTO = (long)u.IdPaciente;
                dto.IdDoctoresDTO = (long)u.IdDoctor;

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método citasToDto() de la clase ImplCitasToDto");

                return dto;
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog($"[ERROR ImplCitasToDto - citasToDto()] - Al convertir entidad Cita a DTO (return null): {e}");
                return null;
            }
        }

        // Método para convertir una lista de entidades Cita a una lista de DTOs CitasDTO
        /// <param name="listaCitas">Lista de entidades Cita a convertir</param>
        /// <returns>Lista de DTOs de citas resultante de la conversión</returns>
        public List<CitasDTO> listCitasToDto(List<Cita> listaCitas)
        {
            try
            {
                List<CitasDTO> listaDto = new List<CitasDTO>();

                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método listCitasToDto() de la clase ImplCitasToDto");

                // Iterar sobre la lista de entidades y convertirlas a DTO
                foreach (Cita cita in listaCitas)
                {
                    listaDto.Add(citasToDto(cita));
                }

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método listCitasToDto() de la clase ImplCitasToDto");
                return listaDto;
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog($"[ERROR ImplCitasToDto - listCitasToDto()] - Al convertir lista de entidades Cita a DTO (return null): {e}");
            }
            return null;
        }
    }
}

using AppCitasSAS.DTO;
using AppCitasSAS.Servicios.Interfaces;
using AppCitasSAS.Utils;
using DAL.Entidades;
using System;
using System.Collections.Generic;

namespace AppCitasSAS.Servicios.Implementaciones
{
    public class ImplDoctorToDto : IntfDoctorToDto
    {
        // Clase de implementación para la conversión de DAO a DTO para la entidad Doctore

        // Método para convertir un objeto DAO Doctore a un objeto DTO DoctoresDTO
        public DoctoresDTO doctoresToDto(Doctore u)
        {
            try
            {
                DoctoresDTO dto = new DoctoresDTO();

                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método doctoresToDto() de la clase ImplDoctorToDto");

                // Asignar valores desde la entidad DAO al DTO
                dto.IdDoctor = u.IdDoctor;
                dto.NombreCompletoDoctor = u.NombreCompletoDoctor;
                dto.EspecialidadDoctor = u.EspecialidadDoctor;
                dto.IdConsultaTurno = (long)u.IdConsultaTurno;

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método doctoresToDto() de la clase ImplDoctorToDto");
                return dto;
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog($"[ERROR ImplDoctorToDto - doctoresToDto()] - Error al convertir doctor DAO a doctorDTO (return null): {e}");
                return null;
            }
        }

        // Método para convertir una lista de objetos DAO Doctore a una lista de objetos DTO DoctoresDTO
        public List<DoctoresDTO> listDoctoresToDto(List<Doctore> listaDoctor)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método listDoctoresToDto() de la clase ImplDoctorToDto");

                // Crear una lista para almacenar los DTO resultantes
                List<DoctoresDTO> listaDto = new List<DoctoresDTO>();

                // Iterar a través de la lista de entidades DAO y convertir cada elemento a DTO
                foreach (Doctore u in listaDoctor)
                {
                    listaDto.Add(doctoresToDto(u));
                }

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método listDoctoresToDto() de la clase ImplDoctorToDto");
                return listaDto;
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog($"[ERROR ImplDoctorToDto - listDoctoresToDto()] - Error al convertir lista de doctor DAO a lista de doctorDTO (return null): {e}");
            }
            return null;
        }
    }
}

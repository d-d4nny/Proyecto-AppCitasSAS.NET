using AppCitasSAS.DTO;
using AppCitasSAS.Servicios.Interfaces;
using AppCitasSAS.Utils;
using DAL.Entidades;
using System;
using System.Collections.Generic;

namespace AppCitasSAS.Servicios.Implementaciones
{
    public class ImplDoctorToDao : IntfDoctorToDao
    {
        // Clase de implementación para la conversión de DTO a DAO para la entidad Doctore

        // Método para convertir un objeto DTO DoctoresDTO a una entidad DAO Doctore
        public Doctore doctoresToDao(DoctoresDTO doctoresDTO)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método doctoresToDao() de la clase ImplDoctorToDao");

                // Crear una nueva entidad DAO y asignar valores desde el DTO
                Doctore dao = new Doctore();
                dao.IdDoctor = doctoresDTO.IdDoctor;
                dao.NombreCompletoDoctor = doctoresDTO.NombreCompletoDoctor;
                dao.EspecialidadDoctor = doctoresDTO.EspecialidadDoctor;
                dao.IdConsultaTurno = doctoresDTO.IdConsultaTurno;

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método doctoresToDao() de la clase ImplDoctorToDao");
                return dao;
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog($"\n[ERROR ImplDoctorToDao - doctoresToDao()] - Al convertir DoctorDTO a DAO (return null): {e}");
            }

            return null;
        }

        // Método para convertir una lista de objetos DTO DoctoresDTO a una lista de entidades DAO Doctore
        public List<Doctore> listDoctoresToDao(List<DoctoresDTO> listaDoctoresDTO)
        {
            List<Doctore> listaDoctoresDao = new List<Doctore>();

            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método listDoctoresToDao() de la clase ImplDoctorToDao");

                // Iterar a través de la lista de DTO y convertir cada elemento a DAO
                foreach (DoctoresDTO doctoresDTO in listaDoctoresDTO)
                {
                    listaDoctoresDao.Add(doctoresToDao(doctoresDTO));
                }
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método listDoctoresToDao() de la clase ImplDoctorToDao");
                return listaDoctoresDao;
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog($"\n[ERROR ImplDoctorToDao - listDoctoresToDao()] - Al convertir lista de doctorDTO a lista de doctorDAO (return null): {e}");
            }
            return null;
        }
    }
}

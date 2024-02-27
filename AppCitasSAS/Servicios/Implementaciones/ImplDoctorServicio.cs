using AppCitasSAS.DTO;
using AppCitasSAS.Servicios.Interfaces;
using AppCitasSAS.Utils;
using DAL.Entidades;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using System;
using System.Collections.Generic;

namespace AppCitasSAS.Servicios.Implementaciones
{
    public class ImplDoctorServicio : IntfDoctorServicio
    {
        // Clase de servicio para la gestión de la entidad Doctor

        private readonly AppCitasSasContext _contexto;
        private readonly IntfDoctorToDto _toDto;
        private readonly IntfDoctorToDao _toDao;

        // Constructor de la clase que recibe el contexto de la aplicación y las interfaces de mapeo DTO y DAO
        public ImplDoctorServicio(AppCitasSasContext contexto, IntfDoctorToDto toDto, IntfDoctorToDao toDao)
        {
            _contexto = contexto;
            _toDto = toDto;
            _toDao = toDao;
        }

        // Método para registrar un nuevo doctor en el sistema
        /// <param name="doctorDTO">DTO DoctoresDTO que contiene la información del doctor a registrar</param>
        /// <returns>DTO DoctoresDTO del doctor registrado o null si ya existe</returns>
        public DoctoresDTO registrar(DoctoresDTO doctorDTO)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método registrar() de la clase ImplDoctorServicio");

                // Verificar si ya existe un doctor con el mismo nombre
                var nombreExistente = _contexto.Doctores.FirstOrDefault(u => u.NombreCompletoDoctor == doctorDTO.NombreCompletoDoctor);

                if (nombreExistente != null)
                {
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método registrar() de la clase ImplDoctorServicio");
                    return null; // El doctor ya existe, no se puede registrar
                }

                // Verificar si ya existe un doctor con el mismo ID
                var doctorExiste = _contexto.Doctores.FirstOrDefault(u => u.IdDoctor == doctorDTO.IdDoctor);

                if (doctorExiste != null)
                {
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método registrar() de la clase ImplDoctorServicio");
                    return doctorDTO; // El doctor ya existe, retornar el DTO proporcionado
                }

                // Convertir el DTO a entidad DAO y guardarlo en la base de datos
                Doctore doctoreDao = _toDao.doctoresToDao(doctorDTO);
                _contexto.Doctores.Add(doctoreDao);
                _contexto.SaveChanges();

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método registrar() de la clase ImplDoctorServicio");

                return doctorDTO; // Retornar el DTO proporcionado (registro exitoso)

            }
            catch (DbUpdateException dbe)
            {
                EscribirLog.escribirEnFicheroLog("[Error ImplDoctorServicio - registrar()] Error de persistencia al actualizar la bbdd: " + dbe.Message);
                return null; // Error al actualizar la base de datos
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog("[Error ImplDoctorServicio - registrar()] Error al registrar un doctor: " + e.Message);
                return null; // Error inesperado al registrar el doctor
            }
        }

        // Método para actualizar la información de un doctor en el sistema
        /// <param name="doctorModificado">DTO DoctoresDTO que contiene la información actualizada del doctor</param>
        public void actualizarDoctor(DoctoresDTO doctorModificado)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método actualizarDoctor() de la clase ImplDoctorServicio");

                // Buscar el doctor en la base de datos por su ID
                Doctore? doctorActual = _contexto.Doctores.Find(doctorModificado.IdDoctor);

                if (doctorActual != null)
                {
                    // Actualizar la información del doctor con la proporcionada en el DTO
                    doctorActual.NombreCompletoDoctor = doctorModificado.NombreCompletoDoctor;
                    doctorActual.EspecialidadDoctor = doctorModificado.EspecialidadDoctor;
                    doctorActual.IdConsultaTurno = doctorModificado.IdConsultaTurno;

                    // Guardar los cambios en la base de datos
                    _contexto.Doctores.Update(doctorActual);
                    _contexto.SaveChanges();
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método actualizarDoctor() de la clase ImplDoctorServicio. Doctor actualizado OK");
                }
                else
                {
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método actualizarDoctor() de la clase ImplDoctorServicio. Doctor no encontrado");
                }
            }
            catch (DbUpdateException dbe)
            {
                EscribirLog.escribirEnFicheroLog("[Error ImplDoctorServicio - actualizarDoctor()] Error de persistencia al modificar el doctor " + dbe.Message);
            }
        }

        // Método para buscar un doctor por su ID
        /// <param name="id">ID del doctor a buscar</param>
        /// <returns>DTO DoctoresDTO del doctor encontrado o null si no se encuentra</returns>
        public DoctoresDTO buscarPorId(long id)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método buscarPorId() de la clase ImplDoctorServicio");

                // Buscar el doctor en la base de datos por su ID
                Doctore? doctor = _contexto.Doctores.Find(id);
                if (doctor != null)
                {
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método buscarPorId() de la clase ImplDoctorServicio");
                    return _toDto.doctoresToDto(doctor); // Convertir la entidad a DTO y retornar
                }
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog($"[ERROR ImplDoctorServicio - buscarPorId()] - Al buscar un doctor por su id: {e}");
            }
            return null; // Error al buscar el doctor por ID o no encontrado
        }

        // Método para obtener todos los doctores registrados en el sistema
        /// <returns>Lista de DTOs DoctoresDTO de todos los doctores registrados</returns>
        public List<DoctoresDTO> buscarTodos()
        {
            EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método obtenerTodos() de la clase ImplDoctorServicio");

            // Obtener todos los doctores de la base de datos y convertir a lista de DTOs
            return _toDto.listDoctoresToDto(_contexto.Doctores.ToList());
        }

        // Método para eliminar un doctor por su ID
        /// <param name="id">ID del doctor a eliminar</param>
        public void eliminar(long id)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método eliminar() de la clase ImplDoctorServicio");

                // Buscar el doctor en la base de datos por su ID
                Doctore? doctor = _contexto.Doctores.Find(id);
                if (doctor != null)
                {
                    // Eliminar el doctor de la base de datos
                    _contexto.Doctores.Remove(doctor);
                    _contexto.SaveChanges();
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método eliminar() de la clase ImplDoctorServicio. Doctor eliminado correctamente.");
                }
            }
            catch (DbUpdateException dbe)
            {
                EscribirLog.escribirEnFicheroLog($"[Error ImplDoctorServicio - eliminar()] Error de persistencia al eliminar un doctor por su id: {dbe.Message}");
            }
        }
    }
}

using AppCitasSAS.DTO;
using AppCitasSAS.Servicios.Interfaces;
using AppCitasSAS.Utils;
using DAL.Entidades;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace AppCitasSAS.Servicios.Implementaciones
{
    public class ImplCitasServicio : IntfCitasServicio
    {
        private readonly AppCitasSasContext _contexto;
        private readonly IntfCitasToDao _toDao;
        private readonly IntfCitasToDto _toDto;

        public ImplCitasServicio(AppCitasSasContext contexto, IntfCitasToDao toDao, IntfCitasToDto toDto)
        {
            _contexto = contexto;
            _toDao = toDao;
            _toDto = toDto;
        }

        // Método para registrar una cita
        /// <param name="citaDTO">DTO de la cita a registrar</param>
        /// <returns>DTO de la cita registrada</returns>
        public CitasDTO registrar(CitasDTO citaDTO)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método registrar() de la clase ImplCitasServicio");

                Cita citaDao = new Cita();
                // Configuración de la entidad Cita a partir del DTO
                citaDao.EstadoCita = "Pendiente";
                citaDao.MotivoCita = citaDTO.MotivoCita;
                citaDao.FechaCita = citaDTO.FechaCita;
                citaDao.HoraCita = citaDTO.HoraCita;
                citaDao.IdDoctor = citaDTO.IdDoctoresDTO;
                citaDao.IdPaciente = citaDTO.IdPacienteDTO;

                _contexto.Citas.Add(citaDao);
                _contexto.SaveChanges();

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método registrar() de la clase ImplCitasServicio");

                return citaDTO;
            }
            catch (DbUpdateException dbe)
            {
                EscribirLog.escribirEnFicheroLog("[Error ImplCitasServicio - registrar()] Error de persistencia al actualizar la bbdd: " + dbe.Message);
                return null;
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog("[Error ImplCitasServicio - registrar()] Error al registrar una cita: " + e.Message);
                return null;
            }
        }

        // Método para obtener todas las citas de un paciente
        /// <param name="IdPaciente">ID del paciente</param>
        /// <returns>Lista de DTOs de citas del paciente</returns>
        public List<CitasDTO> ObtenerCitasDePaciente(long IdPaciente)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método ObtenerCitasDePaciente() de la clase ImplCitasServicio");

                List<Cita> listaCitas = _contexto.Citas.Where(m => m.IdPaciente == IdPaciente).ToList();
                return _toDto.listCitasToDto(listaCitas);
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog($"[ERROR ImplCitasServicio - ObtenerCitasDePaciente()] - Error al obtener las citas de un paciente: {e}");
                return null;
            }
        }

        // Método para buscar una cita por su ID
        /// <param name="id">ID de la cita a buscar</param>
        /// <returns>DTO de la cita encontrada o null si no existe</returns>
        public CitasDTO buscarPorId(long id)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método buscarPorId() de la clase ImplCitasServicio");

                Cita? cita = _contexto.Citas.Find(id);
                if (cita != null)
                {
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método buscarPorId() de la clase ImplCitasServicio");
                    return _toDto.citasToDto(cita);
                }
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog($"[ERROR ImplCitasServicio - buscarPorId()] - Error al buscar una cita por su id: {e}");
            }
            return null;
        }

        // Método para obtener todas las citas
        /// <returns>Lista de todas las citas en forma de DTOs</returns>
        public List<CitasDTO> buscarTodos()
        {
            EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método obtenerTodos() de la clase ImplCitasServicio");

            return _toDto.listCitasToDto(_contexto.Citas.ToList());
        }

        // Método para eliminar una cita por su ID
        /// <param name="id">ID de la cita a eliminar</param>
        public void eliminar(long id)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método eliminar() de la clase ImplCitasServicio");

                Cita? cita = _contexto.Citas.Find(id);
                if (cita != null)
                {
                    _contexto.Citas.Remove(cita);
                    _contexto.SaveChanges();
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método eliminar() de la clase ImplCitasServicio. Cita eliminada correctamente.");
                }
            }
            catch (DbUpdateException dbe)
            {
                EscribirLog.escribirEnFicheroLog($"[Error ImplCitasServicio - eliminar()] Error de persistencia al eliminar una cita por su id: {dbe.Message}");
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog($"[Error ImplCitasServicio - eliminar()] Error al eliminar una cita: {e.Message}");
            }
        }

        // Método para cancelar una cita por su ID
        /// <param name="idCita">ID de la cita a cancelar</param>
        public void cancelarCita(long idCita)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método cancelarCita() de la clase ImplCitasServicio");

                Cita? cita = _contexto.Citas.Find(idCita);
                if (cita != null)
                {
                    cita.EstadoCita = "Cancelada";
                    _contexto.SaveChanges();
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método cancelarCita() de la clase ImplCitasServicio. Estado de la cita cambiado a 'Cancelada'.");
                }
                else
                {
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método cancelarCita() de la clase ImplCitasServicio. Cita no encontrada.");
                }
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog($"[Error ImplCitasServicio - cancelarCita()] Error al cancelar una cita: {e.Message}");
            }
        }

        // Método para marcar una cita como completada por su ID
        /// <param name="idCita">ID de la cita a marcar como completada</param>
        public void completarCita(long idCita)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método completarCita() de la clase ImplCitasServicio");

                Cita cita = _contexto.Citas.Find(idCita);
                if (cita != null)
                {
                    cita.EstadoCita = "Completada";
                    _contexto.SaveChanges();
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método completarCita() de la clase ImplCitasServicio. Estado de la cita cambiado a 'Completada'.");
                }
                else
                {
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método completarCita() de la clase ImplCitasServicio. Cita no encontrada.");
                }
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog($"[Error ImplCitasServicio - completarCita()] Error al completar una cita: {e.Message}");
            }
        }
    }
}

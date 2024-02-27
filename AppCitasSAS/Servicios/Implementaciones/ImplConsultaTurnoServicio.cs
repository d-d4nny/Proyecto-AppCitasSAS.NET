using AppCitasSAS.DTO;
using AppCitasSAS.Servicios.Interfaces;
using AppCitasSAS.Utils;
using DAL.Entidades;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace AppCitasSAS.Servicios.Implementaciones
{
    public class ImplConsultaTurnoServicio : IntfConsultaTurnoServicio
    {
        private readonly AppCitasSasContext _contexto;
        private readonly IntfConsultaTurnoToDao _toDao;
        private readonly IntfConsultaTurnoToDto _toDto;

        public ImplConsultaTurnoServicio(AppCitasSasContext contexto, IntfConsultaTurnoToDao toDao, IntfConsultaTurnoToDto toDto)
        {
            _contexto = contexto;
            _toDao = toDao;
            _toDto = toDto;
        }

        // Método para registrar una nueva consulta de turno
        /// <param name="consultaTurnoDTO">DTO de la consulta de turno a registrar</param>
        /// <returns>DTO de la consulta de turno registrada</returns>
        public ConsultaTurnoDTO registrar(ConsultaTurnoDTO consultaTurnoDTO)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método registrar() de la clase ImplConsultaTurnoServicio");

                // Verificar si la consulta de turno ya existe
                var turnoExiste = _contexto.ConsultasTurnos.FirstOrDefault(u => u.IdConsultaTurno == consultaTurnoDTO.IdConsultaTurno);

                if (turnoExiste != null)
                {
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método registrar() de la clase ImplConsultaTurnoServicio");
                    return consultaTurnoDTO;
                }

                // Crear la entidad ConsultasTurno a partir del DTO
                ConsultasTurno turnoDao = _toDao.consultaTurnoToDao(consultaTurnoDTO);
                _contexto.ConsultasTurnos.Add(turnoDao);
                _contexto.SaveChanges();

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método registrar() de la clase ImplConsultaTurnoServicio");

                return consultaTurnoDTO;
            }
            catch (DbUpdateException dbe)
            {
                EscribirLog.escribirEnFicheroLog("[Error ImplConsultaTurnoServicio - registrar()] Error de persistencia al actualizar la bbdd: " + dbe.Message);
                return null;
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog("[Error ImplConsultaTurnoServicio - registrar()] Error al registrar un turno: " + e.Message);
                return null;
            }
        }

        // Método para actualizar una consulta de turno existente
        /// <param name="turnoModificado">DTO del turno modificado</param>
        public void actualizarTurno(ConsultaTurnoDTO turnoModificado)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método actualizarTurno() de la clase ImplConsultaTurnoServicio");

                // Obtener la consulta de turno actual
                ConsultasTurno? turnoActual = _contexto.ConsultasTurnos.Find(turnoModificado.IdConsultaTurno);

                if (turnoActual != null)
                {
                    // Actualizar los campos de la consulta de turno
                    turnoActual.NumConsulta = turnoModificado.NumConsulta;
                    turnoActual.TramoHoraTurnoInicio = turnoModificado.TramoHoraTurnoInicio;
                    turnoActual.TramoHoraTurnoFin = turnoModificado.TramoHoraTurnoFin;

                    _contexto.ConsultasTurnos.Update(turnoActual);
                    _contexto.SaveChanges();
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método actualizarTurno() de la clase ImplConsultaTurnoServicio. Turno actualizado OK");
                }
                else
                {
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método actualizarTurno() de la clase ImplConsultaTurnoServicio. Turno no encontrado");
                }
            }
            catch (DbUpdateException dbe)
            {
                EscribirLog.escribirEnFicheroLog("[Error ImplDoctorServicio - actualizarDoctor()] Error de persistencia al modificar el turno " + dbe.Message);
            }
        }

        // Método para buscar una consulta de turno por su ID
        /// <param name="id">ID de la consulta de turno a buscar</param>
        /// <returns>DTO de la consulta de turno encontrada</returns>
        public ConsultaTurnoDTO buscarPorId(long id)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método buscarPorId() de la clase ImplConsultaTurnoServicio");

                // Buscar la consulta de turno por su ID
                ConsultasTurno? turno = _contexto.ConsultasTurnos.Find(id);
                if (turno != null)
                {
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método buscarPorId() de la clase ImplConsultaTurnoServicio");
                    return _toDto.consultaTurnoToDto(turno);
                }
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog($"[ERROR ImplConsultaTurnoServicio - buscarPorId()] - Al buscar un turno por su id: {e}");
            }
            return null;
        }

        // Método para obtener todas las consultas de turno
        /// <returns>Lista de DTOs de todas las consultas de turno</returns>
        public List<ConsultaTurnoDTO> buscarTodos()
        {
            EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método obtenerTodos() de la clase ImplConsultaTurnoServicio");

            return _toDto.listConsultaTurnoToDto(_contexto.ConsultasTurnos.ToList());
        }

        // Método para eliminar una consulta de turno por su ID
        /// <param name="id">ID de la consulta de turno a eliminar</param>
        public void eliminar(long id)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método eliminar() de la clase ImplConsultaTurnoServicio");

                // Buscar la consulta de turno por su ID y eliminarla
                ConsultasTurno? turno = _contexto.ConsultasTurnos.Find(id);
                if (turno != null)
                {
                    _contexto.ConsultasTurnos.Remove(turno);
                    _contexto.SaveChanges();
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método eliminar() de la clase ImplConsultaTurnoServicio. turno eliminada correctamente.");
                }
            }
            catch (DbUpdateException dbe)
            {
                EscribirLog.escribirEnFicheroLog($"[Error ImplConsultaTurnoServicio - eliminar()] Error de persistencia al eliminar un turno por su id: {dbe.Message}");
            }
        }
    }
}

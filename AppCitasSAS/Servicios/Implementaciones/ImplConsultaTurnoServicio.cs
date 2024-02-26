using AppCitasSAS.DTO;
using AppCitasSAS.Servicios.Interfaces;
using AppCitasSAS.Utils;
using DAL.Entidades;
using Microsoft.EntityFrameworkCore;

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

        public ConsultaTurnoDTO registrar(ConsultaTurnoDTO consultaTurnoDTO)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método registrar() de la clase ImplConsultaTurnoServicio");

                var turnoExiste = _contexto.ConsultasTurnos.FirstOrDefault(u => u.IdConsultaTurno == consultaTurnoDTO.IdConsultaTurno);

                if (turnoExiste != null)
                {
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método registrar() de la clase ImplConsultaTurnoServicio");
                    return consultaTurnoDTO;
                }

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


        public void actualizarTurno(ConsultaTurnoDTO turnoModificado)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método actualizarTurno() de la clase ImplConsultaTurnoServicio");

                ConsultasTurno? turnoActual = _contexto.ConsultasTurnos.Find(turnoModificado.IdConsultaTurno);

                if (turnoActual != null)
                {
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


        public ConsultaTurnoDTO buscarPorId(long id)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método buscarPorId() de la clase ImplConsultaTurnoServicio");

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


        public List<ConsultaTurnoDTO> buscarTodos()
        {
            EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método obtenerTodos() de la clase ImplConsultaTurnoServicio");

            return _toDto.listConsultaTurnoToDto(_contexto.ConsultasTurnos.ToList());
        }


        public void eliminar(long id)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método eliminar() de la clase ImplConsultaTurnoServicio");

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

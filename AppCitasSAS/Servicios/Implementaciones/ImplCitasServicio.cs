using AppCitasSAS.DTO;
using AppCitasSAS.Servicios.Interfaces;
using AppCitasSAS.Utils;
using DAL.Entidades;
using Microsoft.EntityFrameworkCore;

namespace AppCitasSAS.Servicios.Implementaciones
{
    public class ImplCitasServicio : IntfCitasServicio
    {
        private readonly AppCitasSasContext _contexto;
        private readonly IntfCitasToDao _toDao;
        private readonly IntfCitasToDto _toDto;

        public ImplCitasServicio (AppCitasSasContext contexto, IntfCitasToDao toDao, IntfCitasToDto toDto)
        {
            _contexto = contexto;
            _toDao = toDao;
            _toDto = toDto;
        }

        public CitasDTO registrar(CitasDTO citaDTO)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método registrar() de la clase ImplCitasServicio");


                Cita citaDao = new Cita();

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


		public List<CitasDTO> ObtenerCitasDePaciente(long IdPaciente)
		{
			try
			{
				// Se escribe un mensaje de registro al entrar al método.
				EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método ObtenerCitasDePaciente() de la clase ImplCitasServicio");

                List<Cita> listaCitas = _contexto.Citas.Where(m => m.IdPaciente == IdPaciente).ToList();
                return _toDto.listCitasToDto(listaCitas);
			}
			catch (Exception e)
			{
                // Se atrapa cualquier excepción que pueda ocurrir.
                // Se escribe un mensaje de registro indicando el error.
                EscribirLog.escribirEnFicheroLog($"[ERROR ImplCitasServicio - ObtenerCitasDePaciente()] - Argumento id es NULL al obtener las citas de un paciente: {e}");

                return null;
            }
		}


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
                EscribirLog.escribirEnFicheroLog($"[ERROR ImplCitasServicio - buscarPorId()] - Al buscar una cita por su id: {e}");
            }
            return null;

        }


        public List<CitasDTO> buscarTodos()
        {
            EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método obtenerTodos() de la clase ImplCitasServicio");

            return _toDto.listCitasToDto(_contexto.Citas.ToList());
        }


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
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método eliminar() de la clase ImplCitasServicio. cita eliminada correctamente.");
                }
                  }
            catch (DbUpdateException dbe)
            {
                EscribirLog.escribirEnFicheroLog($"[Error ImplCitasServicio - eliminar()] Error de persistencia al eliminar una cita por su id: {dbe.Message}");
            }
          }

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

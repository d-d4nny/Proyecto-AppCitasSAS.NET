using AppCitasSAS.DTO;
using AppCitasSAS.Servicios.Interfaces;
using AppCitasSAS.Utils;
using DAL.Entidades;
using Microsoft.EntityFrameworkCore;

namespace AppCitasSAS.Servicios.Implementaciones
{
    public class ImplDoctorServicio : IntfDoctorServicio
    {
        private readonly AppCitasSasContext _contexto;
        private readonly IntfDoctorToDto _toDto;
        private readonly IntfDoctorToDao _toDao;

        public ImplDoctorServicio(AppCitasSasContext contexto, IntfDoctorToDto toDto, IntfDoctorToDao toDao)
        {
            _contexto = contexto;
            _toDto = toDto;
            _toDao = toDao;
        }

        public DoctoresDTO registrar (DoctoresDTO doctorDTO)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método registrar() de la clase ImplDoctorServicio");

                var nombreExistente = _contexto.Doctores.FirstOrDefault(u => u.NombreCompletoDoctor == doctorDTO.NombreCompletoDoctor);

                if (nombreExistente != null) 
                {
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método registrar() de la clase ImplDoctorServicio");
                    return null;
                }

                var doctorExiste = _contexto.Doctores.FirstOrDefault(u => u.IdDoctor == doctorDTO.IdDoctor);

                if (doctorExiste != null)
                {
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método registrar() de la clase ImplDoctorServicio");
                    return doctorDTO;
                }

                Doctore doctoreDao = _toDao.doctoresToDao(doctorDTO);
                _contexto.Doctores.Add(doctoreDao);
                _contexto.SaveChanges();

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método registrar() de la clase ImplDoctorServicio");

                return doctorDTO;

            }
            catch (DbUpdateException dbe)
            {
                EscribirLog.escribirEnFicheroLog("[Error ImplDoctorServicio - registrar()] Error de persistencia al actualizar la bbdd: " + dbe.Message);
                return null;
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog("[Error ImplDoctorServicio - registrar()] Error al registrar un doctor: " + e.Message);
                return null;
            }
        }


        public DoctoresDTO buscarPorId(long id)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método buscarPorId() de la clase ImplDoctorServicio");

                Doctore? doctor = _contexto.Doctores.Find(id);
                if (doctor != null)
                {
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método buscarPorId() de la clase ImplDoctorServicio");
                    return _toDto.doctoresToDto(doctor);
                }
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog($"[ERROR ImplDoctorServicio - buscarPorId()] - Al buscar un doctor por su id: {e}");
            }
            return null;

        }


        public List<DoctoresDTO> buscarTodos()
        {
            EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método obtenerTodos() de la clase ImplDoctorServicio");

            return _toDto.listDoctoresToDto(_contexto.Doctores.ToList());
        }


        public void eliminar(long id)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método eliminar() de la clase ImplDoctorServicio");

                Doctore? doctor = _contexto.Doctores.Find(id);
                if (doctor != null)
                {
                    _contexto.Doctores.Remove(doctor);
                    _contexto.SaveChanges();
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método eliminar() de la clase ImplDoctorServicio. doctor eliminada correctamente.");
                }
            }
            catch (DbUpdateException dbe)
            {
                EscribirLog.escribirEnFicheroLog($"[Error ImplDoctorServicio - eliminar()] Error de persistencia al eliminar un doctor por su id: {dbe.Message}");
            }
        }

    }
}

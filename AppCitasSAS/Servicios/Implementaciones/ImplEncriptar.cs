using AppCitasSAS.Servicios.Interfaces;
using AppCitasSAS.Utils;
using System.Security.Cryptography;
using System.Text;

namespace AppCitasSAS.Servicios.Implementaciones
{
    public class ImplEncriptar : IntfEncriptar
    {
        public string Encriptar(string contraseña)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método Encriptar() de la clase EncriptarImpl");

                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(contraseña);
                    byte[] hashBytes = sha256.ComputeHash(bytes);
                    string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método Encriptar() de la clase EncriptarImpl");
                    return hash;
                }
            }
            catch (ArgumentException e)
            {
                EscribirLog.escribirEnFicheroLog("[Error  EncriptarImpl - Encriptar()] Error al encriptar: " + e.Message);
                return null;
            }

        }
    }
}

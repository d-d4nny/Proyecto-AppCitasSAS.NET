using AppCitasSAS.Servicios.Interfaces;
using AppCitasSAS.Utils;
using System.Security.Cryptography;
using System.Text;

namespace AppCitasSAS.Servicios.Implementaciones
{
    public class ImplEncriptar : IntfEncriptar
    {
        // Clase de implementación para encriptar contraseñas

        // Método para encriptar una cadena utilizando el algoritmo SHA-256
        public string Encriptar(string contraseña)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método Encriptar() de la clase ImplEncriptar");

                // Crear un objeto SHA-256 para calcular el hash
                using (SHA256 sha256 = SHA256.Create())
                {
                    // Convertir la cadena de contraseña a bytes
                    byte[] bytes = Encoding.UTF8.GetBytes(contraseña);

                    // Calcular el hash utilizando SHA-256
                    byte[] hashBytes = sha256.ComputeHash(bytes);

                    // Convertir los bytes del hash a una cadena hexadecimal
                    string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método Encriptar() de la clase ImplEncriptar");
                    return hash;
                }
            }
            catch (ArgumentException e)
            {
                EscribirLog.escribirEnFicheroLog("[Error ImplEncriptar - Encriptar()] Error al encriptar: " + e.Message);
                return null;
            }
        }
    }
}

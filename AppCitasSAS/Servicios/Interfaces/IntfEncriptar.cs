namespace AppCitasSAS.Servicios.Interfaces
{
    public interface IntfEncriptar
    {
        /// <summary>
        /// Encripta el texto proporcionado.
        /// </summary>
        /// <param name="texto">Texto a encriptar.</param>
        /// <returns>Texto encriptado.</returns>
        public string Encriptar(string texto);
    }
}

namespace AppCitasSAS.Servicios.Interfaces
{
    public interface IntfEmailRecuperacion
    {
        /// <summary>
        /// Envía un email de recuperación a la dirección especificada.
        /// </summary>
        /// <param name="emailDestino">Dirección de correo electrónico de destino.</param>
        /// <param name="nombreUsuario">Nombre del usuario al que se envía el email.</param>
        /// <param name="token">Token de recuperación para la contraseña.</param>
        public void enviarEmailRecuperacion(string emailDestino, string nombreUsuario, string token);

        /// <summary>
        /// Envía un email de confirmación a la dirección especificada.
        /// </summary>
        /// <param name="emailDestino">Dirección de correo electrónico de destino.</param>
        /// <param name="nombreUsuario">Nombre del usuario al que se envía el email.</param>
        /// <param name="token">Token de confirmación.</param>
        void enviarEmailConfirmacion(string emailDestino, string nombreUsuario, string token);
    }
}

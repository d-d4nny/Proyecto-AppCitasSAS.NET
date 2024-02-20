namespace AppCitasSAS.Servicios.Interfaces
{
    public interface IntfEmailRecuperacion
    {


        public void enviarEmailRecuperacion(string emailDestino, string nombreUsuario, string token);


        void enviarEmailConfirmacion(string emailDestino, string nombreUsuario, string token);
    }
}

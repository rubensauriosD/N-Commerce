namespace IServicio.Seguridad
{
    using IServicio.Usuario.DTOs;

    public interface ISeguridadServicio
    {
        bool VerificarAcceso(string usuario, string password);

        UsuarioDto ObtenerUsuarioLogin(string nombreUsuario);

        bool ModificarPassword(long usuarioId, string password);
    }
}

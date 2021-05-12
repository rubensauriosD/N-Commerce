namespace IServicio.Usuario
{
    using IServicio.Base;

    public interface IUsuarioServicio : IServicioConsulta
    {
        void Crear(long empleadoId, string apellidoEmpleado, string nombreEmpleado);

        void Bloquear(long usuarioId);

        void ResetPassword(long usuarioId);
    }
}

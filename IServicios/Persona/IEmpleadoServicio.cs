namespace IServicio.Persona
{
    using System.Drawing;

    public interface IEmpleadoServicio : IPersonaServicio
    {
        int ObtenerSiguienteLegajo();

        bool VerificarSiExisteDni(string dni, long? entidadId = null);

        bool ModificarFoto(long id, byte[] foto);
    }
}

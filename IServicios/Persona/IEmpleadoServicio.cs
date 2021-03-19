namespace IServicio.Persona
{
    public interface IEmpleadoServicio : IPersonaServicio
    {
        int ObtenerSiguienteLegajo();

        bool VerificarSiExisteDni(string dni, long? entidadId = null);
    }
}

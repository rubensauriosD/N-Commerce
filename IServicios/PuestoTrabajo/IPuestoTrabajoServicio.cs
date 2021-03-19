namespace IServicio.PuestoTrabajo
{
    public interface IPuestoTrabajoServicio : Base.IServicio
    {
        bool VerificarSiExiste(string datoVerificar, long? entidadId = null);

        int ProximoCodigo();
    }
}

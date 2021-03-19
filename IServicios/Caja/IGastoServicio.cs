namespace IServicio.Caja
{
    public interface IGastoServicio : Base.IServicio
    {
        bool VerificarSiExiste(string datoVerificar, long? entidadId = null);
    }
}

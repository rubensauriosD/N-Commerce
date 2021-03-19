namespace IServicio.Caja
{
    public interface IConceptoGastoServicio : Base.IServicio
    {
        bool VerificarSiExiste(string datoVerificar, long? entidadId = null);
    }
}

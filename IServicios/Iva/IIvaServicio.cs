namespace IServicio.Iva
{
    public interface IIvaServicio : Base.IServicio
    {
        bool VerificarSiExiste(string datoVerificar, long? entidadId = null);
    }
}

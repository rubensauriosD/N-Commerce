namespace IServicio.FormaPago
{
    public interface IBancoServicio : Base.IServicio
    {
        bool VerificarSiExiste(string datoVerificar, long? entidadId = null);
    }

}

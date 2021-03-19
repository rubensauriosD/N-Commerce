namespace IServicio.FormaPago
{
    public interface ITarjetaServicio : Base.IServicio
    {
        bool VerificarSiExiste(string datoVerificar, long? entidadId = null);
    }

}
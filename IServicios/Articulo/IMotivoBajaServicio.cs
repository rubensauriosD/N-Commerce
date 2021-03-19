namespace IServicio.Articulo
{
    public interface IMotivoBajaServicio : Base.IServicio
    {
        bool VerificarSiExiste(string datoVerificar, long? entidadId = null);
    }
}
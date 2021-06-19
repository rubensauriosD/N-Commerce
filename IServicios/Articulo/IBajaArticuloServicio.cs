namespace IServicios.Articulo
{
    public interface IBajaArticuloServicio : IServicio.Base.IServicio
    {
        bool RevertirBaja(long id);
    }
}

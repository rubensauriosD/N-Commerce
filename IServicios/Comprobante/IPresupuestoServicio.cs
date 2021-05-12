namespace IServicios.Comprobante
{
    public interface IPresupuestoServicio : IComprobanteServicio
    {
        int ObtenerNumeroPresupuesto(long id);
    }
}

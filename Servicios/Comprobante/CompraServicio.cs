namespace Servicios.Comprobante
{
    using Dominio.UnidadDeTrabajo;
    using IServicios.Comprobante;

    public class CompraServicio : ComprobanteServicio, ICompraServicio
    {
        public CompraServicio(IUnidadDeTrabajo unidadDeTrabajo)
            : base(unidadDeTrabajo)
        {
        }
    }
}

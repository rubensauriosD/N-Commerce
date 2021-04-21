namespace IServicios.Comprobante
{
    using IServicios.Comprobante.DTOs;
    using IServicios.Caja.DTOs;
    using IServicios.Persona.DTOs;

    public interface ICompraServicio : IComprobanteServicio
    {
        bool InsertarDetalleCaja(CajaDetalleDto detalle);

        bool InsertarMovimientoCuentaCorriente(MovimientoCuentaCorrienteProveedorDto movimiento);
    }
}

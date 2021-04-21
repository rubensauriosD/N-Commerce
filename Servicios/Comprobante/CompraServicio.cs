namespace Servicios.Comprobante
{
    using Dominio.UnidadDeTrabajo;
    using IServicios.Caja.DTOs;
    using IServicios.Comprobante;
    using IServicios.Persona.DTOs;
    using System;

    public class CompraServicio : ComprobanteServicio, ICompraServicio
    {
        public CompraServicio(IUnidadDeTrabajo unidadDeTrabajo)
            : base(unidadDeTrabajo)
        {
        }

        public bool InsertarDetalleCaja(CajaDetalleDto detalle)
        {
            try
            {
                var caja = _unidadDeTrabajo.CajaRepositorio.Obtener(detalle.CajaId, "DetalleCajas");
                caja.DetalleCajas.Add(new Dominio.Entidades.DetalleCaja()
                {
                    TipoPago = detalle.TipoPago,
                    TipoMovimiento = detalle.TipoMovimiento,
                    Monto = detalle.Monto,
                    EstaEliminado = false
                });

                _unidadDeTrabajo.Commit();
                return true;
            }
            catch (Exception e)
            {
                throw new Exception("Error al intentar grabar el movimiento cuenta corriente.");
            }

        }
        public bool InsertarMovimientoCuentaCorriente(MovimientoCuentaCorrienteProveedorDto movimiento)
        {
            try
            {
                var proveedor = _unidadDeTrabajo.ProveedorRepositorio.Obtener(movimiento.ProveedorId , "MovimientoCuentaCorrienteProveedores");
                proveedor.MovimientoCuentaCorrienteProveedores.Add(new Dominio.Entidades.MovimientoCuentaCorrienteProveedor()
                {
                    ProveedorId = movimiento.ProveedorId,
                    Descripcion = movimiento.Descripcion,
                    TipoMovimiento = movimiento.TipoMovimiento,
                    Monto = movimiento.Monto,
                    Fecha = movimiento.Fecha,
                    EstaEliminado = false
                });

                _unidadDeTrabajo.Commit();
                return true;
            }
            catch (Exception e)
            {
                throw new Exception("Error al intentar grabar el movimiento cuenta corriente.");
            }

        }
    }
}

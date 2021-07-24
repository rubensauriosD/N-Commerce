namespace Servicios.Comprobante
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Aplicacion.Constantes;
    using Dominio.UnidadDeTrabajo;
    using IServicios.Comprobante;
    using IServicios.Comprobante.DTOs;

    public class FacturaServicio : ComprobanteServicio, IFacturaServicio
    {
        public FacturaServicio(IUnidadDeTrabajo unidadDeTrabajo) 
            : base(unidadDeTrabajo)
        {
        }

        public FacturaDto Obtener(long id)
        {
            try
            {
                var factura = _unidadDeTrabajo.FacturaRepositorio
                    .Obtener(id, "Cliente, DetalleComprobantes");

                if (factura == null)
                {
                    Mjs.Error("Error al obtener el comprobante.");
                    return new FacturaDto();
                }

                return new FacturaDto() { 
                        Id = factura.Id,
                        ClienteId = factura.ClienteId,
                        Fecha = factura.Fecha,
                        Numero = factura.Numero,
                        Eliminado = factura.EstaEliminado,
                        Items = factura.DetalleComprobantes.Select(d => new DetalleComprobanteDto()
                        {
                            Id = d.Id,
                            Eliminado = d.EstaEliminado,
                            Codigo = d.Codigo,
                            Descripcion = d.Descripcion,
                            Cantidad = d.Cantidad,
                            Iva = d.Iva,
                            Precio = d.Precio,
                        }).ToList()
                };

            }
            catch (Exception e)
            {
                throw new Exception("FacturaServicio.Obtener: No se pudo obtener el comprobante.");
            }
        }

        public IEnumerable<ComprobantePendienteDto> ObtenerPendientesPago()
        {
            return _unidadDeTrabajo.FacturaRepositorio
                .Obtener(x => !x.EstaEliminado && x.Estado == Aplicacion.Constantes.Estado.Pendiente, "Cliente, DetalleComprobantes")
                .Select(x => new ComprobantePendienteDto() {
                    Id = x.Id,
                    ClienteId = x.ClienteId,
                    Cliente = $@"{x.Cliente.Apellido} {x.Cliente.Nombre}",
                    Fecha = x.Fecha,
                    MontoPagar = x.Total,
                    NumeroComprobante = x.Numero,
                    Eliminado = x.EstaEliminado,
                    Items = x.DetalleComprobantes.Select(d => new ComprobantePendienteDetalleDto() {
                        Id = d.Id,
                        Cantidad = d.Cantidad,
                        Descripcion = d.Descripcion,
                        Precio = d.Precio,
                    }).ToList()
                })
                .OrderBy(x => x.Fecha)
                .ToList();
        }
    }
}

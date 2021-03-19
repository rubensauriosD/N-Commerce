namespace Servicios.Comprobante
{
    using System.Collections.Generic;
    using System.Linq;
    using Dominio.UnidadDeTrabajo;
    using IServicios.Comprobante;
    using IServicios.Comprobante.DTOs;

    public class FacturaServicio : ComprobanteServicio, IFacturaServicio
    {
        public FacturaServicio(IUnidadDeTrabajo unidadDeTrabajo) 
            : base(unidadDeTrabajo)
        {
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

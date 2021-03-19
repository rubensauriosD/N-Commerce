namespace Servicios.Comprobante
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Transactions;
    using Aplicacion.Constantes;
    using Dominio.Entidades;
    using IServicio.Configuracion;
    using IServicios.Comprobante.DTOs;
    using IServicios.Contador;
    using StructureMap;

    public class Factura : Comprobante
    {
        private readonly IContadorServicio _contadorServicio;
        private readonly IConfiguracionServicio _configuracionServicio;

        public Factura() : base()
        {
            _contadorServicio = ObjectFactory.GetInstance<IContadorServicio>();
            _configuracionServicio = ObjectFactory.GetInstance<IConfiguracionServicio>();
        }

        public override long Insertar(ComprobanteDto comprobante)
        {
            using (var tran = new TransactionScope())
            {
                try
                {
                    // Preparar los datos que necesito
                    var facturaDto = (FacturaDto)comprobante;
                    var numeroComprobante = _contadorServicio.ObtenerSiguienteNumero(comprobante.TipoComprobante);

                    var config = _configuracionServicio.Obtener();
                    
                    if (config == null)
                        throw new Exception("Ocurrio un error al obtener la Configuración");

                    Dominio.Entidades.Factura _facturaNueva = new Dominio.Entidades.Factura
                    {
                        Fecha = DateTime.Now,
                        Numero = numeroComprobante,
                        Estado = Estado.Pendiente,
                        ClienteId = facturaDto.ClienteId,
                        Descuento = facturaDto.Descuento,
                        EmpleadoId = facturaDto.EmpleadoId,
                        Iva105 = facturaDto.Iva105,
                        Iva21 = facturaDto.Iva21,
                        SubTotal = facturaDto.SubTotal,
                        Total = facturaDto.Total,
                        PuestoTrabajoId = facturaDto.PuestoTrabajoId,
                        TipoComprobante = facturaDto.TipoComprobante,
                        UsuarioId = facturaDto.UsuarioId,
                        DetalleComprobantes = facturaDto.Items.Select(item => new DetalleComprobante() {
                            Cantidad = item.Cantidad,
                            ArticuloId = item.ArticuloId,
                            Iva = item.Iva,
                            Descripcion = item.Descripcion,
                            Precio = item.Precio,
                            Codigo = item.Codigo,
                            SubTotal = item.SubTotal
                        }).ToList(),
                        FormaPagos = new List<FormaPago>(),
                        EstaEliminado = false
                    };

                    if (config.FacturaDescuentaStock)
                    {
                        foreach (var item in facturaDto.Items)
                        {
                            var stockActual = _unidadDeTrabajo.StockRepositorio
                                .Obtener(x => x.ArticuloId == item.ArticuloId && x.DepositoId == config.DepositoVentaId)
                                .FirstOrDefault();

                            if (stockActual == null)
                                throw new Exception("Ocurrio un error al obtener el Stock del Articulo");

                            if (stockActual.Cantidad >= item.Cantidad)
                                stockActual.Cantidad -= item.Cantidad;

                            _unidadDeTrabajo.StockRepositorio.Modificar(stockActual);
                        }
                    }

                    _unidadDeTrabajo.FacturaRepositorio.Insertar(_facturaNueva);
                    _unidadDeTrabajo.Commit();
                    tran.Complete();

                    return _facturaNueva.Id;
                }
                catch
                {
                    tran.Dispose();
                    throw new Exception("Ocurrio un error en FacturaServicios.Insertar");
                }
            }
        }

    }
}

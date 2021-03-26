namespace Servicios.Comprobante
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Transactions;
    using Dominio.Entidades;
    using IServicio.Configuracion;
    using IServicios.Comprobante.DTOs;
    using StructureMap;

    public class Compra : Comprobante
    {
        private readonly IConfiguracionServicio _configuracionServicio;

        public Compra() : base()
        {
            _configuracionServicio = ObjectFactory.GetInstance<IConfiguracionServicio>();
        }

        public override long Insertar(ComprobanteDto comprobante)
        {
            using (var tran = new TransactionScope())
            {
                try
                {
                    // Preparar los datos que necesito
                    var compraDto = (CompraDto)comprobante;

                    var config = _configuracionServicio.Obtener();

                    if (config == null)
                        throw new Exception("Ocurrio un error al obtener la Configuración");

                    Dominio.Entidades.Compra _compraNueva = new Dominio.Entidades.Compra
                    {
                        Fecha = compraDto.Fecha,
                        Numero = compraDto.Numero,
                        ProveedorId = compraDto.ProveedorId,
                        Descuento = compraDto.Descuento,
                        EmpleadoId = compraDto.EmpleadoId,
                        Iva105 = compraDto.Iva105,
                        Iva21 = compraDto.Iva21,
                        SubTotal = compraDto.SubTotal,
                        Total = compraDto.Total,
                        TipoComprobante = compraDto.TipoComprobante,
                        UsuarioId = compraDto.UsuarioId,
                        DetalleComprobantes = compraDto.Items.Select(item => new DetalleComprobante()
                        {
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

                    foreach (var item in compraDto.Items)
                    {
                        // Actualizar Stock
                        var stockActual = _unidadDeTrabajo.StockRepositorio
                            .Obtener(x => x.ArticuloId == item.ArticuloId && x.DepositoId == config.DepositoNuevoArticuloId)
                            .FirstOrDefault();
                    
                        if (stockActual == null)
                            throw new Exception("Ocurrio un error al obtener el Stock del Articulo");
                    
                        stockActual.Cantidad += item.Cantidad;
                    
                        _unidadDeTrabajo.StockRepositorio.Modificar(stockActual);
                    
                        // Actualizar Precio
                        if (config.ActualizaCostoDesdeCompra)
                            _unidadDeTrabajo.PrecioRepositorio.Insertar(new Precio { 
                                ArticuloId = item.ArticuloId,
                                PrecioCosto = item.Precio,
                                FechaActualizacion = DateTime.Now
                            });
                    }

                    _unidadDeTrabajo.CompraRepositorio.Insertar(_compraNueva);
                    _unidadDeTrabajo.Commit();
                    tran.Complete();

                    return _compraNueva.Id;
                }
                catch
                {
                    tran.Dispose();
                    throw new Exception("Ocurrio un error en CompraServicios.Insertar");
                }
            }
        }
    }
}

namespace Servicios.Comprobante
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Transactions;
    using StructureMap;
    using Dominio.Entidades;
    using IServicio.Configuracion;
    using IServicios.Comprobante.DTOs;
    using IServicios.Contador;

    public class Presupuesto : Comprobante
    {
        private readonly IContadorServicio _contadorServicio;
        private readonly IConfiguracionServicio _configuracionServicio;

        public Presupuesto() : base()
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
                    var presupuestoDto = (PresupuestoDto)comprobante;
                    var numeroComprobante = _contadorServicio.ObtenerSiguienteNumero(comprobante.TipoComprobante);

                    var config = _configuracionServicio.Obtener();

                    if (config == null)
                        throw new Exception("Ocurrio un error al obtener la Configuración");

                    Dominio.Entidades.Presupuesto _presupuestoNueva = new Dominio.Entidades.Presupuesto
                    {
                        Fecha = DateTime.Now,
                        Numero = numeroComprobante,
                        ClienteId = presupuestoDto.ClienteId,
                        Descuento = presupuestoDto.Descuento,
                        EmpleadoId = presupuestoDto.EmpleadoId,
                        Iva105 = presupuestoDto.Iva105,
                        Iva21 = presupuestoDto.Iva21,
                        SubTotal = presupuestoDto.SubTotal,
                        Total = presupuestoDto.Total,
                        TipoComprobante = presupuestoDto.TipoComprobante,
                        UsuarioId = presupuestoDto.UsuarioId,
                        DetalleComprobantes = presupuestoDto.Items.Select(item => new DetalleComprobante()
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

                    if (config.PresupuestoDescuentaStock)
                    {
                        foreach (var item in presupuestoDto.Items)
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

                    _unidadDeTrabajo.PresupuestoRepositorio.Insertar(_presupuestoNueva);
                    _unidadDeTrabajo.Commit();
                    tran.Complete();

                    return _presupuestoNueva.Id;
                }
                catch(Exception e)
                {
                    tran.Dispose();
                    throw new Exception($"Ocurrio un error en PresupuestoServicios.Insertar{Environment.NewLine}{e.Message}");
                }
            }
        }

    }
}

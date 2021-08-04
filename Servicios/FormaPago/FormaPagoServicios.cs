namespace Servicios.FormaPago
{
    using System.Collections.Generic;
    using System.Linq;
    using System;
    using System.Transactions;
    using Aplicacion.Constantes;
    using Dominio.UnidadDeTrabajo;
    using Dominio.Entidades;
    using IServicios.FormaPago.DTOs;
    using IServicios.FormaPago;
    using StructureMap;

    public class FormaPagoServicios : IFormaPagoServicios
    {
        protected readonly IUnidadDeTrabajo _unidadDeTrabajo;

        public FormaPagoServicios()
        {
            _unidadDeTrabajo = ObjectFactory.GetInstance<IUnidadDeTrabajo>();
        }

        public void Insertar(List<FormaPagoDto> pagos, long facturaId)
        {
            using (var tran = new TransactionScope())
            {
                try
                {
                    var factura = _unidadDeTrabajo.FacturaRepositorio.Obtener(facturaId, "Cliente, Cliente.CuentaCorriente, FormaPagos");

                    var caja = _unidadDeTrabajo.CajaRepositorio
                        .Obtener(x => x.UsuarioAperturaId == factura.UsuarioId && x.UsuarioCierreId == null, "DetalleCajas")
                        .FirstOrDefault();

                    if (caja == null)
                        throw new Exception("Error al encontrar la caja en FormaPagoServicios.Insertar");


                    foreach (var fp in pagos)
                    {
                        switch (fp.TipoPago)
                        {
                            case TipoPago.Efectivo:
                                AgregarNuevoPagoEfectivo(factura, fp, caja);
                                break;

                            case TipoPago.Tarjeta:
                                AgregarNuevoPagoTarjeta(factura, (FormaPagoTarjetaDto)fp, caja);
                                        break;

                            case TipoPago.Cheque:
                                AgregarNuevoPagoCheque(factura, (FormaPagoChequeDto)fp, caja);
                                break;

                            case TipoPago.CtaCte:
                                AgregarNuevoPagoCtaCte(factura, (FormaPagoCtaCteDto)fp);
                                break;
                        }
                    }

                    factura.Estado = Estado.Pagada;

                    _unidadDeTrabajo.CajaRepositorio.Modificar(caja);
                    _unidadDeTrabajo.FacturaRepositorio.Modificar(factura);
                    _unidadDeTrabajo.Commit();
                    tran.Complete();
                }
                catch(Exception e)
                {
                    tran.Dispose();
                    throw new Exception("Ocurrio un error en FormaPagoServicios.Insertar");
                }
            }
        }

        private void AgregarNuevoPagoCtaCte(Factura factura, FormaPagoCtaCteDto formaPago)
        {
            factura.FormaPagos.Add(new FormaPagoCtaCte
            {
                Monto = formaPago.Monto,
                TipoPago = TipoPago.CtaCte,
                EstaEliminado = false,
                MovimientoCuentaCorriente = new MovimientoCuentaCorriente
                {
                    Fecha = factura.Fecha,
                    ClienteId = formaPago.ClienteId,
                    Monto = formaPago.Monto,
                    TipoMovimiento = TipoMovimiento.Ingreso,
                    Descripcion = $"F{factura.TipoComprobante} - {factura.Numero.ToString("0000")}",
                    EstaEliminado = false
                }

            });
        }

        private void AgregarNuevoPagoCheque(Factura factura, FormaPagoChequeDto formaPago, Caja caja)
        {
            factura.FormaPagos.Add(new FormaPagoCheque
            {
                Monto = formaPago.Monto,
                TipoPago = TipoPago.Cheque,
                Cheque = new Cheque
                {
                    BancoId = formaPago.BancoId,
                    ClienteId = factura.ClienteId,
                    FechaVencimiento = formaPago.FechaVencimiento,
                    Numero = formaPago.Numero,
                    EstaEliminado = false
                }
            });

            caja.DetalleCajas.Add(new DetalleCaja
            {
                Monto = formaPago.Monto,
                TipoPago = TipoPago.Cheque,
                TipoMovimiento = TipoMovimiento.Ingreso,
                EstaEliminado = false
            });

        }

        private void AgregarNuevoPagoTarjeta(Factura factura, FormaPagoTarjetaDto formaPago, Caja caja)
        {
            factura.FormaPagos.Add(new FormaPagoTarjeta
            {
                Monto = formaPago.Monto,
                TipoPago = formaPago.TipoPago,
                CantidadCuotas = formaPago.CantidadCuotas,
                CuponPago = formaPago.CuponPago,
                NumeroTarjeta = formaPago.NumeroTarjeta,
                TarjetaId = formaPago.TarjetaId,
                EstaEliminado = false
            });

            caja.DetalleCajas.Add(new DetalleCaja
            {
                Monto = formaPago.Monto,
                TipoPago = TipoPago.Tarjeta,
                TipoMovimiento = TipoMovimiento.Ingreso,
                EstaEliminado = false
            });
        }

        private void AgregarNuevoPagoEfectivo(Factura factura, FormaPagoDto formaPago, Caja caja)
        {
            factura.FormaPagos.Add(new FormaPago
            {
                Monto = formaPago.Monto,
                TipoPago = formaPago.TipoPago,
                EstaEliminado = false
            });

            caja.DetalleCajas.Add(new DetalleCaja
            {
                Monto = formaPago.Monto,
                TipoPago = TipoPago.Efectivo,
                TipoMovimiento = TipoMovimiento.Ingreso,
                EstaEliminado = false
            });
        }
    }
}

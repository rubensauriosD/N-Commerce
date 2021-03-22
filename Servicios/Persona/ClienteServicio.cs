namespace Servicios.Persona
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Aplicacion.Constantes;
    using Dominio.Entidades;
    using Dominio.UnidadDeTrabajo;
    using IServicio.Persona;
    using IServicios.Persona.DTOs;

    public class ClienteServicio : PersonaServicio, IClienteServicio
    {
        public ClienteServicio(IUnidadDeTrabajo unidadDeTrabajo) 
            : base(unidadDeTrabajo)
        {

        }

        public bool VerificarSiExisteDni(string datoVerificar, long? entidadId = null)
        {
            return entidadId.HasValue
                ? _unidadDeTrabajo.ClienteRepositorio.Obtener(x => !x.EstaEliminado
                                                                     && x.Id != entidadId.Value
                                                                     && x.Dni.Equals(datoVerificar,
                                                                         StringComparison.CurrentCultureIgnoreCase))
                    .Any()
                : _unidadDeTrabajo.ClienteRepositorio.Obtener(x => !x.EstaEliminado
                                                                     && x.Dni.Equals(datoVerificar,
                                                                         StringComparison.CurrentCultureIgnoreCase))
                    .Any();
        }

        public decimal SaldoCuentaCorriente(long clienteId)
        {
            try
            {
                return _unidadDeTrabajo.ClienteRepositorio
                    .Obtener(clienteId, "CuentaCorriente")
                    .CuentaCorriente.Sum(m => m.Monto * (m.TipoMovimiento == TipoMovimiento.Ingreso ? 1 : -1));
            }
            catch (Exception ex)
            {
                throw new Exception("Error en ClienteServicios.SaldoCuentaCorriente" +
                    $"{Environment.NewLine} {ex.Message}");
            }
        }

        public bool AgregarPagoCuentaCorriente(MovimientoCuentaCorrienteClienteDto pago)
        {
            try
            {
                var cliente = _unidadDeTrabajo.ClienteRepositorio.Obtener(pago.ClienteId, "CuentaCorriente");
                var caja = _unidadDeTrabajo.CajaRepositorio.Obtener(pago.CajaId, "DetalleCajas");

                if (cliente == null || caja == null)
                    return false;

                cliente.CuentaCorriente.Add(new MovimientoCuentaCorriente()
                {
                    Descripcion = pago.Descripcion,
                    Monto = pago.Monto,
                    Fecha = DateTime.Now,
                    TipoMovimiento = TipoMovimiento.Ingreso,
                    EstaEliminado = false
                });

                caja.DetalleCajas.Add(new DetalleCaja()
                {
                    TipoPago = TipoPago.Efectivo,
                    TipoMovimiento = TipoMovimiento.Ingreso,
                    Monto = pago.Monto,
                });

                _unidadDeTrabajo.Commit();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool RevertirPagoCuentaCorriente(MovimientoCuentaCorrienteClienteDto pago)
        {
            try
            {
                var cliente = _unidadDeTrabajo.ClienteRepositorio.Obtener(pago.ClienteId, "CuentaCorriente");
                var caja = _unidadDeTrabajo.CajaRepositorio.Obtener(pago.CajaId, "DetalleCajas");

                if (cliente == null || caja == null)
                    return false;

                cliente.CuentaCorriente.Add(new MovimientoCuentaCorriente()
                {
                    Descripcion = pago.Descripcion,
                    Monto = pago.Monto,
                    Fecha = DateTime.Now,
                    TipoMovimiento = TipoMovimiento.Egreso,
                    EstaEliminado = false
                });

                caja.DetalleCajas.Add(new DetalleCaja()
                {
                    TipoPago = TipoPago.Efectivo,
                    TipoMovimiento = TipoMovimiento.Egreso,
                    Monto = pago.Monto,
                });

                _unidadDeTrabajo.Commit();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<MovimientoCuentaCorrienteClienteDto> ObtenerMovimientosCuentaCorriente(long id)
        {
            var lstMovimientos = _unidadDeTrabajo.ClienteRepositorio.Obtener(id, "MovimientoCuentaCorriente")
                .CuentaCorriente
                .ToList();

            var cuentaCorriente = new List<MovimientoCuentaCorrienteClienteDto>();

            if (lstMovimientos.Count > 0)
                cuentaCorriente = lstMovimientos
                    .Select(m => new MovimientoCuentaCorrienteClienteDto()
                    {
                        Id = m.Id,
                        Eliminado = m.EstaEliminado,
                        Monto = m.Monto,
                        Descripcion = m.Descripcion,
                        TipoMovimiento = m.TipoMovimiento,
                        Fecha = m.Fecha
                    })
                    .ToList();

            return cuentaCorriente;
        }
    }
}

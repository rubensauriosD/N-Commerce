namespace Servicios.Persona
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Dominio.Entidades;
    using Dominio.UnidadDeTrabajo;
    using IServicio.BaseDto;
    using IServicio.Persona;
    using IServicio.Persona.DTOs;
    using IServicios.Persona.DTOs;
    using Servicios.Base;

    public class ProveedorServicios : IProveedorServicio
    {
        private readonly IUnidadDeTrabajo _unidadDeTrabajo;

        public ProveedorServicios(IUnidadDeTrabajo unidadDeTrabajo)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
        }

        // PERSISTENCIA
        public void Eliminar(long id)
        {
            _unidadDeTrabajo.ProveedorRepositorio.Eliminar(id);
            _unidadDeTrabajo.Commit();
        }

        public void Insertar(DtoBase dtoEntidad)
        {
            var entidad = Map((ProveedorDto)dtoEntidad);
            entidad.EstaEliminado = false;

            _unidadDeTrabajo.ProveedorRepositorio.Insertar(entidad);
            _unidadDeTrabajo.Commit();
        }

        public void Modificar(DtoBase dtoEntidad)
        {
            var dto = (ProveedorDto)dtoEntidad;

            var entidad = _unidadDeTrabajo.ProveedorRepositorio.Obtener(dtoEntidad.Id);

            if (entidad == null) throw new Exception("Ocurrio un Error al Obtener la Proveedor");

            entidad.RazonSocial = dto.RazonSocial;
            entidad.CUIT = dto.CUIT;
            entidad.Direccion = dto.Direccion;
            entidad.Telefono = dto.Telefono;
            entidad.Mail = dto.Mail;
            entidad.LocalidadId = dto.LocalidadId;
            entidad.CondicionIvaId = dto.CondicionIvaId;
            entidad.EstaEliminado = false;

            _unidadDeTrabajo.ProveedorRepositorio.Modificar(entidad);
            _unidadDeTrabajo.Commit();
        }

        public bool AgregarPagoCuentaCorriente(MovimientoCuentaCorrienteProveedorDto pago)
        {
            try
            {
                var proveedor = _unidadDeTrabajo.ProveedorRepositorio.Obtener(pago.ProveedorId, "MovimientoCuentaCorrienteProveedores");
                var caja = _unidadDeTrabajo.CajaRepositorio.Obtener(pago.CajaId, "DetalleCajas");

                if (proveedor == null || caja == null)
                    return false;

                proveedor.MovimientoCuentaCorrienteProveedores.Add(new MovimientoCuentaCorrienteProveedor()
                {
                    Descripcion = pago.Descripcion,
                    Monto = pago.Monto,
                    Fecha = DateTime.Now,
                    TipoMovimiento = Aplicacion.Constantes.TipoMovimiento.Ingreso,
                    EstaEliminado = false
                });

                caja.DetalleCajas.Add(new DetalleCaja() { 
                    TipoPago = Aplicacion.Constantes.TipoPago.Efectivo,
                    TipoMovimiento = Aplicacion.Constantes.TipoMovimiento.Ingreso,
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

        public bool RevertirPagoCuentaCorriente(MovimientoCuentaCorrienteProveedorDto pago)
        {
            try
            {
                var proveedor = _unidadDeTrabajo.ProveedorRepositorio.Obtener(pago.ProveedorId, "MovimientoCuentaCorrienteProveedores");
                var caja = _unidadDeTrabajo.CajaRepositorio.Obtener(pago.CajaId, "DetalleCajas");

                if (proveedor == null || caja == null)
                    return false;

                proveedor.MovimientoCuentaCorrienteProveedores.Add(new MovimientoCuentaCorrienteProveedor()
                {
                    Descripcion = "Reversion Pago",
                    Monto = pago.Monto,
                    Fecha = DateTime.Now,
                    TipoMovimiento = Aplicacion.Constantes.TipoMovimiento.Egreso,
                    EstaEliminado = false
                });

                caja.DetalleCajas.Add(new DetalleCaja() { 
                    TipoPago = Aplicacion.Constantes.TipoPago.Efectivo,
                    TipoMovimiento = Aplicacion.Constantes.TipoMovimiento.Egreso,
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

        // CONSULTA
        public DtoBase Obtener(long id)
        {
            var entidad = _unidadDeTrabajo.ProveedorRepositorio.Obtener(id);

            return Map(entidad);
        }

        public IEnumerable<DtoBase> Obtener(string cadenaBuscar, bool mostrarTodos = true)
        {
            Expression<Func<Proveedor, bool>> filtro =
                x => x.RazonSocial.Contains(cadenaBuscar);

            if (!mostrarTodos)
                filtro = filtro.And(x => !x.EstaEliminado);

            return _unidadDeTrabajo.ProveedorRepositorio.Obtener(filtro, "MovimientoCuentaCorrienteProveedores")
                .Select(x => Map(x))
                .OrderBy(x => x.RazonSocial)
                .ToList();
        }

        public bool VerificarSiExiste(string datoVerificar, long? entidadId = null)
        {
            return entidadId.HasValue
                ? _unidadDeTrabajo.ProveedorRepositorio.Obtener(x => !x.EstaEliminado
                                                                        && x.Id != entidadId.Value
                                                                        && x.RazonSocial.Equals(datoVerificar,
                                                                            StringComparison.CurrentCultureIgnoreCase))
                    .Any()
                : _unidadDeTrabajo.ProveedorRepositorio.Obtener(x => !x.EstaEliminado
                                                                        && x.RazonSocial.Equals(datoVerificar,
                                                                            StringComparison.CurrentCultureIgnoreCase))
                    .Any();
        }

        public bool VerificarSiExisteCuit(string datoVerificar, long? entidadId = null)
        {
            return entidadId.HasValue
                ? _unidadDeTrabajo.ProveedorRepositorio.Obtener(x => !x.EstaEliminado
                                                                        && x.Id != entidadId.Value
                                                                        && x.CUIT.Equals(datoVerificar,
                                                                            StringComparison.CurrentCultureIgnoreCase))
                    .Any()
                : _unidadDeTrabajo.ProveedorRepositorio.Obtener(x => !x.EstaEliminado
                                                                        && x.CUIT.Equals(datoVerificar,
                                                                            StringComparison.CurrentCultureIgnoreCase))
                    .Any();
        }

        public List<MovimientoCuentaCorrienteProveedorDto> ObtenerMovimientosCuentaCorriente(long id)
        {
            var lstMovimientos = _unidadDeTrabajo.MovimientoCuentaCorrienteProveedorRepositorio
                .Obtener(x => x.ProveedorId == id)
                .ToList();

            var cuentaCorriente = new List<MovimientoCuentaCorrienteProveedorDto>();

            if (lstMovimientos.Count > 0)
                cuentaCorriente = lstMovimientos
                    .Select(m => new MovimientoCuentaCorrienteProveedorDto()
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

        // PRIVADAS
        private ProveedorDto Map(Proveedor entidad)
        {
            return new ProveedorDto
            {
                Id = entidad.Id,
                RazonSocial = entidad.RazonSocial,
                CUIT = entidad.CUIT,
                Direccion = entidad.Direccion,
                Telefono = entidad.Telefono,
                Mail = entidad.Mail,
                LocalidadId = entidad.LocalidadId,
                CondicionIvaId = entidad.CondicionIvaId,
                Eliminado = entidad.EstaEliminado
            };
        }

        private Proveedor Map(ProveedorDto dto) 
        {
            return new Proveedor
            {
                Id = dto.Id,
                RazonSocial = dto.RazonSocial,
                CUIT = dto.CUIT,
                Direccion = dto.Direccion,
                Telefono = dto.Telefono,
                Mail = dto.Mail,
                LocalidadId = dto.LocalidadId,
                CondicionIvaId = dto.CondicionIvaId,
                EstaEliminado = dto.Eliminado
            };
        }

    }
}

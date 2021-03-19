namespace Servicios.MovimientoCuentaCorrienteProveedor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using IServicios.Persona.DTOs;
    using Servicios.Persona;

    public class MovimientoCuentaCorrienteProveedor : Movimiento
    {
        public override long Insertar(MovimientoDto entidad)
        {
            if (entidad == null)
                throw new Exception("Ocurrio un error al Insertar el MovimientoCuentaCorrienteProveedor");

            var entidadNueva = (MovimientoCuentaCorrienteProveedorDto)entidad;

            var entidadId = _unidadDeTrabajo.MovimientoCuentaCorrienteProveedorRepositorio.Insertar(new Dominio.Entidades.MovimientoCuentaCorrienteProveedor
            {
                EstaEliminado = false,
            });

            _unidadDeTrabajo.Commit();

            return entidadId;
        }

        public override void Modificar(MovimientoDto entidad)
        {
            if (entidad == null)
                throw new Exception("Ocurrio un error al modificar el MovimientoCuentaCorrienteProveedor");

            var entidadModificar = (MovimientoCuentaCorrienteProveedorDto)entidad;

            _unidadDeTrabajo.MovimientoCuentaCorrienteProveedorRepositorio.Modificar(new Dominio.Entidades.MovimientoCuentaCorrienteProveedor
            {
                Id = entidadModificar.Id,
                EstaEliminado = false,
                Descripcion = entidadModificar.Descripcion,
                Fecha = entidadModificar.Fecha,
                Monto = entidadModificar.Monto,
                TipoMovimiento = entidadModificar.TipoMovimiento
            });

            _unidadDeTrabajo.Commit();
        }

        public override void Eliminar(long id)
        {
            _unidadDeTrabajo.MovimientoCuentaCorrienteProveedorRepositorio.Eliminar(id);
            _unidadDeTrabajo.Commit();
        }

        public override IEnumerable<MovimientoDto> Obtener(string cadenaBuscar)
        {
            Expression<Func<Dominio.Entidades.MovimientoCuentaCorrienteProveedor, bool>> filtro = cliente =>
                    cliente.Descripcion.Contains(cadenaBuscar);

            return _unidadDeTrabajo.MovimientoCuentaCorrienteProveedorRepositorio.Obtener(filtro)
                    .Select(x => new MovimientoCuentaCorrienteProveedorDto
                    {
                        Id = x.Id,
                        Eliminado = x.EstaEliminado,
                        Descripcion = x.Descripcion,
                        Fecha = x.Fecha,
                        Monto = x.Monto,
                        TipoMovimiento = x.TipoMovimiento
                    }).OrderByDescending(x => x.Fecha)
                    .ToList();
        }

        public override MovimientoDto Obtener(long id)
        {
            var entidad = _unidadDeTrabajo.MovimientoCuentaCorrienteProveedorRepositorio.Obtener(id);

            return new MovimientoCuentaCorrienteProveedorDto
            {
                Id = entidad.Id,
                Eliminado = entidad.EstaEliminado,
                Descripcion = entidad.Descripcion,
                Fecha = entidad.Fecha,
                Monto = entidad.Monto,
                TipoMovimiento = entidad.TipoMovimiento
            };
        }
    }
}

namespace Servicios.Caja
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Dominio.UnidadDeTrabajo;
    using IServicio.BaseDto;
    using IServicio.Caja;
    using IServicio.Caja.DTOs;
    using Servicios.Base;

    public class GastoServicio : IGastoServicio
    {
        private readonly IUnidadDeTrabajo _unidadDeTrabajo;

        public GastoServicio(IUnidadDeTrabajo unidadDeTrabajo)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
        }

        public void Eliminar(long id)
        {
            _unidadDeTrabajo.GastoRepositorio.Eliminar(id);
            _unidadDeTrabajo.Commit();
        }

        public void Insertar(DtoBase dtoEntidad)
        {
            var dto = (GastoDto)dtoEntidad;

            var entidad = new Dominio.Entidades.Gasto
            {
                Descripcion = dto.Descripcion,
                ConceptoGastoId = dto.ConceptoGastoId,
                Fecha = dto.Fecha,
                Monto = dto.Monto,
                EstaEliminado = false
            };

            _unidadDeTrabajo.GastoRepositorio.Insertar(entidad);
            _unidadDeTrabajo.Commit();
        }

        public void Modificar(DtoBase dtoEntidad)
        {
            var dto = (GastoDto)dtoEntidad;

            var entidad = _unidadDeTrabajo.GastoRepositorio.Obtener(dto.Id);

            if (entidad == null) throw new Exception("Ocurrio un Error al Obtener la Gasto");

            entidad.Descripcion = dto.Descripcion;
            entidad.ConceptoGastoId = dto.ConceptoGastoId;
            entidad.Fecha = dto.Fecha;
            entidad.Monto = dto.Monto;

            _unidadDeTrabajo.GastoRepositorio.Modificar(entidad);
            _unidadDeTrabajo.Commit();
        }

        public DtoBase Obtener(long id)
        {
            var entidad = _unidadDeTrabajo.GastoRepositorio.Obtener(id);

            return new GastoDto
            {
                Id = entidad.Id,
                Descripcion = entidad.Descripcion,
                ConceptoGastoId = entidad.ConceptoGastoId,
                Fecha = entidad.Fecha,
                Monto = entidad.Monto,
                Eliminado = entidad.EstaEliminado
            };
        }

        public IEnumerable<DtoBase> Obtener(string cadenaBuscar, bool mostrarTodos = true)
        {
            Expression<Func<Dominio.Entidades.Gasto, bool>> filtro =
                x => x.Descripcion.Contains(cadenaBuscar);

            if (!mostrarTodos)
                filtro = filtro.And(x => !x.EstaEliminado);

            return _unidadDeTrabajo.GastoRepositorio.Obtener(filtro)
                .Select(x => new GastoDto
                {
                    Id = x.Id,
                    Descripcion = x.Descripcion,
                    ConceptoGastoId = x.ConceptoGastoId,
                    Fecha = x.Fecha,
                    Monto = x.Monto,
                    Eliminado = x.EstaEliminado
                })
                .OrderBy(x => x.Descripcion)
                .ToList();
        }

        public bool VerificarSiExiste(string datoVerificar, long? entidadId = null)
        {
            return entidadId.HasValue
                ? _unidadDeTrabajo.GastoRepositorio.Obtener(x => !x.EstaEliminado
                                                                        && x.Id != entidadId.Value
                                                                        && x.Descripcion.Equals(datoVerificar,
                                                                            StringComparison.CurrentCultureIgnoreCase))
                    .Any()
                : _unidadDeTrabajo.GastoRepositorio.Obtener(x => !x.EstaEliminado
                                                                        && x.Descripcion.Equals(datoVerificar,
                                                                            StringComparison.CurrentCultureIgnoreCase))
                    .Any();
        }
    }
}

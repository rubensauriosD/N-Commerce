using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Dominio.UnidadDeTrabajo;
using IServicio.BaseDto;
using IServicio.Caja;
using IServicio.Caja.DTOs;
using Servicios.Base;

namespace Servicios.ConceptoGasto
{
    public class ConceptoGastoServicio : IConceptoGastoServicio
    {
        private readonly IUnidadDeTrabajo _unidadDeTrabajo;

        public ConceptoGastoServicio(IUnidadDeTrabajo unidadDeTrabajo)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
        }

        public void Eliminar(long id)
        {
            _unidadDeTrabajo.ConceptoGastoRepositorio.Eliminar(id);
            _unidadDeTrabajo.Commit();
        }

        public void Insertar(DtoBase dtoEntidad)
        {
            var dto = (ConceptoGastoDto)dtoEntidad;

            var entidad = new Dominio.Entidades.ConceptoGasto
            {
                Descripcion = dto.Descripcion,
                EstaEliminado = false
            };

            _unidadDeTrabajo.ConceptoGastoRepositorio.Insertar(entidad);
            _unidadDeTrabajo.Commit();
        }

        public void Modificar(DtoBase dtoEntidad)
        {
            var dto = (ConceptoGastoDto)dtoEntidad;

            var entidad = _unidadDeTrabajo.ConceptoGastoRepositorio.Obtener(dto.Id);

            if (entidad == null) throw new Exception("Ocurrio un Error al Obtener la ConceptoGasto");

            entidad.Descripcion = dto.Descripcion;

            _unidadDeTrabajo.ConceptoGastoRepositorio.Modificar(entidad);
            _unidadDeTrabajo.Commit();
        }

        public DtoBase Obtener(long id)
        {
            var entidad = _unidadDeTrabajo.ConceptoGastoRepositorio.Obtener(id);

            return new ConceptoGastoDto
            {
                Id = entidad.Id,
                Descripcion = entidad.Descripcion,
                Eliminado = entidad.EstaEliminado
            };
        }

        public IEnumerable<DtoBase> Obtener(string cadenaBuscar, bool mostrarTodos = true)
        {
            Expression<Func<Dominio.Entidades.ConceptoGasto, bool>> filtro =
                x => x.Descripcion.Contains(cadenaBuscar);

            if (!mostrarTodos)
                filtro = filtro.And(x => !x.EstaEliminado);

            return _unidadDeTrabajo.ConceptoGastoRepositorio.Obtener(filtro)
                .Select(x => new ConceptoGastoDto
                {
                    Id = x.Id,
                    Descripcion = x.Descripcion,
                    Eliminado = x.EstaEliminado
                })
                .OrderBy(x => x.Descripcion)
                .ToList();
        }

        public bool VerificarSiExiste(string datoVerificar, long? entidadId = null)
        {
            return entidadId.HasValue
                ? _unidadDeTrabajo.ConceptoGastoRepositorio.Obtener(x => !x.EstaEliminado
                                                                        && x.Id != entidadId.Value
                                                                        && x.Descripcion.Equals(datoVerificar,
                                                                            StringComparison.CurrentCultureIgnoreCase))
                    .Any()
                : _unidadDeTrabajo.ConceptoGastoRepositorio.Obtener(x => !x.EstaEliminado
                                                                        && x.Descripcion.Equals(datoVerificar,
                                                                            StringComparison.CurrentCultureIgnoreCase))
                    .Any();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Dominio.UnidadDeTrabajo;
using IServicio.BaseDto;
using IServicio.PuestoTrabajo;
using IServicio.PuestoTrabajo.DTOs;
using Servicios.Base;

namespace Servicios.PuestoTrabajo
{
    public class PuestoTrabajoServicio : IPuestoTrabajoServicio
    {
        private readonly IUnidadDeTrabajo _unidadDeTrabajo;

        public PuestoTrabajoServicio(IUnidadDeTrabajo unidadDeTrabajo)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
        }

        public void Eliminar(long id)
        {
            _unidadDeTrabajo.PuestoTrabajoRepositorio.Eliminar(id);
            _unidadDeTrabajo.Commit();
        }

        public void Insertar(DtoBase dtoEntidad)
        {
            var dto = (PuestoTrabajoDto)dtoEntidad;

            var entidad = new Dominio.Entidades.PuestoTrabajo
            {
                Descripcion = dto.Descripcion,
                Codigo = dto.Codigo,
                EstaEliminado = false
            };

            _unidadDeTrabajo.PuestoTrabajoRepositorio.Insertar(entidad);
            _unidadDeTrabajo.Commit();
        }

        public void Modificar(DtoBase dtoEntidad)
        {
            var dto = (PuestoTrabajoDto)dtoEntidad;

            var entidad = _unidadDeTrabajo.PuestoTrabajoRepositorio.Obtener(dto.Id);

            if (entidad == null) throw new Exception("Ocurrio un Error al Obtener la PuestoTrabajo");

            entidad.Descripcion = dto.Descripcion;
            entidad.Codigo = dto.Codigo;

            _unidadDeTrabajo.PuestoTrabajoRepositorio.Modificar(entidad);
            _unidadDeTrabajo.Commit();
        }

        public DtoBase Obtener(long id)
        {
            var entidad = _unidadDeTrabajo.PuestoTrabajoRepositorio.Obtener(id);

            return new PuestoTrabajoDto
            {
                Id = entidad.Id,
                Descripcion = entidad.Descripcion,
                Codigo = entidad.Codigo,
                Eliminado = entidad.EstaEliminado
            };
        }

        public IEnumerable<DtoBase> Obtener(string cadenaBuscar, bool mostrarTodos = true)
        {
            Expression<Func<Dominio.Entidades.PuestoTrabajo, bool>> filtro =
                x => x.Descripcion.Contains(cadenaBuscar) || x.Codigo.ToString().Contains(cadenaBuscar);

            if (!mostrarTodos)
                filtro = filtro.And(x => !x.EstaEliminado);

            return _unidadDeTrabajo.PuestoTrabajoRepositorio.Obtener(filtro)
                .Select(x => new PuestoTrabajoDto
                {
                    Id = x.Id,
                    Descripcion = x.Descripcion,
                    Codigo = x.Codigo,
                    Eliminado = x.EstaEliminado
                })
                .OrderBy(x => x.Descripcion)
                .ToList();
        }

        public bool VerificarSiExiste(string datoVerificar, long? entidadId = null)
        {
            bool ok = false;

            //
            // Chequea si existe una descripcion igual
            //
            ok |= entidadId.HasValue
                ? _unidadDeTrabajo.PuestoTrabajoRepositorio.Obtener(x => !x.EstaEliminado
                                                                        && x.Id != entidadId.Value
                                                                        && x.Descripcion.Equals(datoVerificar,
                                                                            StringComparison.CurrentCultureIgnoreCase))
                    .Any()
                : _unidadDeTrabajo.PuestoTrabajoRepositorio.Obtener(x => !x.EstaEliminado
                                                                        && x.Descripcion.Equals(datoVerificar,
                                                                            StringComparison.CurrentCultureIgnoreCase))
                    .Any();

            //
            // Chequea si existe un codigo igual
            //
            if (int.TryParse(datoVerificar, out int codigo))
            { 
                ok |= entidadId.HasValue
                    ? _unidadDeTrabajo.PuestoTrabajoRepositorio.Obtener(x => !x.EstaEliminado
                                                                            && x.Id != entidadId.Value
                                                                            && x.Codigo == codigo)
                        .Any()
                    : _unidadDeTrabajo.PuestoTrabajoRepositorio.Obtener(x => !x.EstaEliminado
                                                                            && x.Codigo == codigo)
                        .Any();
            }

            return ok;
        }

        public int ProximoCodigo() {

            var puestos = _unidadDeTrabajo.PuestoTrabajoRepositorio.Obtener();

            return puestos.Any() ? puestos.Max(x => x.Codigo) + 1 : 1;
        }
    }
}

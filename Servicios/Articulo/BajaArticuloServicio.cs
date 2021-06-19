namespace Servicios.Articulo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Aplicacion.Constantes;
    using Dominio.UnidadDeTrabajo;
    using IServicio.BaseDto;
    using IServicios.Articulo;
    using IServicios.Articulo.DTOs;
    using Servicios.Base;

    public class BajaArticuloServicio : IBajaArticuloServicio
    {
        private readonly IUnidadDeTrabajo _unidadDeTrabajo;

        public BajaArticuloServicio(IUnidadDeTrabajo unidadDeTrabajo)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
        }

        // --- Persistencia
        public void Insertar(DtoBase dtoEntidad)
        {
            try
            {
                var dto = (BajaArticuloDto)dtoEntidad;

                var entidad = new Dominio.Entidades.BajaArticulo
                {
                    ArticuloId = dto.ArticuloId,
                    MotivoBajaId = dto.MotivoBajaId,
                    Cantidad = dto.Cantidad,
                    Fecha = DateTime.Now,
                    Observacion = dto.Observacion,
                    EstaEliminado = false
                };

                decimal cantidadParaBaja = dto.Cantidad;
                var stocks = _unidadDeTrabajo.StockRepositorio.Obtener()
                    .Where(s => s.ArticuloId == dto.ArticuloId && s.Cantidad > 0)
                    .ToList();

                if (stocks.Sum(s => s.Cantidad) < dto.Cantidad)
                    throw new Exception("No hay stock suficiente para realizar la baja.");

                foreach (var deposito in stocks)
                {
                    if (cantidadParaBaja <= 0)
                        break;

                    if (deposito.Cantidad < cantidadParaBaja)
                    {
                        cantidadParaBaja -= deposito.Cantidad;
                        deposito.Cantidad = 0;
                    }

                    if (deposito.Cantidad >= cantidadParaBaja)
                    { 
                        deposito.Cantidad -= cantidadParaBaja;
                        cantidadParaBaja = 0;
                    }

                    _unidadDeTrabajo.StockRepositorio.Modificar(deposito);
                }

                _unidadDeTrabajo.BajaArticuloRepositorio.Insertar(entidad);
                _unidadDeTrabajo.Commit();
            }
            catch (Exception e)
            {
                throw new Exception($"Error en BajaArticuloServicio.Insertar:{Environment.NewLine}{e.Message}");
            }
        }

        public bool RevertirBaja(long id)
        {
            try
            {
                var entidad = _unidadDeTrabajo.BajaArticuloRepositorio.Obtener(id);

                if (entidad == null)
                    return false;

                decimal cantidadParaBaja = entidad.Cantidad;
                var deposito = _unidadDeTrabajo.StockRepositorio.Obtener()
                    .FirstOrDefault(s => s.ArticuloId == entidad.ArticuloId);

                if (deposito == null)
                {
                    Mjs.Error($"Error al encontrar un depósito para el artículo");
                    return false;
                }

                deposito.Cantidad += entidad.Cantidad;

                var reversion = new Dominio.Entidades.BajaArticulo() 
                {
                    ArticuloId = entidad.ArticuloId,
                    MotivoBajaId = entidad.MotivoBajaId,
                    Cantidad = entidad.Cantidad * -1,
                    Fecha = DateTime.Now,
                    Observacion = "Reversion de baja.",
                    EstaEliminado = false
                };

                _unidadDeTrabajo.StockRepositorio.Modificar(deposito);
                _unidadDeTrabajo.BajaArticuloRepositorio.Insertar(reversion);
                _unidadDeTrabajo.Commit();

                return true;
            }
            catch (Exception e)
            {
                throw new Exception($"Error en BajaArticuloServicio.Insertar:{Environment.NewLine}{e.Message}");
            }
        }


        public void Eliminar(long id)
        {

        }

        public void Modificar(DtoBase dtoEntidad)
        {
        }

        // --- Consulta
        public DtoBase Obtener(long id)
        {
            var entidad = _unidadDeTrabajo.BajaArticuloRepositorio.Obtener(id, "Articulo, Articulo.Stocks, MotivoBaja");

            return new BajaArticuloDto
            {
                Id = entidad.Id,
                ArticuloId = entidad.ArticuloId,
                Articulo = entidad.Articulo.Descripcion,
                Foto = entidad.Articulo.Foto,
                Stock = entidad.Articulo.Stocks.Sum(x => x.Cantidad),
                MotivoBajaId = entidad.MotivoBajaId,
                MotivoBaja = entidad.MotivoBaja.Descripcion,
                Cantidad = entidad.Cantidad,
                Fecha = entidad.Fecha,
                Observacion = entidad.Observacion,
                Eliminado = entidad.EstaEliminado,
            };
        }

        public IEnumerable<DtoBase> Obtener(string cadenaBuscar, bool mostrarTodos = true)
        {
            Expression<Func<Dominio.Entidades.BajaArticulo, bool>> filtro =
                x => x.Observacion.Contains(cadenaBuscar);

            if (!mostrarTodos)
                filtro = filtro.And(x => !x.EstaEliminado);

            return _unidadDeTrabajo.BajaArticuloRepositorio.Obtener(filtro, "Articulo, Articulo.Stocks, MotivoBaja")
                .Select(x => new BajaArticuloDto
                {
                    Id = x.Id,
                    ArticuloId = x.ArticuloId,
                    Articulo = x.Articulo.Descripcion,
                    Foto = x.Articulo.Foto,
                    Stock = x.Articulo.Stocks.Sum(s => s.Cantidad),
                    MotivoBajaId = x.MotivoBajaId,
                    MotivoBaja = x.MotivoBaja.Descripcion,
                    Cantidad = x.Cantidad,
                    Fecha = x.Fecha,
                    Observacion = x.Observacion,
                    Eliminado = x.EstaEliminado,
                })
                .OrderByDescending(x => x.Fecha)
                .ToList();
        }
    }
}

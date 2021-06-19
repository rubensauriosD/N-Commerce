namespace Servicios.Deposito
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Transactions;
    using Aplicacion.Constantes;
    using Dominio.UnidadDeTrabajo;
    using IServicio.BaseDto;
    using IServicio.Deposito;
    using IServicio.Deposito.DTOs;
    using IServicios.Deposito.DTOs;
    using Servicios.Base;

    public class DepositoServicio : IDepositoSevicio
    {
        private readonly IUnidadDeTrabajo _unidadDeTrabajo;

        public DepositoServicio(IUnidadDeTrabajo unidadDeTrabajo)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
        }

        // --- Persistencia
        public void Eliminar(long id)
        {
            if (id == 1)
                return;

            try
            {
                var deposito = _unidadDeTrabajo.DepositoRepositorio.Obtener(id, "Stocks");

                if (deposito == null)
                {
                    Mjs.Alerta("Depósito no encontrado.");
                    return;
                }

                if (deposito.Stocks.Any(s => s.Cantidad > 0))
                {
                    Mjs.Alerta("No se puede eliminar depositos con Stoks.");
                    return;
                }

                _unidadDeTrabajo.DepositoRepositorio.Eliminar(id);
                _unidadDeTrabajo.Commit();

            }
            catch (Exception e)
            {
                throw new Exception($"DepositoServicio.Eliminar: {e.Message}");
            }
        }

        public void Insertar(DtoBase dtoEntidad)
        {
            using (var tran = new TransactionScope())
            {
                try
                {
                    var dto = (DepositoDto)dtoEntidad;

                    var entidad = new Dominio.Entidades.Deposito
                    {
                        Descripcion = dto.Descripcion,
                        Ubicacion = dto.Ubicacion,
                        EstaEliminado = false
                    };

                    _unidadDeTrabajo.DepositoRepositorio.Insertar(entidad);

                    var articulos = _unidadDeTrabajo.ArticuloRepositorio.Obtener();

                    foreach (var art in articulos)
                        _unidadDeTrabajo.StockRepositorio.Insertar(new Dominio.Entidades.Stock
                        {
                            ArticuloId = art.Id,
                            Cantidad = 0,
                            DepositoId = entidad.Id,
                            EstaEliminado = false
                        });

                    _unidadDeTrabajo.Commit();

                    tran.Complete();
                }
                catch (Exception e)
                {
                    tran.Dispose();
                    throw new Exception(e.Message);
                }
            }

        }

        public void Modificar(DtoBase dtoEntidad)
        {
            var dto = (DepositoDto)dtoEntidad;

            var entidad = _unidadDeTrabajo.DepositoRepositorio.Obtener(dto.Id);

            if (entidad == null) throw new Exception("Ocurrio un Error al Obtener la Deposito");

            entidad.Descripcion = dto.Descripcion;
            entidad.Ubicacion = dto.Ubicacion;

            _unidadDeTrabajo.DepositoRepositorio.Modificar(entidad);
            _unidadDeTrabajo.Commit();
        }

        public bool TransferirArticulos(TransferenciaDepositoDto transferencia)
        {
            try
            {
                var stocks = _unidadDeTrabajo.StockRepositorio.Obtener().ToList();

                var origen = stocks.First(x => x.DepositoId == transferencia.OrigenId && x.ArticuloId == transferencia.ArticuloId);
                var destino = stocks.First(x => x.DepositoId == transferencia.DestinoId && x.ArticuloId == transferencia.ArticuloId);

                if (origen.Cantidad < transferencia.Cantidad)
                {
                    Mjs.Alerta("Stock insuficiente en el deposito de origen para realizar la transferencia.");
                    return false;
                }

                origen.Cantidad -= transferencia.Cantidad;
                destino.Cantidad += transferencia.Cantidad;

                _unidadDeTrabajo.StockRepositorio.Modificar(origen);
                _unidadDeTrabajo.StockRepositorio.Modificar(destino);
                _unidadDeTrabajo.Commit();

                return true;
            }
            catch (Exception e)
            {
                throw new Exception($"Error en DepositoServicio.TransferirArticulos:{Environment.NewLine}{e.Message}");
            }
        }

        // --- Consulta
        public DtoBase Obtener(long id)
        {
            var entidad = _unidadDeTrabajo.DepositoRepositorio.Obtener(id, "Stocks, Stocks.Articulo");

            return new DepositoDto
            {
                Id = entidad.Id,
                Descripcion = entidad.Descripcion,
                Ubicacion = entidad.Ubicacion,
                Stocks = entidad.Stocks.Select(s => new StockDto()
                {
                    ArticuloId = s.ArticuloId,
                    Articulo = s.Articulo.Descripcion,
                    Cantidad = s.Cantidad
                }).ToList(),
                Eliminado = entidad.EstaEliminado
            };
        }

        public IEnumerable<DtoBase> Obtener(string cadenaBuscar, bool mostrarTodos = true)
        {
            Expression<Func<Dominio.Entidades.Deposito, bool>> filtro =
                x => x.Descripcion.Contains(cadenaBuscar);

            if (!mostrarTodos)
                filtro = filtro.And(x => !x.EstaEliminado);


            return _unidadDeTrabajo.DepositoRepositorio.Obtener(filtro,"Stocks, Stocks.Articulo")
                .Select(x => new DepositoDto
                {
                    Id = x.Id,
                    Descripcion = x.Descripcion,
                    Ubicacion = x.Ubicacion,
                    Stocks = x.Stocks.Select(s => new StockDto() { 
                        ArticuloId = s.ArticuloId,
                        Articulo = s.Articulo.Descripcion,
                        Cantidad = s.Cantidad
                    }).ToList(),
                    Eliminado = x.EstaEliminado
                })
                .OrderBy(x => x.Descripcion)
                .ToList();
        }

        public bool VerificarSiExiste(string datoVerificar, long? entidadId = null)
        {
            return entidadId.HasValue
                ? _unidadDeTrabajo.DepositoRepositorio.Obtener(x => !x.EstaEliminado
                                                                        && x.Id != entidadId.Value
                                                                        && x.Descripcion.Equals(datoVerificar,
                                                                            StringComparison.CurrentCultureIgnoreCase))
                    .Any()
                : _unidadDeTrabajo.DepositoRepositorio.Obtener(x => !x.EstaEliminado
                                                                        && x.Descripcion.Equals(datoVerificar,
                                                                            StringComparison.CurrentCultureIgnoreCase))
                    .Any();
        }

        public bool TieneStokDeArticulos(long? id)
        {
            try
            {
                var deposito = _unidadDeTrabajo.DepositoRepositorio.Obtener((long)id, "Stocks");

                if (deposito == null)
                {
                    Mjs.Alerta("Depósito no encontrado.");
                    return false;
                }

                return deposito.Stocks.Any(s => s.Cantidad > 0);

            }
            catch (Exception e)
            {
                throw new Exception($"DepositoServicio.TieneStokDeArticulos: {e.Message}");
            }
        }
    }
}

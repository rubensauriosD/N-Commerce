namespace Servicios.Articulo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Transactions;
    using Aplicacion.Constantes;
    using Dominio.Entidades;
    using Dominio.UnidadDeTrabajo;
    using IServicio.Articulo;
    using IServicio.Articulo.DTOs;
    using IServicio.BaseDto;
    using IServicios.Articulo.DTOs;
    using Servicios.Base;

    public class ArticuloServicio : IArticuloServicio
    {
        private readonly IUnidadDeTrabajo _unidadDeTrabajo;

        public ArticuloServicio(IUnidadDeTrabajo unidadDeTrabajo)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
        }

        // PERSISTENCIA
        public void Eliminar(long id)
        {
            _unidadDeTrabajo.ArticuloRepositorio.Eliminar(id);
            _unidadDeTrabajo.Commit();
        }

        public void Insertar(DtoBase dtoEntidad)
        {
            using (var tran = new TransactionScope())
            {
                try
                {
                    var dto = (ArticuloCrudDto)dtoEntidad;
                    var entidad = new Articulo
                    {
                        MarcaId = dto.MarcaId,
                        RubroId = dto.RubroId,
                        UnidadMedidaId = dto.UnidadMedidaId,
                        IvaId = dto.IvaId,
                        Codigo = dto.Codigo,
                        CodigoBarra = dto.CodigoBarra,
                        Abreviatura = dto.Abreviatura,
                        Descripcion = dto.Descripcion,
                        Detalle = dto.Detalle,
                        Ubicacion = dto.Ubicacion,
                        ActivarLimiteVenta = dto.ActivarLimiteVenta,
                        LimiteVenta = dto.LimiteVenta,
                        ActivarHoraVenta = dto.ActivarHoraVenta,
                        HoraLimiteVentaDesde = dto.HoraLimiteVentaDesde,
                        HoraLimiteVentaHasta = dto.HoraLimiteVentaHasta,
                        PermiteStockNegativo = dto.PermiteStockNegativo,
                        DescuentaStock = dto.DescuentaStock,
                        StockMinimo = dto.StockMinimo,
                        Foto = dto.Foto,
                        EstaEliminado = false
                    };

                    _unidadDeTrabajo.ArticuloRepositorio.Insertar(entidad);

                    var precioArticulo = new Precio
                    {
                        FechaActualizacion = DateTime.Now,
                        ArticuloId = entidad.Id,
                        PrecioCosto = dto.PrecioCosto,
                        EstaEliminado = false
                    };
                    _unidadDeTrabajo.PrecioRepositorio.Insertar(precioArticulo);

                    var configSistema = _unidadDeTrabajo.ConfiguracionRepositorio.Obtener().FirstOrDefault();

                    if (configSistema == null)
                        throw new Exception("Ocurrio un error al Obtener la configuracion del sistema");

                    var depositos = _unidadDeTrabajo.DepositoRepositorio.Obtener();

                    foreach (var depo in depositos)
                        _unidadDeTrabajo.StockRepositorio.Insertar(new Stock
                        {
                            DepositoId = depo.Id,
                            ArticuloId = entidad.Id,
                            Cantidad = depo.Id == configSistema.DepositoNuevoArticuloId ? dto.StockActual : 0,
                            EstaEliminado = false
                        });

                    _unidadDeTrabajo.Commit();

                    tran.Complete();
                }
                catch (Exception ex)
                {
                    tran.Dispose();
                    throw new Exception(ex.Message);
                }
            }
        }

        public void Modificar(DtoBase dtoEntidad)
        {
            var dto = (ArticuloCrudDto)dtoEntidad;
            var entidad = _unidadDeTrabajo.ArticuloRepositorio.Obtener(dto.Id);
            if (entidad == null) throw new Exception("Ocurrio un Error al Obtener el Artículo");
            entidad.MarcaId = dto.MarcaId;
            entidad.RubroId = dto.RubroId;
            entidad.UnidadMedidaId = dto.UnidadMedidaId;
            entidad.IvaId = dto.IvaId;
            entidad.Codigo = dto.Codigo;
            entidad.CodigoBarra = dto.CodigoBarra;
            entidad.Abreviatura = dto.Abreviatura;
            entidad.Descripcion = dto.Descripcion;
            entidad.Detalle = dto.Detalle;
            entidad.Ubicacion = dto.Ubicacion;
            entidad.ActivarLimiteVenta = dto.ActivarLimiteVenta;
            entidad.LimiteVenta = dto.LimiteVenta;
            entidad.ActivarHoraVenta = dto.ActivarHoraVenta;
            entidad.HoraLimiteVentaDesde = dto.HoraLimiteVentaDesde;
            entidad.HoraLimiteVentaHasta = dto.HoraLimiteVentaHasta;
            entidad.PermiteStockNegativo = dto.PermiteStockNegativo;
            entidad.DescuentaStock = dto.DescuentaStock;
            entidad.StockMinimo = dto.StockMinimo;
            entidad.Foto = dto.Foto;
            _unidadDeTrabajo.ArticuloRepositorio.Modificar(entidad);
            _unidadDeTrabajo.Commit();
        }

        public bool ModificarPrecioPorPorcentaje(List<ArticuloDto> articulos, decimal porcentaje)
        {
            try
            {
                foreach (var art in articulos)
                {
                    var preciosDeArticulo = _unidadDeTrabajo.PrecioRepositorio.Obtener()
                        .Where(pre => pre.ArticuloId == art.Id)
                        .ToList();

                    decimal ultimoPrecioArticulo = preciosDeArticulo
                        .FirstOrDefault(pre => pre.FechaActualizacion == preciosDeArticulo.Max(p => p.FechaActualizacion))
                        .PrecioCosto;

                    _unidadDeTrabajo.PrecioRepositorio.Insertar(new Precio()
                    {
                        ArticuloId = art.Id,
                        PrecioCosto = ultimoPrecioArticulo * (porcentaje / 100),
                        FechaActualizacion = DateTime.Now,
                        EstaEliminado = false
                    });
                }

                _unidadDeTrabajo.Commit();
                return true;
            }
            catch (Exception e)
            {
                Mjs.Error($@"Error al modificar el precio de {articulos.Count} artículos.");
                return false;
            }
        }

        public bool ModificarPrecioPorPrecio(List<ArticuloDto> articulos, decimal monto)
        {
            try
            {
                foreach (var art in articulos)
                {
                    var preciosDeArticulo = _unidadDeTrabajo.PrecioRepositorio.Obtener()
                        .Where(pre => pre.ArticuloId == art.Id)
                        .ToList();

                    decimal ultimoPrecioArticulo = preciosDeArticulo
                        .FirstOrDefault(pre => pre.FechaActualizacion == preciosDeArticulo.Max(p => p.FechaActualizacion))
                        .PrecioCosto;

                    _unidadDeTrabajo.PrecioRepositorio.Insertar(new Precio()
                    {
                        ArticuloId = art.Id,
                        PrecioCosto = ultimoPrecioArticulo + monto,
                        FechaActualizacion = DateTime.Now,
                        EstaEliminado = false
                    });
                }

                _unidadDeTrabajo.Commit();
                return true;
            }
            catch (Exception e)
            {
                Mjs.Error($@"Error al modificar el precio de {articulos.Count} artículos.");
                return false;
            }
        }

        // CONSULTA
        public DtoBase Obtener(long id)
        {
            var entidad = _unidadDeTrabajo.ArticuloRepositorio.Obtener(id, "Rubro, Marca, UnidadMedida, Iva, Stocks, Stocks.Deposito, Precios");
            return new ArticuloDto
            {
                Id = entidad.Id,
                MarcaId = entidad.MarcaId,
                Marca = entidad.Marca.Descripcion,
                RubroId = entidad.RubroId,
                Rubro = entidad.Rubro.Descripcion,
                UnidadMedidaId = entidad.UnidadMedidaId,
                UnidadMedida = entidad.UnidadMedida.Descripcion,
                IvaId = entidad.IvaId,
                Iva = entidad.Iva.Descripcion,
                Codigo = entidad.Codigo,
                CodigoBarra = entidad.CodigoBarra,
                Abreviatura = entidad.Abreviatura,
                Descripcion = entidad.Descripcion,
                Detalle = entidad.Detalle,
                Ubicacion = entidad.Ubicacion,
                ActivarLimiteVenta = entidad.ActivarLimiteVenta,
                LimiteVenta = entidad.LimiteVenta,
                ActivarHoraVenta = entidad.ActivarHoraVenta,
                HoraLimiteVentaDesde = entidad.HoraLimiteVentaDesde,
                HoraLimiteVentaHasta = entidad.HoraLimiteVentaHasta,
                PermiteStockNegativo = entidad.PermiteStockNegativo,
                DescuentaStock = entidad.DescuentaStock,
                StockMinimo = entidad.StockMinimo,
                Foto = entidad.Foto,
                Eliminado = entidad.EstaEliminado,
                Stocks = entidad.Stocks
            .Select(x => new StockDepositoDto
            {
                Cantidad = x.Cantidad,
                Desposito = x.Deposito.Descripcion
            }).ToList(),
                Precios = ObtenerListadoPrecioVentasDesdeArticulos(entidad)
            };
        }

        public IEnumerable<DtoBase> Obtener(string cadenaBuscar, bool mostrarTodos = true)
        {
            Expression<Func<Dominio.Entidades.Articulo, bool>> filtro = x =>
            x.Descripcion.Contains(cadenaBuscar)
            || x.Marca.Descripcion.Contains(cadenaBuscar)
            || x.Rubro.Descripcion.Contains(cadenaBuscar)
            || x.UnidadMedida.Descripcion.Contains(cadenaBuscar)
            || x.Iva.Descripcion.Contains(cadenaBuscar);

            if (!mostrarTodos)
                filtro = filtro.And(x => !x.EstaEliminado);

            return _unidadDeTrabajo.ArticuloRepositorio.Obtener(filtro, "Rubro, Marca, UnidadMedida, Iva, Stocks, Stocks.Deposito, Precios")
                .Select(x => new ArticuloDto
                {
                    Id = x.Id,
                    MarcaId = x.MarcaId,
                    Marca = x.Marca.Descripcion,
                    RubroId = x.RubroId,
                    Rubro = x.Rubro.Descripcion,
                    UnidadMedidaId = x.UnidadMedidaId,
                    UnidadMedida = x.UnidadMedida.Descripcion,
                    IvaId = x.IvaId,
                    Iva = x.Iva.Descripcion,
                    Codigo = x.Codigo,
                    CodigoBarra = x.CodigoBarra,
                    Abreviatura = x.Abreviatura,
                    Descripcion = x.Descripcion,
                    Detalle = x.Detalle,
                    Ubicacion = x.Ubicacion,
                    ActivarLimiteVenta = x.ActivarLimiteVenta,
                    LimiteVenta = x.LimiteVenta,
                    ActivarHoraVenta = x.ActivarHoraVenta,
                    HoraLimiteVentaDesde = x.HoraLimiteVentaDesde,
                    HoraLimiteVentaHasta = x.HoraLimiteVentaHasta,
                    PermiteStockNegativo = x.PermiteStockNegativo,
                    DescuentaStock = x.DescuentaStock,
                    StockMinimo = x.StockMinimo,
                    Foto = x.Foto,
                    Eliminado = x.EstaEliminado,
                    Stocks = x.Stocks
                        .Select(s => new StockDepositoDto
                        {
                            Cantidad = s.Cantidad,
                            Desposito = s.Deposito.Descripcion
                        }).ToList(),
                    Precios = ObtenerListadoPrecioVentasDesdeArticulos(x)
                })
                .OrderBy(x => x.Descripcion)
                .ToList();
        }

        public ArticuloVentaDto ObtenerPorCodigo(int codigo, long listaPrecioId, long depositoId)
        {
            return _unidadDeTrabajo.ArticuloRepositorio
                .Obtener(x => (x.CodigoBarra == codigo.ToString() || x.Codigo == codigo) && !x.EstaEliminado,
                    "Rubro, Marca, UnidadMedida, Iva, Stocks, Stocks.Deposito, Precios")
                .Select(x => new ArticuloVentaDto()
                {
                    Id = x.Id,
                    Iva = x.Iva.Porcentaje,
                    Codigo = x.Codigo.ToString(),
                    CodigoBarra = x.CodigoBarra,
                    Descripcion = x.Descripcion,
                    HoraDesde = x.HoraLimiteVentaDesde,
                    HoraHasta = x.HoraLimiteVentaHasta,
                    TieneRestriccionHorario = x.ActivarHoraVenta,
                    TieneRestriccionPorCantidad = x.ActivarLimiteVenta,
                    LimiteVenta = x.LimiteVenta,
                    PermiteStockNegativo = x.PermiteStockNegativo,
                    Stock = x.Stocks.Any()
                        ? x.Stocks.Where(d => d.DepositoId == depositoId).Sum(s => s.Cantidad)
                        : 0m,
                    Precio = ObtenerPrecioPublicoDesdeArticulo(x, listaPrecioId),
                    ListaPrecioId = listaPrecioId
                }).FirstOrDefault();
        }

        public ArticuloCompraDto ObtenerPorCodigo(string codigo)
        {
            int.TryParse(codigo, out int _codigo);

            return _unidadDeTrabajo.ArticuloRepositorio
                .Obtener(x => (x.CodigoBarra == codigo || x.Codigo == _codigo) && !x.EstaEliminado, "Precios")
                .Select(x => new ArticuloCompraDto()
                {
                    ProductoId = x.Id,
                    Codigo = x.Codigo.ToString(),
                    CodigoBarra = x.CodigoBarra,
                    Descripcion = x.Descripcion,
                    Precio = x.Precios.First(p => p.FechaActualizacion == x.Precios.Max(pc => pc.FechaActualizacion)).PrecioCosto
                }).FirstOrDefault();
        }

        public IEnumerable<ArticuloVentaDto> ObtenerLookUp(string cadenaBuscar, long listaPrecioId)
        {
            var fechaActual = DateTime.Now;

            int.TryParse(cadenaBuscar, out int codigoArticulo);

            Expression<Func<Articulo, bool>> filtro = x => !x.EstaEliminado
                                                        && x.CodigoBarra == cadenaBuscar
                                                        || x.Descripcion.Contains(cadenaBuscar)
                                                        || x.Codigo == codigoArticulo;

            return _unidadDeTrabajo.ArticuloRepositorio.Obtener(filtro,
                    "Iva, Stocks, Stocks.Deposito, Precios")
                .Select(x => new ArticuloVentaDto()
                {
                    Id = x.Id,
                    Iva = x.Iva.Porcentaje,
                    CodigoBarra = x.CodigoBarra,
                    Descripcion = x.Descripcion,
                    HoraDesde = x.HoraLimiteVentaDesde,
                    HoraHasta = x.HoraLimiteVentaHasta,
                    TieneRestriccionHorario = x.ActivarHoraVenta,
                    TieneRestriccionPorCantidad = x.ActivarLimiteVenta,
                    LimiteVenta = x.LimiteVenta,
                    Stock = x.Stocks.Any()
                        ? x.Stocks.Sum(s => s.Cantidad)
                        : 0m,
                    Precio = ObtenerPrecioPublicoDesdeArticulo(x, listaPrecioId),
                    ListaPrecioId = listaPrecioId
                }).ToList();
        }

        public int ObtenerCantidadArticulos()
        {
            return _unidadDeTrabajo.ArticuloRepositorio.Obtener().Count();
        }

        public int ObtenerSigueinteNroCodigo()
        {
            var articulos = _unidadDeTrabajo.ArticuloRepositorio.Obtener();
            return articulos.Any()
            ? articulos.Max(x => x.Codigo) + 1
            : 1;
        }
 
        public bool VerificarSiExiste(string datoVerificar, long? entidadId = null)
        {
            if (entidadId.HasValue)
                return _unidadDeTrabajo.ArticuloRepositorio.Obtener(x => !x.EstaEliminado
                && x.Id != entidadId.Value
                && x.Descripcion.Equals(datoVerificar,
                StringComparison.CurrentCultureIgnoreCase))
                .Any();

            else
                return _unidadDeTrabajo.ArticuloRepositorio.Obtener(x => !x.EstaEliminado
                    && x.Descripcion.Equals(datoVerificar,
                    StringComparison.CurrentCultureIgnoreCase))
                    .Any();
        }

        public bool VerificarSiExisteCodigo(int codigo, long? entidadId = null)
        {
            if (entidadId.HasValue)
                return _unidadDeTrabajo.ArticuloRepositorio.Obtener(x => !x.EstaEliminado
                && x.Id != entidadId.Value
                && x.Codigo == codigo)
                .Any();

            else
                return _unidadDeTrabajo.ArticuloRepositorio.Obtener(x => !x.EstaEliminado
                    && x.Codigo == codigo)
                    .Any();
        }

        public bool VerificarSiExisteCodigoBarra(string codigoBarra, long? entidadId = null)
        {
            if (entidadId.HasValue)
                return _unidadDeTrabajo.ArticuloRepositorio.Obtener(x => !x.EstaEliminado
                && x.Id != entidadId.Value
                && x.CodigoBarra == codigoBarra)
                .Any();

            else
                return _unidadDeTrabajo.ArticuloRepositorio.Obtener(x => !x.EstaEliminado
                    && x.CodigoBarra == codigoBarra)
                    .Any();
        }

        private decimal ObtenerPrecioPublicoDesdeArticulo(Articulo art, long listaPrecioId)
        {
            var porcentajeGanancia = _unidadDeTrabajo.ListaPrecioRepositorio.Obtener(listaPrecioId).PorcentajeGanancia;

            var fechaHoy = DateTime.Today.Year *10000 + DateTime.Today.Month *100 +DateTime.Today.Day;

            Precio precio = art.Precios
                        .FirstOrDefault(p => p.FechaActualizacion == art.Precios
                            .Where(pre => pre.FechaActualizacion.Year * 10000 + pre.FechaActualizacion.Month * 100 + pre.FechaActualizacion.Day <= fechaHoy)
                            .Max(f => f.FechaActualizacion));

            //Precio precio = art.Precios
            //            .FirstOrDefault(p => p.FechaActualizacion == art.Precios.Where(pre => pre.FechaActualizacion.Day <= DateTime.Today.Day
            //                                                                        && pre.FechaActualizacion.Month <= DateTime.Today.Month
            //                                                                        && pre.FechaActualizacion.Year <= DateTime.Today.Year)
            //                                                                    .Max(f => f.FechaActualizacion));

            return precio.PrecioCosto * (1 + porcentajeGanancia / 100);
        }

        private List<PrecioDto> ObtenerListadoPrecioVentasDesdeArticulos(Articulo art) 
        {
            var ultimaFechaActualizacion = art.Precios
                .Where(pre => (pre.FechaActualizacion.Year * 1000 + pre.FechaActualizacion.DayOfYear)
                                <= (DateTime.Today.Year * 1000 + DateTime.Today.DayOfYear))
                .ToList()
                .Max(f => f.FechaActualizacion);

            Precio precio = art.Precios
                        .FirstOrDefault(p => p.FechaActualizacion == ultimaFechaActualizacion);

            return _unidadDeTrabajo.ListaPrecioRepositorio.Obtener()
                .Select(lp => new PrecioDto() {
                    Fecha = precio.FechaActualizacion,
                    ListaPrecio = lp.Descripcion,
                    PrecioPublico = precio.PrecioCosto * (1 + lp.PorcentajeGanancia / 100)
                })
                .ToList() ;
        }

    }
}

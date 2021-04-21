namespace Servicios.Caja
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Dominio.UnidadDeTrabajo;
    using Infraestructura.UnidadDeTrabajo;
    using IServicios.Caja;
    using IServicios.Caja.DTOs;
    using Servicios.Base;

    public class CajaServicio : ICajaServicio
    {
        private readonly UnidadDeTrabajo _unidadDeTrabajo;

        public CajaServicio(UnidadDeTrabajo _unidadDeTrabajo)
        {
            this._unidadDeTrabajo = _unidadDeTrabajo;
        }

        // CONSULTA
        public long? ObtenerCajaAciva(long usuarioId)
        {
            var caja = _unidadDeTrabajo.CajaRepositorio
            .Obtener()
            .ToList()
            .LastOrDefault(x => x.UsuarioAperturaId == usuarioId && x.UsuarioCierreId == null);

            return caja?.Id;
        }

        public decimal ObtenerMontoCajaAnterior(long usuarioId)
        {
            var cajasUsuario = _unidadDeTrabajo.CajaRepositorio
            .Obtener(x => x.UsuarioAperturaId == usuarioId &&
            x.UsuarioCierre != null);

            var ultimaCaja = cajasUsuario.Where(x => x.FechaApertura ==
            cajasUsuario.Max(f => f.FechaApertura))
            .LastOrDefault();

            return ultimaCaja == null ? 0m : ultimaCaja.MontoCierre.Value;
        }

        public bool VerificarSiExisteCajaAbierta(long usuarioId)
        {
            return _unidadDeTrabajo.CajaRepositorio.Obtener(x =>
            x.UsuarioAperturaId == usuarioId && x.UsuarioCierreId ==
            null).Any();
        }

        public IEnumerable<CajaDto> Obtener(string cadenaBuscar, bool filtroPorFecha, DateTime fechaDesde, DateTime fechaHasta)
        {
            Expression<Func<Dominio.Entidades.Caja, bool>> filtro = x =>
            !x.EstaEliminado && x.UsuarioApertura.Nombre.Contains
            (cadenaBuscar);

            var _fechaDesde = new DateTime(fechaDesde.Year, fechaDesde.Month,
            fechaDesde.Day, 0, 0, 0);

            var _fechaHasta = new DateTime(fechaHasta.Year, fechaHasta.Month,
            fechaHasta.Day, 23, 59, 59);

            if (filtroPorFecha)
            {
                filtro = filtro.And(x => x.FechaApertura >= _fechaDesde &&
                x.FechaApertura <= _fechaHasta);
            }

            return _unidadDeTrabajo.CajaRepositorio.Obtener(filtro,
            "UsuarioApertura, UsuarioCierre, DetalleCajas")
            .Select(x => Map(x))
            .ToList();
        }

        // PERSISTENCIA

        public void Abrir(long usuarioId, decimal monto)
        {
            try
            {
                var nuevaCaja = new Dominio.Entidades.Caja
                {
                    UsuarioAperturaId = usuarioId,
                    FechaApertura = DateTime.Now,
                    MontoInicial = monto,
                    //----------------------------------//
                    UsuarioCierreId = (long?)null,
                    FechaCierre = (DateTime?)null,
                    MontoCierre = (decimal?)null,
                    //----------------------------------//
                    EstaEliminado = false
                };

                _unidadDeTrabajo.CajaRepositorio.Insertar(nuevaCaja);
                _unidadDeTrabajo.Commit();

            }
            catch (Exception ex)
            {
                throw new Exception($@"Error al abrir la caja:{Environment.NewLine + ex.Message}");
            }
        }

        public void Cerrar(long usuarioId, decimal monto)
        {
            try
            {
                var caja = _unidadDeTrabajo.CajaRepositorio.Obtener()
                    .ToList()
                    .LastOrDefault(x => x.UsuarioAperturaId == usuarioId && x.UsuarioCierreId == null);

                if (caja == null)
                    throw new Exception("No se encontro una caja para cerrar");

                caja.UsuarioCierreId = (long?)null;
                caja.FechaCierre = DateTime.Now;
                caja.MontoCierre = monto;

                _unidadDeTrabajo.CajaRepositorio.Modificar(caja);
                _unidadDeTrabajo.Commit();

            }
            catch (Exception ex)
            {
                throw new Exception($@"Error al abrir la caja:{Environment.NewLine + ex.Message}");
            }

        }

        // PRIVADAS
        private CajaDto Map(Dominio.Entidades.Caja x)
        {
            try
            {
                var detalle = (x.DetalleCajas ?? new List<Dominio.Entidades.DetalleCaja>())
                        .Select(d => new CajaDetalleDto()
                        {
                            Id = d.Id,
                            CajaId = d.CajaId,
                            TipoMovimiento = d.TipoMovimiento,
                            TipoPago = d.TipoPago,
                            Monto = d.Monto,
                            Eliminado = d.EstaEliminado,
                        }).ToList();

                return 
                    new CajaDto()
                    {
                        Id = x.Id,
                        // ----------------------------------------//
                        UsuarioAperturaId = x.UsuarioAperturaId,
                        UsuarioApertura = x.UsuarioApertura.Nombre ?? "",
                        FechaApertura = x.FechaApertura,
                        MontoApertura = x.MontoInicial,
                        // ----------------------------------------//
                        UsuarioCierreId = x.UsuarioCierreId ?? 0,
                        UsuarioCierre = x.UsuarioCierreId.HasValue ? x.UsuarioCierre.Nombre : "----",
                        FechaCierre = x.FechaCierre ?? DateTime.Today,
                        Eliminado = x.EstaEliminado,
                        Detalle = detalle
                    };
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private Dominio.Entidades.Caja Map (CajaDto x)
            => new Dominio.Entidades.Caja()
            {
                Id = x.Id,
                // ----------------------------------------//
                UsuarioAperturaId = x.UsuarioAperturaId,
                FechaApertura = x.FechaApertura,
                MontoInicial = x.MontoApertura,
                // ----------------------------------------//
                UsuarioCierreId = x.UsuarioCierreId,
                FechaCierre = x.FechaCierre,
                MontoCierre = x.MontoCierre,
                EstaEliminado = x.Eliminado,
                // ----------------------------------------//
                DetalleCajas = x.Detalle.Select(d => new Dominio.Entidades.DetalleCaja() { 
                    Id = d.Id,
                    CajaId = d.CajaId,
                    TipoMovimiento = d.TipoMovimiento,
                    TipoPago = d.TipoPago,
                    Monto = d.Monto,
                    EstaEliminado = d.Eliminado,
                }).ToList()
            };

    }
}

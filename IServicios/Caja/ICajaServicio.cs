namespace IServicios.Caja
{
    using IServicios.Caja.DTOs;
    using System;
    using System.Collections.Generic;

    public interface ICajaServicio
    {
        bool VerificarSiExisteCajaAbierta(long usuarioId);

        bool VerificarSiCajaFueCerrada(long cajaId);

        decimal ObtenerMontoCajaAnterior(long usuarioId);

        IEnumerable<CajaDto> Obtener(string cadenaBuscar, bool filtroPorFecha, DateTime fechaDesde, DateTime fechaHasta);

        long? ObtenerIdCajaAciva(long usuarioId);

        CajaDto ObtenerCajaAciva(long usuarioId);

        void Abrir(long usuarioId, decimal monto);

        void Cerrar(long cajaId, decimal monto);
    }
}

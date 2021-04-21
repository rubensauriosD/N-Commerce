namespace IServicios.Caja
{
    using IServicios.Caja.DTOs;
    using System;
    using System.Collections.Generic;

    public interface ICajaServicio
    {
        bool VerificarSiExisteCajaAbierta(long usuarioId);

        decimal ObtenerMontoCajaAnterior(long usuarioId);

        IEnumerable<CajaDto> Obtener(string cadenaBuscar, bool filtroPorFecha, DateTime fechaDesde, DateTime fechaHasta);

        long? ObtenerCajaAciva(long usuarioId);

        void Abrir(long usuarioId, decimal monto);

        void Cerrar(long usuarioId, decimal monto);
    }
}

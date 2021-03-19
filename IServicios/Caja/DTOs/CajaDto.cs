namespace IServicios.Caja.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Aplicacion.Constantes;
    using IServicio.BaseDto;

    public class CajaDto : DtoBase
    {
        public CajaDto()
        {
            if (Detalle == null)
                Detalle = new List<CajaDetalleDto>();
        }

        public long UsuarioAperturaId { get; set; }
        public string UsuarioApertura { get; set; }
        public decimal MontoApertura { get; set; }
        public string MontoAperturaStr => MontoApertura.ToString("C2");
        public DateTime FechaApertura { get; set; }
        public string FechaAperturaStr => FechaApertura.ToShortDateString();

        // ==========================================//

        public long? UsuarioCierreId { get; set; }
        public string UsuarioCierre { get; set; }
        public DateTime? FechaCierre { get; set; }
        public string FechaCierreStr => FechaCierre.HasValue ? FechaCierre.Value.ToShortDateString() : "----";

        public decimal MontoCierre =>
            TotalEntradaEfectivo + TotalEntradaCheque + TotalEntradaTarjeta
            - TotalSalidaEfectivo - TotalSalidaCheque - TotalSalidaTarjeta;

        public string MontoCierreStr => MontoCierre == 0 ? MontoCierre.ToString("C2") : "----";

        // ==========================================//

        public decimal TotalEntradaEfectivo => Detalle.Where(x => x.TipoPago == TipoPago.Efectivo && x.TipoMovimiento == TipoMovimiento.Ingreso).Sum(x => x.Monto);
        public string TotalEntradaEfectivoStr => TotalEntradaEfectivo.ToString("C2");
        public decimal TotalSalidaEfectivo => Detalle.Where(x => x.TipoPago == TipoPago.Efectivo && x.TipoMovimiento == TipoMovimiento.Egreso).Sum(x => x.Monto);
        public string TotalSalidaEfectivoStr => TotalSalidaEfectivo.ToString("C2");

        // ==========================================//

        public decimal TotalEntradaTarjeta => Detalle.Where(x => x.TipoPago == TipoPago.Tarjeta && x.TipoMovimiento == TipoMovimiento.Ingreso).Sum(x => x.Monto);
        public string TotalEntradaTarjetaStr => TotalEntradaTarjeta.ToString("C2");
        public decimal TotalSalidaTarjeta => Detalle.Where(x => x.TipoPago == TipoPago.Tarjeta && x.TipoMovimiento == TipoMovimiento.Egreso).Sum(x => x.Monto);
        public string TotalSalidaTarjetaStr => TotalSalidaTarjeta.ToString("C2");

        // ==========================================//

        public decimal TotalEntradaCheque => Detalle.Where(x => x.TipoPago == TipoPago.Cheque && x.TipoMovimiento == TipoMovimiento.Ingreso).Sum(x => x.Monto);
        public string TotalEntradaChequeStr => TotalEntradaCheque.ToString("C2");
        public decimal TotalSalidaCheque => Detalle.Where(x => x.TipoPago == TipoPago.Cheque && x.TipoMovimiento == TipoMovimiento.Egreso).Sum(x => x.Monto);
        public string TotalSalidaChequeStr => TotalSalidaCheque.ToString("C2");

        // ==========================================//

        public decimal TotalEntradaCtaCte => Detalle.Where(x => x.TipoPago == TipoPago.CtaCte && x.TipoMovimiento == TipoMovimiento.Ingreso).Sum(x => x.Monto);
        public string TotalEntradaCtaCteStr => TotalEntradaCtaCte.ToString("C2");
        public decimal TotalSalidaCtaCte => Detalle.Where(x => x.TipoPago == TipoPago.CtaCte && x.TipoMovimiento == TipoMovimiento.Egreso).Sum(x => x.Monto);
        public string TotalSalidaCtaCteStr => TotalSalidaCtaCte.ToString("C2");

        // Colecciones
        public ICollection<CajaDetalleDto> Detalle { get; set; }
    }
}
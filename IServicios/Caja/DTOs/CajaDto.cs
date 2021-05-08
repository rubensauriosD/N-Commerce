namespace IServicios.Caja.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Aplicacion.Constantes;
    using IServicio.BaseDto;
    using IServicio.Caja.DTOs;

    public class CajaDto : DtoBase
    {
        public CajaDto()
        {
            if (Detalle == null)
                Detalle = new List<CajaDetalleDto>();

            if (Gastos == null)
                Gastos = new List<GastoDto>();
        }

        public long UsuarioAperturaId { get; set; }
        public string UsuarioApertura { get; set; }

        public DateTime FechaApertura { get; set; }
        public string FechaAperturaStr => FechaApertura.ToShortDateString();

        public decimal MontoApertura { get; set; } = 0;
        public string MontoAperturaStr => MontoApertura.ToString("C2");
        public decimal MontoCierre { get; set; } = 0;
        public string MontoCierreStr => MontoCierre != 0 ? MontoCierre.ToString("C2") : "----";

        // ==========================================//

        public long? UsuarioCierreId { get; set; }
        public string UsuarioCierre { get; set; }
        public DateTime? FechaCierre { get; set; }
        public string FechaCierreStr => FechaCierre.HasValue ? FechaCierre.Value.ToShortDateString() : "----";

        // ==========================================//

        public decimal TotalIngresos => Detalle.Where(x => x.TipoMovimiento == TipoMovimiento.Ingreso).Sum(x => x.Monto);
        public string TotalIngresosStr => TotalIngresos.ToString("C2");

        public decimal TotalIngresoEfectivo => Detalle.Where(x => x.TipoPago == TipoPago.Efectivo && x.TipoMovimiento == TipoMovimiento.Ingreso).Sum(x => x.Monto);
        public string TotalIngresoEfectivoStr => TotalIngresoEfectivo.ToString("C2");

        public decimal TotalIngresoTarjeta => Detalle.Where(x => x.TipoPago == TipoPago.Tarjeta && x.TipoMovimiento == TipoMovimiento.Ingreso).Sum(x => x.Monto);
        public string TotalIngresoTarjetaStr => TotalIngresoTarjeta.ToString("C2");

        public decimal TotalIngresoCheque => Detalle.Where(x => x.TipoPago == TipoPago.Cheque && x.TipoMovimiento == TipoMovimiento.Ingreso).Sum(x => x.Monto);
        public string TotalIngresoChequeStr => TotalIngresoCheque.ToString("C2");

        public decimal TotalIngresoCtaCte => Detalle.Where(x => x.TipoPago == TipoPago.CtaCte && x.TipoMovimiento == TipoMovimiento.Ingreso).Sum(x => x.Monto);
        public string TotalIngresoCtaCteStr => TotalIngresoCtaCte.ToString("C2");

        // ==========================================//

        public decimal TotalCompras => Detalle.Where(x => x.TipoMovimiento == TipoMovimiento.Egreso).Sum(x => x.Monto);
        public string TotalComprasStr => TotalCompras.ToString("C2");

        public decimal TotalEgresoEfectivo => Detalle.Where(x => x.TipoPago == TipoPago.Efectivo && x.TipoMovimiento == TipoMovimiento.Egreso).Sum(x => x.Monto);
        public string TotalEgresoEfectivoStr => TotalEgresoEfectivo.ToString("C2");
        
        public decimal TotalEgresoTarjeta => Detalle.Where(x => x.TipoPago == TipoPago.Tarjeta && x.TipoMovimiento == TipoMovimiento.Egreso).Sum(x => x.Monto);
        public string TotalEgresoTarjetaStr => TotalEgresoTarjeta.ToString("C2");
        
        public decimal TotalEgresoCheque => Detalle.Where(x => x.TipoPago == TipoPago.Cheque && x.TipoMovimiento == TipoMovimiento.Egreso).Sum(x => x.Monto);
        public string TotalEgresoChequeStr => TotalEgresoCheque.ToString("C2");
        
        public decimal TotalEgresoCtaCte => Detalle.Where(x => x.TipoPago == TipoPago.CtaCte && x.TipoMovimiento == TipoMovimiento.Egreso).Sum(x => x.Monto);
        public string TotalEgresoCtaCteStr => TotalEgresoCtaCte.ToString("C2");

        // ==========================================//

        public decimal TotalGastos => Gastos.Sum(x => x.Monto);
        public string TotalGastosStr => TotalGastos.ToString("C2");

        // ==========================================//

        public decimal TotalEgresos => TotalGastos + TotalCompras;
        public string TotalEgresosStr => TotalEgresos.ToString("C2");

        public decimal MontoCierreCalculado => MontoApertura + TotalIngresos - TotalEgresos;
        public string MontoCierreCalculadoStr => MontoCierreCalculado.ToString("C2");

        // Colecciones
        public List<CajaDetalleDto> Detalle { get; set; }

        public List<GastoDto> Gastos { get; set; }
    }
}
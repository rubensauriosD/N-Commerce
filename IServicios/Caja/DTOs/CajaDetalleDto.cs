namespace IServicios.Caja.DTOs
{
    using Aplicacion.Constantes;
    using IServicio.BaseDto;

    public class CajaDetalleDto : DtoBase
    {
        // Propiedades
        public long CajaId { get; set; }

        public TipoPago TipoPago { get; set; }

        public TipoMovimiento TipoMovimiento { get; set; }

        public decimal Monto { get; set; }

        public string TipoPagoStr => TipoPago.ToString();
    }
}

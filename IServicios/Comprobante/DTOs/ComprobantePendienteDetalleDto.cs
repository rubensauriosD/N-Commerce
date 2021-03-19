namespace IServicios.Comprobante.DTOs
{
    using IServicio.BaseDto;

    public class ComprobantePendienteDetalleDto : DtoBase
    {
        public string Descripcion { get; set; }

        public decimal Precio { get; set; } = 0m;

        public decimal Cantidad { get; set; } = 0m;

        public decimal SubTotal => Precio * Cantidad;

        public string PrecioStr => Precio.ToString("C2");

        public string SubTotalStr => SubTotal.ToString("C2");

    }
}

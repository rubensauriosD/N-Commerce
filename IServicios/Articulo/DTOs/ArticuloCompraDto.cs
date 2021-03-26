namespace IServicios.Articulo.DTOs
{
    public class ArticuloCompraDto
    {
        public long Id { get; set; } = 0;

        public long ProductoId { get; set; } = 0;

        public string Codigo { get; set; } = "";

        public string CodigoBarra { get; set; } = "";

        public string Descripcion { get; set; } = "";

        public decimal Cantidad { get; set; } = 1;

        public decimal Precio { get; set; } = 0;

        public decimal SubTotal => Precio * Cantidad;
    }
}

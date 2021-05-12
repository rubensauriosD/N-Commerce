namespace IServicios.Informes.DTOs
{
    public class InformePresupuestoDetalleDto
    {
        public string Cantidad { get; set; } = 0.ToString("c");

        public string Descripcion { get; set; } = "";

        public string Precio { get; set; } = 0.ToString("c");

        public string Subtotal { get; set; } = 0.ToString("c");
    }
}

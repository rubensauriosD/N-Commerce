namespace IServicios.Comprobante.DTOs
{
    using Aplicacion.Constantes;

    public class FacturaDto : ComprobanteDto
    {
        public long ClienteId { get; set; }

        public long PuestoTrabajoId { get; set; }

        public Estado Estado { get; set; }
    }
}

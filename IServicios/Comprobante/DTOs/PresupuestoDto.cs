namespace IServicios.Comprobante.DTOs
{
    using IServicio.Persona.DTOs;

    public class PresupuestoDto : ComprobanteDto
    {
        public long ClienteId { get; set; }

        public string Cliente { get; set; } = "";
    }
}

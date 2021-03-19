namespace IServicios.Persona.DTOs
{
    public class MovimientoCuentaCorrienteClienteDto : MovimientoDto
    {
        public long ClienteId { get; set; } = 0;

        public long CajaId { get; set; } = 0;
    }
}

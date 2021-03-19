namespace IServicios.Persona.DTOs
{
    public class MovimientoCuentaCorrienteProveedorDto : MovimientoDto
    {
        public long ProveedorId { get; set; } = 0;

        public long CajaId { get; set; } = 0;
    }
}

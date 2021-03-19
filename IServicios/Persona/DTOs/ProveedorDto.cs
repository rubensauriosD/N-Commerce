namespace IServicio.Persona.DTOs
{
    using IServicio.BaseDto;
    using IServicios.Persona.DTOs;
    using System.Collections.Generic;
    using System.Linq;

    public class ProveedorDto : DtoBase
    {
        public ProveedorDto()
        {
            if (MovimientosCuentaCorriente == null)
                MovimientosCuentaCorriente = new List<MovimientoCuentaCorrienteProveedorDto>();
        }

        public string RazonSocial { get; set; }

        public string CUIT { get; set; }

        public string Direccion { get; set; }

        public string Telefono { get; set; }

        public string Mail { get; set; }

        public long LocalidadId { get; set; }

        public long CondicionIvaId { get; set; }

        public string CondicionIva { get; set; }

        public decimal SaldoCuentaCorriente => MovimientosCuentaCorriente
            .Sum(m => m.Monto * (m.TipoMovimiento == Aplicacion.Constantes.TipoMovimiento.Ingreso ? 1 : -1));

        // Colecciones
        public List<MovimientoCuentaCorrienteProveedorDto> MovimientosCuentaCorriente { get; set; }
    }
}

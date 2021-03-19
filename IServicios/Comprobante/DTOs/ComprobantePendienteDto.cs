namespace IServicios.Comprobante.DTOs
{
    using System;
    using System.Collections.Generic;
    using IServicio.BaseDto;

    public class ComprobantePendienteDto : DtoBase
    {
        public ComprobantePendienteDto()
        {
            if (Items == null)
                Items = new List<ComprobantePendienteDetalleDto>();
        }

        public DateTime Fecha { get; set; } = DateTime.Now;

        public long ClienteId { get; set; } = 0;

        public string Cliente { get; set; } = "";

        public int NumeroComprobante { get; set; } = 0;

        public decimal MontoPagar { get; set; } = 0;

        public string MontoPagarStr => MontoPagar.ToString("C2");

        public List<ComprobantePendienteDetalleDto> Items { get; set; }
    }
}

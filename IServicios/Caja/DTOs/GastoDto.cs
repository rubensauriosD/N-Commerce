using System;
using IServicio.BaseDto;

namespace IServicio.Caja.DTOs
{
    public class GastoDto : DtoBase
    {
        public long ConceptoGastoId { get; set; }

        public DateTime Fecha { get; set; }

        public string Descripcion { get; set; }

        public decimal Monto { get; set; }
    }
}

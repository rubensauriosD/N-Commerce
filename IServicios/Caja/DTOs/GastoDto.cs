namespace IServicio.Caja.DTOs
{
    using System;
    using IServicio.BaseDto;

    public class GastoDto : DtoBase
    {
        public long ConceptoGastoId { get; set; }

        public DateTime Fecha { get; set; }

        public string Descripcion { get; set; }

        public decimal Monto { get; set; }

        public long CajaId { get; set; }
    }
}

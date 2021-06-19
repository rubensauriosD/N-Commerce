namespace IServicios.Articulo.DTOs
{
    using System;
    using IServicio.BaseDto;

    public class BajaArticuloDto : DtoBase
    {
        public long ArticuloId { get; set; } = 0;

        public string Articulo { get; set; } = "";

        public decimal Stock { get; set; } = 0;

        public byte[] Foto { get; set; }

        public long MotivoBajaId { get; set; } = 0;

        public string MotivoBaja { get; set; } = "";

        public decimal Cantidad { get; set; } = 0;

        public DateTime Fecha { get; set; }

        public string Observacion { get; set; }
    }
}

namespace IServicio.Articulo.DTOs
{
    using System;
    using System.Globalization;
    using IServicio.BaseDto;

    public class PrecioDto : DtoBase
    {
        public DateTime Fecha { get; set; }

        public decimal PrecioPublico { get; set; }

        public string ListaPrecio { get; set; }

        public string PrecioStr => PrecioPublico.ToString("C2", CultureInfo.CreateSpecificCulture("es-Ar"));

        public string FechaStr => Fecha.ToString("d", CultureInfo.CreateSpecificCulture("es-Ar"));
    }
}

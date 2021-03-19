using Aplicacion.Constantes;

namespace Presentacion.Core.Comprobantes
{
    public class ItemView
    {
        public long Id { get; set; }

        public long ArticuloId { get; set; }

        public long ListaPrecioId { get; set; }

        public bool EsArticuloAlternativo { get; set; }

        public bool IngresoPorBascula { get; set; }

        public string Codigo { get; set; }

        public string Descripcion { get; set; }

        public decimal Iva { get; set; }

        public decimal Precio { get; set; }

        public string PrecioStr => Precio.ToString("C", ConfiguracionPorDefecto.CultureInfo);

        public decimal Cantidad { get; set; }

        // Propiedad (Campo) Calculada/o
        public decimal SubTotal => Precio * Cantidad + Iva;

        public string SubTotalStr => SubTotal.ToString("C", ConfiguracionPorDefecto.CultureInfo);


    }
}

namespace IServicios.Deposito.DTOs
{
    using IServicio.BaseDto;

    public class StockDto : DtoBase
    {
        public long ArticuloId { get; set; }

        public string Articulo { get; set; }

        public decimal Cantidad { get; set; }
    }
}

namespace IServicios.Deposito.DTOs
{
    public class TransferenciaDepositoDto
    {
        public long DestinoId { get; set; } = 0;

        public long OrigenId { get; set; } = 0;

        public long ArticuloId { get; set; } = 0;

        public decimal Cantidad { get; set; } = 0;

        public string Destino { get; set; } = "";

        public string Origen { get; set; } = "";

        public string Articulo { get; set; } = "";

    }
}

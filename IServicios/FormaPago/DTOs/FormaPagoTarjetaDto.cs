namespace IServicios.FormaPago.DTOs
{
    public class FormaPagoTarjetaDto : FormaPagoDto
    {
        public long TarjetaId { get; set; }

        public string NumeroTarjeta { get; set; }

        public string CuponPago { get; set; }

        public int CantidadCuotas { get; set; }
    }
}

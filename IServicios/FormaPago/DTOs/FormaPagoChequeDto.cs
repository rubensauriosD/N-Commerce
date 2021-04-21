namespace IServicios.FormaPago.DTOs
{
    using System;

    public class FormaPagoChequeDto : FormaPagoDto
    {
        public long BancoId { get; set; }

        public string Numero { get; set; }

        public DateTime FechaVencimiento { get; set; }
    }
}

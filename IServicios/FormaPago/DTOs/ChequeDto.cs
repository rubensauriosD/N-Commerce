namespace IServicios.FormaPago.DTOs
{
    using System;
    using IServicio.BaseDto;

    public class ChequeDto : DtoBase
    {
        public DateTime FechaVencimiento { get; set; }

        public DateTime? FechaDeposito { get; set; }

        public string Numero { get; set; } = "";

        public bool Depositado { get; set; } = false;

        public long ClienteId { get; set; } = 0;

        public string Cliente { get; set; } = "";

        public long BancoId { get; set; } = 0;

        public string Banco { get; set; } = "";

        public string DepositadoStr => Depositado ? "SI" : "NO";

        public string FechaDepositoStr => FechaDeposito != null ? ((DateTime)FechaDeposito).ToString("d") : "---";

        public bool Vencido => FechaVencimiento >= DateTime.Today;

        public bool PorVencer => FechaVencimiento < DateTime.Today;
    }
}

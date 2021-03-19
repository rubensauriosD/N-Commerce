namespace IServicios.FormaPago.DTOs
{
    using Aplicacion.Constantes;
    using IServicio.BaseDto;

    public class FormaPagoDto : DtoBase
    {
        public TipoPago TipoPago { get; set; }

        public decimal Monto { get; set; }
    }
}

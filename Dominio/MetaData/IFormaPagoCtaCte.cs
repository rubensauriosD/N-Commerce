using System.ComponentModel.DataAnnotations;

namespace Dominio.MetaData
{
    public interface IFormaPagoCtaCte : IFormaPago
    {
        //[Required(ErrorMessage = "El campo {0} es Obligatorio")]
        //long ClienteId { get; set; }

        [Required(ErrorMessage = "El campo {0} es Obligatorio")]
        long MovimientoCuentaCorrienteId { get; set; }
    }
}

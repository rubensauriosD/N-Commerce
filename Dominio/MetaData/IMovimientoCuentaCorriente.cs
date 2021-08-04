namespace Dominio.MetaData
{
    using System.ComponentModel.DataAnnotations;

    public interface IMovimientoCuentaCorriente : IMovimiento
    {
        [Required(ErrorMessage = "El campo {0} es Obligatorio.")]
        long ClienteId { get; set; }
    }
}

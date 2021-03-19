namespace Dominio.MetaData
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Aplicacion.Constantes;

    public interface IMovimiento
    {
        [Required(ErrorMessage = "El campo {0} es Obligatorio.")]
        [DataType(DataType.Currency)]
        decimal Monto { get; set; }

        [Required(ErrorMessage = "El campo {0} es Obligatorio.")]
        [DataType(DataType.DateTime)]
        DateTime Fecha { get; set; }

        [Required(ErrorMessage = "El campo {0} es Obligatorio.")]
        [StringLength(4000, ErrorMessage = "El campo {0} debe ser menor a {1} caracteres.")]
        string Descripcion { get; set; }

        [Required(ErrorMessage = "El campo {0} es Obligatorio.")]
        TipoMovimiento TipoMovimiento { get; set; }
    }
}

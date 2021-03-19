namespace Dominio.MetaData
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public interface ICaja
    {
        [Required(ErrorMessage = "El campo {0} es Obligatorio.")]
        long UsuarioAperturaId { get; set; }

        [Required(ErrorMessage = "El campo {0} es Obligatorio.")]
        decimal MontoInicial { get; set; }

        [Required(ErrorMessage = "El campo {0} es Obligatorio.")]
        DateTime FechaApertura { get; set; }

        // ===================================================== //
        // ============      Cierre de Caja      =============== //
        // ===================================================== //

        long? UsuarioCierreId { get; set; }

        [DataType(DataType.DateTime)]
        DateTime? FechaCierre { get; set; }
        
        [DataType(DataType.Currency)]
        decimal? MontoCierre { get; set; }

    }
}

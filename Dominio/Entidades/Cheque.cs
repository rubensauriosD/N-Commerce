namespace Dominio.Entidades
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Dominio.MetaData;

    [Table("Cheque")]
    [MetadataType(typeof(ICheque))]
    public class Cheque : EntidadBase
    {
        // Propiedades
        public long ClienteId { get; set; }

        public long BancoId { get; set; }

        public string Numero { get; set; }

        public DateTime FechaVencimiento { get; set; }

        public DateTime? FechaDeposito { get; set; }

        public bool Depositado { get; set; }

        // Propiedades de Navegacion
        public virtual Banco Banco { get; set; }

        public virtual Cliente Cliente { get; set; }
    }
}

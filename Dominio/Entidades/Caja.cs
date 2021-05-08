namespace Dominio.Entidades
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Dominio.MetaData;

    [Table("Caja")]
    [MetadataType(typeof(ICaja))]
    public class Caja : EntidadBase
    {
        // ==========================================//
        public long UsuarioAperturaId { get; set; }

        public decimal MontoInicial { get; set; }

        public DateTime FechaApertura { get; set; }

        // ==========================================//

        public long? UsuarioCierreId { get; set; }

        public DateTime? FechaCierre { get; set; }

        public decimal? MontoCierre { get; set; }


        // Propiedades de Navegacion
        public virtual Usuario UsuarioApertura { get; set; }

        public virtual Usuario UsuarioCierre { get; set; }

        public virtual ICollection<DetalleCaja> DetalleCajas { get; set; }

        [ForeignKey("CajaId")]
        public virtual ICollection<Gasto> Gastos { get; set; }
    }
}

namespace Dominio.Entidades
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Dominio.MetaData;

    [Table("Banco")]
    [MetadataType(typeof(IBanco))]
    public class Banco : EntidadBase
    {
        // Propiedades
        public string Descripcion { get; set; }

        // Propiedades de Navegacion
        public virtual ICollection<Cheque> Cheques { get; set; }
    }
}

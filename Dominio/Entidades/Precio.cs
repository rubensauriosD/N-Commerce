namespace Dominio.Entidades
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Dominio.MetaData;

    [Table("Precio")]
    [MetadataType(typeof(IPrecio))]
    public class Precio : EntidadBase
    {
        // Propiedades
        public long ArticuloId { get; set; }

        public decimal PrecioCosto { get; set; }

        public DateTime FechaActualizacion { get; set; }

        // Propiedades de Navegacion
        public virtual Articulo Articulo { get; set; }
    }
}
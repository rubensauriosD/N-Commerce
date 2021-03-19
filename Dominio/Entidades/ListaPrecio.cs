namespace Dominio.Entidades
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Dominio.MetaData;

    [Table("ListaPrecio")]
    [MetadataType(typeof(IListaPrecio))]
    public class ListaPrecio : EntidadBase
    {
        // Propiedades
        public string Descripcion { get; set; }

        public decimal PorcentajeGanancia { get; set; }

        public bool NecesitaAutorizacion { get; set; }

        // Propiedades de Navegacion
        public virtual ICollection<Configuracion> Configuraciones { get; set; }
    }
}
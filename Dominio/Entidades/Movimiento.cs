namespace Dominio.Entidades
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Aplicacion.Constantes;
    using Dominio.MetaData;

    [Table("Movimiento")]
    [MetadataType(typeof(IMovimiento))]
    public class Movimiento : EntidadBase
    {
        // Propiedades
        public decimal Monto { get; set; }

        public DateTime Fecha { get; set; }

        public string Descripcion { get; set; }

        public TipoMovimiento TipoMovimiento { get; set; }
    }
}
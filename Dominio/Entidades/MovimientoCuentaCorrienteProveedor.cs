namespace Dominio.Entidades
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Dominio.MetaData;

    [Table("Movimiento_CuentaCorrienteProveedor")]
    [MetadataType(typeof(IMovimientoCuentaCorrienteProveedor))]
    public class MovimientoCuentaCorrienteProveedor : Movimiento
    {
        // Propiedades 
        public long ProveedorId { get; set; }

        // Propiedades de Navegacion
        public virtual Proveedor Proveedor { get; set; }
    }
}

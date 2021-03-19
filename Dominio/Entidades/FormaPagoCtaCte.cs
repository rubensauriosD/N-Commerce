namespace Dominio.Entidades
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Dominio.MetaData;

    [Table("FormaPago_CtaCte")]
    [MetadataType(typeof(IFormaPagoCtaCte))]
    public class FormaPagoCtaCte :FormaPago
    {
        // Propiedades
        public long MovimientoCuentaCorrienteId { get; set; }

        // Propiedades de Navegacion
        public virtual MovimientoCuentaCorriente MovimientoCuentaCorriente { get; set; }
    }
}

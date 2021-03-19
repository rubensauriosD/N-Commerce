namespace IServicios.Persona.DTOs
{
    using Aplicacion.Constantes;
    using IServicio.BaseDto;
    using System;

    public class MovimientoDto : DtoBase
    {
        public decimal Monto { get; set; }

        public DateTime Fecha { get; set; }

        public string Descripcion { get; set; }

        public TipoMovimiento TipoMovimiento { get; set; }

        public decimal MontoSignado => Monto * (TipoMovimiento == TipoMovimiento.Ingreso ? 1 : -1);
    }
}

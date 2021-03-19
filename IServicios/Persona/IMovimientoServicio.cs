namespace IServicio.Persona
{
    using System;
    using System.Collections.Generic;
    using IServicios.Persona.DTOs;

    public interface IMovimientoServicio
    {
        long Insertar(MovimientoDto persona);

        void Modificar(MovimientoDto persona);

        void Eliminar(Type tipoEntidad, long id);

        MovimientoDto Obtener(Type tipoEntidad, long id);

        IEnumerable<MovimientoDto> Obtener(Type tipoEntidad, string cadenaBuscar);

        // ==================================================================== //

        void AgregarOpcionDiccionario(Type type, string value);
    }
}

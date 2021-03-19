namespace Servicios.Persona
{
    using Dominio.UnidadDeTrabajo;
    using IServicio.Persona;
    using IServicios.Persona.DTOs;
    using System;
    using System.Collections.Generic;

    public class MovimientoServicio : IMovimientoServicio
    {
        protected readonly IUnidadDeTrabajo _unidadDeTrabajo;
        private Dictionary<Type, string> _diccionario;

        public MovimientoServicio(IUnidadDeTrabajo unidadDeTrabajo)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
            _diccionario = new Dictionary<Type, string>();

            InicializadorDiccionario();
        }

        private void InicializadorDiccionario()
        {
            _diccionario.Add(typeof(MovimientoCuentaCorrienteProveedorDto), "Servicios.MovimientoCuentaCorrienteProveedor");
            //_diccionario.Add(typeof(ClienteDto), "Servicios.Movimiento.Cliente");
        }

        public void AgregarOpcionDiccionario(Type type, string value)
        {
            _diccionario.Add(type, value);
        }

        public long Insertar(MovimientoDto entidad)
        {
            var movimiento = InstanciaMovimiento(entidad);

            return movimiento.Insertar(entidad);
        }

        public void Eliminar(Type tipoEntidad, long id)
        {
            var movimiento = InstanciarMovimiento(tipoEntidad);

            movimiento.Eliminar(id);
        }

        public IEnumerable<MovimientoDto> Obtener(Type tipo, string cadenaBuscar)
        {
            var movimiento = InstanciarMovimiento(tipo);

            return movimiento.Obtener(cadenaBuscar);
        }

        public MovimientoDto Obtener(Type tipo, long id)
        {
            var movimiento = InstanciarMovimiento(tipo);

            return movimiento.Obtener(id);
        }

        public void Modificar(MovimientoDto entidad)
        {
            var movimiento = InstanciaMovimiento(entidad);

            movimiento.Modificar(entidad);
        }

        // ====================================================================== //
        // =================         Metodos Privados           ================= //
        // ====================================================================== //

        private Movimiento InstanciarEntidad(string tipoEntidad)
        {
            var tipoObjeto = Type.GetType(tipoEntidad);

            if (tipoObjeto == null) return null;

            var entidad = Activator.CreateInstance(tipoObjeto) as Movimiento;

            return entidad;
        }

        private Movimiento InstanciaMovimiento(MovimientoDto entidad)
        {
            if (!_diccionario.TryGetValue(entidad.GetType(), out var tipoEntidad))
                throw new Exception($"No hay {entidad.GetType()} para Instanciar.");

            var movimiento = InstanciarEntidad(tipoEntidad);

            if (movimiento == null) throw new Exception($"Ocurrió un error al Instanciar {entidad.GetType()}");

            return movimiento;
        }

        private Movimiento InstanciarMovimiento(Type tipo)
        {
            if (!_diccionario.TryGetValue(tipo, out var tipoEntidad))
                throw new Exception($"No hay {tipoEntidad} para Instanciar.");

            var movimiento = InstanciarEntidad(tipoEntidad);

            if (movimiento == null) throw new Exception($"Ocurrió un error al Instanciar {tipo}");

            return movimiento;
        }
    }
}

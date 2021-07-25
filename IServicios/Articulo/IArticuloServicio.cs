namespace IServicio.Articulo
{
    using IServicio.Articulo.DTOs;
    using IServicios.Articulo.DTOs;
    using System.Collections.Generic;

    public interface IArticuloServicio : Base.IServicio
    {
        int ObtenerCantidadArticulos();
        
        int ObtenerSigueinteNroCodigo();

        bool VerificarSiExiste(string datoVerificar, long? entidadId = null);

        bool VerificarSiExisteCodigo(int codigo, long? entidadId = null);

        bool VerificarSiExisteCodigoBarra(string codigoBarra, long? entidadId = null);

        IEnumerable<ArticuloVentaDto> ObtenerLookUp(string cadenaBuasar, long listaPreciosId);

        ArticuloVentaDto ObtenerPorCodigo(string codigo, long depositoId, long listaPrecioId);

        ArticuloCompraDto ObtenerPorCodigo(string codigo);

        bool ModificarPrecioPorPorcentaje(List<ArticuloDto> articulos, decimal porcentaje);

        bool ModificarPrecioPorPrecio(List<ArticuloDto> articulos, decimal monto);
    }

}

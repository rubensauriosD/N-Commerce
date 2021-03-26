namespace Presentacion.Core.Comprobantes.Clases
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Aplicacion.Constantes;
    using IServicio.Persona.DTOs;
    using IServicios.Articulo.DTOs;

    public class CompraView
    {
        public CompraView()
        {
            Items = new List<ArticuloCompraDto>();
        }

        public ProveedorDto Proveedor { get; set; }

        public TipoComprobante Tipo { get; set; }

        public DateTime Fecha { get; set; }

        public decimal Iva27 { get; set; } = 0;
        
        public decimal Iva21 { get; set; } = 0;
        
        public decimal Iva105 { get; set; } = 0;
        
        public decimal RetencionIva { get; set; } = 0;
        
        public decimal RetencionIB { get; set; } = 0;
        
        public decimal ImpuestosInternos { get; set; } = 0;
        
        public decimal RetencionTEM { get; set; } = 0;
        
        public decimal RetencionPyP { get; set; } = 0;
        
        public List<ArticuloCompraDto> Items { get; private set; }

        public void AgregarItem(ArticuloCompraDto item)
        {
            var unificarLineas = Items.Any(x => x.Codigo == item.Codigo && x.Precio == item.Precio);

            if (unificarLineas)
            {
                var it = Items.First(x => x.Codigo == item.Codigo
                                        && x.Precio == item.Precio);
                it.Cantidad += item.Cantidad;
                return;
            }

            item.Id = Items.Any() ? Items.Max(x => x.Id) + 1 : 1;
            Items.Add(item);
        }

        public void EliminarItem(long id)
        {
            var item = Items.FirstOrDefault(x => x.Id == id);

            if (item == null)
            {
                Mjs.Error("No se pudo obtener el item a eliminar.");
                return;
            }

            Items.Remove(item);
        }
    }
}

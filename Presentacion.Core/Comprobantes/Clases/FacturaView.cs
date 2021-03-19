using Aplicacion.Constantes;
using IServicio.Articulo;
using IServicio.Persona.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Presentacion.Core.Comprobantes
{
    public class FacturaView
    {
        public FacturaView()
        {
            Items = new List<ItemView>();
        }

        // Cabecera
        public ClienteDto Cliente { get; set; }

        public EmpleadoDto Vendedor { get; set; }

        public long PuestoVentaId { get; set; }

        public long UsuarioId { get; set; }

        public TipoComprobante TipoComprobante { get; set; }

        // Cuerpo
        public List<ItemView> Items;


        // Pie
        public decimal Subtotal => Items.Sum(x => x.SubTotal);

        public string SubtotalStr => Subtotal.ToString("C", ConfiguracionPorDefecto.CultureInfo);

        public decimal Descuento { get; set; }

        public decimal Total => Subtotal * (1 - Descuento / 100m);

        public string TotalStr => Total.ToString("C", ConfiguracionPorDefecto.CultureInfo);

        public void AgregarItem(ItemView item, bool unificarLineas)
        {
            if (unificarLineas 
                && !item.EsArticuloAlternativo
                && !item.IngresoPorBascula
                && Items.Any(x => x.Codigo == item.Codigo
                                  && x.ListaPrecioId == item.ListaPrecioId
                                  && !x.EsArticuloAlternativo
                                  && !x.IngresoPorBascula))
            {
                var it = Items.First(x => x.Codigo == item.Codigo
                                        && x.ListaPrecioId == item.ListaPrecioId);
                it.Cantidad += item.Cantidad;
                return;
            }

            item.Id = Items.Any() ? Items.Max(x => x.Id) + 1 : 1;
            Items.Add(item);
        }

        public void EliminarItem(long? id)
        {
            if (id == null)
                return;

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

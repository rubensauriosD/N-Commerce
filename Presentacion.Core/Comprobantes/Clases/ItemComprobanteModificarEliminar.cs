using System;
using Aplicacion.Constantes;
using PresentacionBase.Formularios;

namespace Presentacion.Core.Comprobantes.Clases
{
    public partial class ItemComprobanteModificarEliminar : FormBase
    {
        private ItemView item;
        public long? EliminarItemId { get; private set; }

        public ItemComprobanteModificarEliminar(ItemView item)
        {
            InitializeComponent();
            this.item = item;
            EliminarItemId = null;
        }

        private void ItemComprobanteModificarEliminar_Load(object sender, EventArgs e)
        {
            if (item == null)
            {
                Mjs.Error("No se seleccionó ningún item.");
                Close();
                return;
            }

            lblDescripcion.Text = item.Descripcion;
            nudCantidad.Value = item.Cantidad;
            nudCantidad.Maximum = item.Cantidad;
            nudCantidad.Select(0, nudCantidad.Text.Length);

        }

        private void btnEjecutar_Click(object sender, EventArgs e)
        {
            item.Cantidad = nudCantidad.Value;

            // Si no tengo cantidad de item, lo borro
            if (item.Cantidad == 0)
                EliminarItemId = item.Id;
            
            Close();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (!Mjs.Preguntar("¿Seguro que desea eliminar el item seleccionado?"))
                return;

            EliminarItemId = item.Id;
            Close();
        }
    }
}

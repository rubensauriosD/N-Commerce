using Aplicacion.Constantes;
using IServicio.Configuracion;
using IServicios.Caja;
using PresentacionBase.Formularios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentacion.Core.Caja
{
    public partial class _00039_AperturaCaja : FormBase
    {
        private readonly ICajaServicio cajaServicio;
        private readonly IConfiguracionServicio configuracionServicio;

        public _00039_AperturaCaja(ICajaServicio cajaServicio, IConfiguracionServicio configuracionServicio)
        {
            InitializeComponent();
            this.cajaServicio = cajaServicio;
            this.configuracionServicio = configuracionServicio;

            DoubleBuffered = true;
        }

        private void _00039_AperturaCaja_Load(object sender, EventArgs e)
        {
            var config = configuracionServicio.Obtener();

            nudMonto.Value = config.IngresoManualCajaInicial ? 0
                : cajaServicio.ObtenerMontoCajaAnterior(Identidad.UsuarioId);
            nudMonto.Select(0, nudMonto.Text.Length);
            nudMonto.Focus();
        }

        //
        // Acciones de botones
        //
        private void btnEjecutar_Click(object sender, EventArgs e)
        {
            try
            {
                cajaServicio.Abrir(Identidad.UsuarioId, nudMonto.Value);
                Mjs.Info("Los datos se guardaron correctamente.");
                Close();
            }
            catch (Exception ex)
            {
                Mjs.Error(ex.Message);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarControles(this);
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

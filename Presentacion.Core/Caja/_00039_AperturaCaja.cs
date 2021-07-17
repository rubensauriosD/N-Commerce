namespace Presentacion.Core.Caja
{
    using Aplicacion.Constantes;
    using IServicio.Configuracion;
    using IServicios.Caja;
    using PresentacionBase.Formularios;
    using System;

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

            if (config.IngresoManualCajaInicial)
            {
                nudMonto.Value = 0;
                nudMonto.Select(0, nudMonto.Text.Length);
                nudMonto.Focus();
            }
            else
            {
                var montoCajaAnterior = cajaServicio.ObtenerMontoCajaAnterior(Identidad.UsuarioId);

                cajaServicio.Abrir(Identidad.UsuarioId, montoCajaAnterior);

                Mjs.Info(
                    "Caja abierta correctamente."
                    +$@"{Environment.NewLine}Monto de apertura: {montoCajaAnterior.ToString("C")}"
                    );

                Close();
            }
        }

        // --- Acciones de botones
        private void btnEjecutar_Click(object sender, EventArgs e)
        {
            cajaServicio.Abrir(Identidad.UsuarioId, nudMonto.Value);
            Mjs.Info("Los datos se guardaron correctamente.");
            Close();
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

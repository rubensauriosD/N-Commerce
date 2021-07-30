namespace Presentacion.Core.Articulo
{
    using System;
    using System.Collections.Generic;
    using StructureMap;
    using Aplicacion.Constantes;
    using IServicio.Articulo;
    using IServicio.Articulo.DTOs;
    using PresentacionBase.Formularios;
    using IServicio.Marca;
    using IServicio.Rubro;
    using IServicio.Marca.DTOs;
    using IServicio.Rubro.DTOs;
    using System.Linq;

    public partial class _00031_ActualizarPrecios : FormBase
    {
        private IArticuloServicio _articuloServicios;
        private IMarcaServicio _marcaServicios;
        private IRubroServicio _rubroServicios;
        private Validar Validar;

        private List<ArticuloDto> lstArticulosFiltrados;
        private List<ArticuloDto> lstArticulos;

        public _00031_ActualizarPrecios()
        {
            InitializeComponent();

            _articuloServicios = ObjectFactory.GetInstance<IArticuloServicio>();
            _marcaServicios = ObjectFactory.GetInstance<IMarcaServicio>();
            _rubroServicios = ObjectFactory.GetInstance<IRubroServicio>();
            Validar = new Validar();

            lstArticulosFiltrados = new List<ArticuloDto>();
            lstArticulos = (List<ArticuloDto>)_articuloServicios.Obtener(string.Empty);

            SetearControles();
        }

        private void SetearControles()
        {
            chkArticulo.Checked = false;
            chkRubro.Checked = false;
            chkArticulo.Checked = false;

            cmbMarca.Enabled = false;
            CargarComboMarca();

            cmbRubro.Enabled = false;
            CargarComboRubro();

            nudCodigoDesde.Value = 0m;
            nudCodigoDesde.Enabled = chkArticulo.Checked;

            nudCodigoHasta.Value = 0m;
            nudCodigoHasta.Enabled = chkArticulo.Checked;

            nudAjuste.Minimum = 0;
            nudAjuste.Maximum = 500;
            nudAjuste.DecimalPlaces = 0;
            nudAjuste.Value = 100;
            chkMarca.Focus();
        }

        private void CargarComboRubro(long id = 0)
        {
            var lstRubro = _rubroServicios.Obtener(string.Empty, false)
                .Select(x => (RubroDto)x)
                .ToList();

            if (id != 0)
            {
                RubroDto rubro = (RubroDto)_rubroServicios.Obtener(id);
                lstRubro.Add(rubro);
            }

            PoblarComboBox(cmbRubro, lstRubro, "Descripcion", "Id");
        }

        private void CargarComboMarca(long id = 0)
        {
            var lstMarca = _marcaServicios.Obtener(string.Empty, false)
                .Select(x => (MarcaDto)x)
                .ToList();

            if (id != 0)
            {
                MarcaDto marca = (MarcaDto)_marcaServicios.Obtener(id);
                lstMarca.Add(marca);
            }

            PoblarComboBox(cmbMarca, lstMarca, "Descripcion", "Id");
        }

        private void chkMarca_CheckedChanged(object sender, EventArgs e)
        {
            cmbMarca.Enabled = chkMarca.Checked;
        }

        private void chkRubro_CheckedChanged(object sender, EventArgs e)
        {
            cmbRubro.Enabled = chkRubro.Checked;
        }

        private void chkArticulo_CheckedChanged(object sender, EventArgs e)
        {
            nudCodigoDesde.Enabled = chkArticulo.Checked;
            nudCodigoHasta.Enabled = chkArticulo.Checked;
        }

        private void rdbPorcentaje_CheckedChanged(object sender, EventArgs e)
        {
            if (!rdbPorcentaje.Checked)
                return;

            nudAjuste.Minimum = 0;
            nudAjuste.Maximum = 500;
            nudAjuste.DecimalPlaces = 0;
            nudAjuste.Value = 100;
            nudAjuste.Focus();
            nudAjuste.Select(0,3);

        }

        private void RdbPrecio_CheckedChanged(object sender, EventArgs e)
        {
            if (!rdbPrecio.Checked)
                return;

            nudAjuste.Minimum = -999999;
            nudAjuste.Maximum = 999999;
            nudAjuste.DecimalPlaces = 2;
            nudAjuste.Value = 0;
            nudAjuste.Focus();
            nudAjuste.Select(0, 1);
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            chkArticulo.Checked = false;
            chkRubro.Checked = false;
            chkArticulo.Checked = false;

            cmbMarca.Enabled = false;
            cmbRubro.Enabled = false;

            nudCodigoDesde.Value = 0m;
            nudCodigoDesde.Enabled = chkArticulo.Checked;
            
            nudCodigoHasta.Value = 0m;
            nudCodigoHasta.Enabled = chkArticulo.Checked;

            rdbPorcentaje.PerformClick();
        }

        private void btnEjecutar_Click(object sender, EventArgs e)
        {
            if (!ValidarDatosCargados())
                return;

            FiltrarArticulos();

            if (lstArticulosFiltrados.Count < 1)
            {
                Mjs.Alerta("No hay artículos que cumplan todos los requisitos.");
                return;
            }

            if (rdbPorcentaje.Checked
                && !_articuloServicios.ModificarPrecioPorPorcentaje(lstArticulosFiltrados, nudAjuste.Value))
                return;

            if (rdbPrecio.Checked
                && !_articuloServicios.ModificarPrecioPorPrecio(lstArticulosFiltrados, nudAjuste.Value))
                return;

            Mjs.Info("Datos actualizados correctamente.");
            Close();
        }

        private bool ValidarDatosCargados()
        {
            var ok = true;

            if (chkMarca.Checked && cmbMarca.SelectedValue == null)
            {
                Validar.SetErrorProvider(cmbMarca, "Seleccionar una marca.");
                ok = false;
            }
            else Validar.ClearErrorProvider(cmbMarca);

            if (chkRubro.Checked && cmbRubro.SelectedValue == null)
            {
                Validar.SetErrorProvider(cmbRubro, "Seleccionar una rubro.");
                ok = false;
            }
            else Validar.ClearErrorProvider(cmbRubro);

            bool porcentajeAlto = rdbPorcentaje.Checked && nudAjuste.Value >= 200;
            if (porcentajeAlto && !Mjs.Preguntar($@"El porcentaje ingresado es alto.{Environment.NewLine} ¿Seguro que desea continuar?"))
                ok = false;

            bool precioAlto = rdbPorcentaje.Checked && nudAjuste.Value >= 10000;
            if (precioAlto && !Mjs.Preguntar($@"El precio ingresado es alto.{Environment.NewLine} ¿Seguro que desea continuar?"))
                ok = false;

            bool noHayCriteriosDeFiltrado = !chkArticulo.Checked && !chkMarca.Checked && !chkRubro.Checked;
            if (noHayCriteriosDeFiltrado && !Mjs.Preguntar($@"Está por modificar el precio de todos los artículos en existencia.{Environment.NewLine}¿Seguro que desea continuar?"))
                ok = false;

            return ok;
        }

        private void FiltrarArticulos()
        {
            lstArticulosFiltrados = lstArticulos;

            if(chkMarca.Checked)
                lstArticulosFiltrados = lstArticulosFiltrados
                    .Where( x => x.MarcaId == (long)cmbMarca.SelectedValue)
                    .ToList();

            if(chkRubro.Checked)
                lstArticulosFiltrados = lstArticulosFiltrados
                    .Where( x => x.RubroId == (long) cmbRubro.SelectedValue)
                    .ToList();

            if(chkArticulo.Checked)
                lstArticulosFiltrados = lstArticulosFiltrados
                    .Where( x => x.Codigo >= nudCodigoDesde.Value && x.Codigo <= nudCodigoHasta.Value)
                    .ToList();

        }
    }
}

using System;
using System.Diagnostics.Eventing.Reader;
using System.Windows.Forms;

namespace PresentacionBase.Formularios
{
    public partial class FormBase : Form
    {
        public FormBase()
        {
            InitializeComponent();
        }

        protected void Control_Leave(object sender, EventArgs e)
        {
            switch (sender)
            {
                case TextBox box:
                    box.BackColor = Aplicacion.Constantes.Color.ControlSinFoco;
                    break;
                case NumericUpDown down:
                    down.BackColor = Aplicacion.Constantes.Color.ControlSinFoco;
                    break;
            }
        }

        protected void Control_Enter(object sender, EventArgs e)
        {
            switch (sender)
            {
                case TextBox box:
                    box.BackColor = Aplicacion.Constantes.Color.ControlConFoco;
                    break;
                case NumericUpDown down:
                    down.BackColor = Aplicacion.Constantes.Color.ControlConFoco;
                    break;
            }
        }

        public virtual void DesactivarControles(object obj)
        {
            switch (obj)
            {
                // Controles                
                case TextBox ctrl:
                    ctrl.Enabled = false;
                    break;

                case RichTextBox ctrl:
                    ctrl.Enabled = false;
                    break;

                case NumericUpDown ctrl:
                    ctrl.Enabled = false;
                    break;

                case ComboBox ctrl:
                    ctrl.Enabled = false;
                    break;

                case DateTimePicker ctrl:
                    ctrl.Enabled = false;
                    break;

                // Contenedores
                case GroupBox cont:
                    foreach (object ctrl in cont.Controls)
                        DesactivarControles(ctrl);
                    break;

                case TabPage cont:
                    foreach (object ctrl in cont.Controls)
                        DesactivarControles(ctrl);
                    break;

                case TabControl cont:
                    foreach (object ctrl in cont.Controls)
                        DesactivarControles(ctrl);
                    break;

                case Panel cont:
                    foreach (object ctrl in cont.Controls)
                        DesactivarControles(ctrl);
                    break;

                case Form cont:
                    foreach (object ctrl in cont.Controls)
                        DesactivarControles(ctrl);
                    break;

                default:
                    break;
            }
        }

        public virtual void LimpiarControles(object obj, bool tieneValorAsociado = false)
        {
            switch (obj)
            {
                // Controles                
                case TextBox ctrl:
                    ctrl.Clear();
                    break;

                case RichTextBox ctrl:
                    ctrl.Clear();
                    break;

                case NumericUpDown ctrl:
                    ctrl.Value = ctrl.Minimum;
                    break;

                case ComboBox ctrl:
                    ctrl.SelectedIndex = 0;
                    break;

                case DateTimePicker ctrl:
                    ctrl.Value = DateTime.Now;
                    break;

                // Contenedores
                case GroupBox cont:
                    foreach (object ctrl in cont.Controls)
                        LimpiarControles(ctrl);
                    break;

                case TabPage cont:
                    foreach (object ctrl in cont.Controls)
                        LimpiarControles(ctrl);
                    break;

                case TabControl cont:
                    foreach (object ctrl in cont.Controls)
                        LimpiarControles(ctrl);
                    break;

                case Panel cont:
                    foreach (object ctrl in cont.Controls)
                        LimpiarControles(ctrl);
                    break;

                case Form cont:
                    foreach (object ctrl in cont.Controls)
                        LimpiarControles(ctrl);
                    break;

                default:
                    break;
            }            
        }

        protected void AsignarEvento_EnterLeave(object obj)
        {
            if (obj is Form)
            {
                foreach (var controlForm in ((Form)obj).Controls)
                {
                    if (controlForm is TextBox)
                    {
                        ((TextBox)controlForm).Enter += Control_Enter;
                        ((TextBox)controlForm).Leave += Control_Leave;
                        continue;
                    }

                    if (controlForm is NumericUpDown)
                    {
                        ((NumericUpDown)controlForm).Enter += Control_Enter;
                        ((NumericUpDown)controlForm).Leave += Control_Leave;
                        continue;
                    }

                    if (controlForm is Panel)
                    {
                        AsignarEvento_EnterLeave(controlForm);
                    }
                }
            }
            else
            {
                if (obj is Panel)
                {
                    foreach (var ControlPanel in ((Panel)obj).Controls)
                    {
                        if (ControlPanel is TextBox)
                        {
                            ((TextBox)ControlPanel).Enter += Control_Enter;
                            ((TextBox)ControlPanel).Leave += Control_Leave;
                            continue;
                        }

                        if (ControlPanel is NumericUpDown)
                        {
                            ((NumericUpDown)ControlPanel).Enter += Control_Enter;
                            ((NumericUpDown)ControlPanel).Leave += Control_Leave;
                            continue;
                        }

                        if (ControlPanel is Panel)
                        {
                            AsignarEvento_EnterLeave(ControlPanel);
                        }
                    }
                }
            }
        }

        public virtual void PoblarComboBox(ComboBox cmb,
             object datos,
            string PropiedadMostrar = "",
            string propiedadDevolver = "")
        {
            cmb.DropDownStyle = ComboBoxStyle.DropDownList;
            cmb.DataSource = datos;


            if (!string.IsNullOrEmpty(PropiedadMostrar))
            {
                cmb.DisplayMember = PropiedadMostrar;
            }

            if (!string.IsNullOrEmpty(propiedadDevolver))
            {
                cmb.ValueMember = propiedadDevolver;
            }
        }

        public virtual void PoblarComboBox(ComboBox cmb,
            object datos,
            long idSeleccionado,
            string PropiedadMostrar = "",
            string propiedadDevolver = "")
        {
            cmb.DropDownStyle = ComboBoxStyle.DropDownList;
            cmb.DataSource = datos;


            if (!string.IsNullOrEmpty(PropiedadMostrar))
            {
                cmb.DisplayMember = PropiedadMostrar;
            }

            if (!string.IsNullOrEmpty(propiedadDevolver))
            {
                cmb.ValueMember = propiedadDevolver;
            }
            
            cmb.SelectedValue = idSeleccionado;
        }

        public virtual void FormatearGrilla(DataGridView dgv)
        {
            for (int i = 0; i < dgv.ColumnCount; i++)
            {
                dgv.Columns[i].Visible = false;

                dgv.Columns[i].HeaderCell.Style.Alignment 
                    = DataGridViewContentAlignment.MiddleCenter;
            }
        }
    }
}

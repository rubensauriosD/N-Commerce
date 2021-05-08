namespace PresentacionBase.Formularios
{
    using Aplicacion.Constantes;
    using System;
    using System.Windows.Forms;

    public partial class FormAbm : FormBase
    {
        public long? EntidadId { get; protected set; }
        protected TipoOperacion TipoOperacion;

        private bool _realizoAlgunaOperacion;
        public bool RealizoAlgunaOperacion => _realizoAlgunaOperacion;

        public FormAbm()
        {
            InitializeComponent();

            _realizoAlgunaOperacion = false;
        }

        public FormAbm(TipoOperacion tipoOperacion, long? entidadId = null)
            : this()
        {
            TipoOperacion = tipoOperacion;
            EntidadId = entidadId;
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarControles(this);
            EjecutarPostLimpieza();
        }

        private void btnEjecutar_Click(object sender, EventArgs e)
        {
            switch (TipoOperacion)
            {
                case TipoOperacion.Nuevo:
                    if (!VerificarDatosObligatorios())
                    {
                        MessageBox.Show("Por favor ingrese los datos Obligatorios");
                        break;
                    }

                    if (VerificarSiExiste())
                    {
                        MessageBox.Show("Los datos ingresados ya Existen");
                        break;
                    }

                    try
                    {
                        EjecutarComandoNuevo(); // Grabar
                        MessageBox.Show("Los datos se grabaron Correctamente");
                        _realizoAlgunaOperacion = true;
                        Close();
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show($"{exception.Message}", "Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                    break;

                case TipoOperacion.Modificar:
                    if (!VerificarDatosObligatorios())
                    {
                        MessageBox.Show("Por favor ingrese los datos Obligatorios");
                        break;
                    }

                    if (VerificarSiExiste(EntidadId))
                    {
                        MessageBox.Show("Los datos ingresados ya Existen");
                        break;
                    }

                    try
                    {
                        EjecutarComandoModificar(); // Modificar
                        MessageBox.Show("Los datos se Modificaron Correctamente");
                        _realizoAlgunaOperacion = true;
                        Close();
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show($"{exception.Message}", "Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                    break;

                case TipoOperacion.Eliminar:
                    try
                    {
                        EjecutarComandoEliminar();
                        MessageBox.Show("Los datos se Eliminaron Correctamente");
                        _realizoAlgunaOperacion = true;
                        Close();
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show($"{exception.Message}", "Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                    break;
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Esta seguro de salir", "Atencion", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)
                == DialogResult.OK)
                Close();
        }

        private void FormAbm_Load(object sender, EventArgs e)
        {
            switch (TipoOperacion)
            {
                case TipoOperacion.Nuevo:
                    btnEjecutar.Text = "&Guardar";
                    btnEjecutar.Image = Imagen.Guardar;
                    break;

                case TipoOperacion.Modificar:
                    btnEjecutar.Text = "&Modificar";
                    btnEjecutar.Image = Imagen.Editar;
                    break;

                case TipoOperacion.Eliminar:
                    btnEjecutar.Text = "&Eliminar";
                    btnEjecutar.Image = Imagen.Eliminar;
                    btnLimpiar.Enabled = false;
                    break;
            }

            if (EntidadId.HasValue)
                CargarDatos(EntidadId);

            if (TipoOperacion == TipoOperacion.Eliminar)
                DesactivarControles(this);
        }


        //
        // Metodos configurables
        //
        public virtual void EjecutarComandoNuevo()
        {
        }

        public virtual void EjecutarComandoEliminar()
        {
        }

        public virtual void EjecutarComandoModificar()
        {
        }

        public virtual bool VerificarSiExiste(long? id = null)
        {
            return false;
        }

        public virtual bool VerificarDatosObligatorios()
        {
            return false;
        }

        public virtual void CargarDatos(long? entidadId)
        {

        }

        public virtual void EjecutarPostLimpieza()
        {

        }

    }
}

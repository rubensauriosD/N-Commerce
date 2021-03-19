using Aplicacion.Constantes;
using IServicio.Persona.DTOs;
using IServicio.Usuario;
using IServicio.Usuario.DTOs;
using Presentacion.Core.Empleado;
using PresentacionBase.Formularios;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Presentacion.Core.Usuario
{
    public partial class _00011_Usuario : FormBase
    {
        private readonly IUsuarioServicio _servicio;
        private long? entidadId;
        protected object EntidadSeleccionada;

        public _00011_Usuario(IUsuarioServicio servicio)
        {
            InitializeComponent();

            _servicio = servicio;
        }


        private void ActualizarDatos(string txt)

        {
            // Cargar datos
            dgvGrilla.DataSource = _servicio.Obtener(txt);

            if (dgvGrilla.Columns.Count < 1)
                dgvGrilla.DataSource = new List<UsuarioDto>();

            // Formato Grilla
            for (int i = 0; i < dgvGrilla.Columns.Count; i++)
                dgvGrilla.Columns[i].Visible = false;

            dgvGrilla.Columns["ApyNomEmpleado"].Visible = true;
            dgvGrilla.Columns["ApyNomEmpleado"].HeaderText = "Empleado";
            dgvGrilla.Columns["ApyNomEmpleado"].DisplayIndex = 1;
            dgvGrilla.Columns["ApyNomEmpleado"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgvGrilla.Columns["NombreUsuario"].Visible = true;
            dgvGrilla.Columns["NombreUsuario"].HeaderText = "Usuario";
            dgvGrilla.Columns["NombreUsuario"].DisplayIndex = 2;
            dgvGrilla.Columns["NombreUsuario"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            dgvGrilla.Columns["EstaBloqueadoStr"].Visible = true;
            dgvGrilla.Columns["EstaBloqueadoStr"].HeaderText = "Bloqueado";
            dgvGrilla.Columns["EstaBloqueadoStr"].DisplayIndex = 3;
            dgvGrilla.Columns["EstaBloqueadoStr"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }

        public virtual void dgvGrilla_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvGrilla.RowCount <= 0) return;

            entidadId = (long)dgvGrilla["Id", e.RowIndex].Value;

            // Obtener el Objeto completo seleccionado
            EntidadSeleccionada = dgvGrilla.Rows[e.RowIndex].DataBoundItem;
        }

        //
        // Acciones de Botones
        //
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            ActualizarDatos(txtBuscar.Text);
        }

        private void txtBuscar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true; // Quita Ruido molesto enter
                btnBuscar.PerformClick(); // Hago un Click por Codigo
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            ActualizarDatos(string.Empty);
            txtBuscar.Clear();
            txtBuscar.Focus();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            if (!entidadId.HasValue)
            {
                Mjs.Error("No se seleccionó ningún registro.");
                return;
            }

            long id = (long)entidadId;
            UsuarioDto usr = ((UsuarioDto)EntidadSeleccionada);

            if (usr.Id != 0)
            {
                Mjs.Alerta("El empleado ya tiene un usuario");
                return;
            }

            // Generar Usuarios
            _servicio.Crear(usr.EmpleadoId, usr.ApellidoEmpleado, usr.NombreEmpleado);
            Mjs.Info("Usuario generado.");

            // Actualizar grilla
            ActualizarDatos(string.Empty);
        }

        private void btnBloquear_Click(object sender, EventArgs e)
        {
            if (dgvGrilla.RowCount < 1 || !entidadId.HasValue)
            {
                Mjs.Alerta("No se selecciono ningún elemento.");
                return;
            }

            if (((UsuarioDto)EntidadSeleccionada).Id == 0)
            {
                Mjs.Alerta("El empleado no tiene un usuario.");
                return;
            }

            _servicio.Bloquear((long)entidadId);
            Mjs.Info("Opearción realizada.");
            ActualizarDatos(string.Empty);
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            if (dgvGrilla.RowCount < 1 || !entidadId.HasValue)
            {
                Mjs.Alerta("No se selecciono ningún elemento.");
                return;
            }

            if (((UsuarioDto)EntidadSeleccionada).Id == 0)
            {
                Mjs.Alerta("El empleado no tiene un usuario.");
                return;
            }

            _servicio.ResetPassword((long)entidadId);
            Mjs.Info("Password por defecto asiganda.");
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void _00011_Usuario_Load(object sender, EventArgs e)
        {
            // Asignar Imagen a Botones
            btnActualizar.Image = Imagen.Actualizar;
            btnNuevo.Image = Imagen.Nuevo;
            btnSalir.Image = Imagen.Salir;
            btnBloquear.Image = Imagen.Bloquear;
            btnResetPassword.Image = Imagen.Limpiar;
            btnActualizar.Image = Imagen.Actualizar;

            ActualizarDatos(string.Empty);
        }
    }
}

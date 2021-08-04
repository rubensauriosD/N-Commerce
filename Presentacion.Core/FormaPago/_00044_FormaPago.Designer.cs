namespace Presentacion.Core.FormaPago
{
    partial class _00044_FormaPago
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.nudPagaCon = new System.Windows.Forms.NumericUpDown();
            this.label24 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.btnSalir = new System.Windows.Forms.Button();
            this.btnConfirmar = new System.Windows.Forms.Button();
            this.label19 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tabPageCuentaCorriente = new System.Windows.Forms.TabPage();
            this.lblLimiteCuentaCorriente = new System.Windows.Forms.Label();
            this.lblSaldoCuentaCorriente = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.txtDni = new System.Windows.Forms.TextBox();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.txtApellido = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.nudMontoCtaCte = new System.Windows.Forms.NumericUpDown();
            this.label21 = new System.Windows.Forms.Label();
            this.btnBuscarCliente = new System.Windows.Forms.Button();
            this.lblDni = new System.Windows.Forms.Label();
            this.lblNombre = new System.Windows.Forms.Label();
            this.lblApellido = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tabPageCheque = new System.Windows.Forms.TabPage();
            this.label15 = new System.Windows.Forms.Label();
            this.dtpFechaVencimientoCheque = new System.Windows.Forms.DateTimePicker();
            this.btnNuevoBanco = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.cmbBanco = new System.Windows.Forms.ComboBox();
            this.txtNumeroCheque = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.nudMontoCheque = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tabPageTarjeta = new System.Windows.Forms.TabPage();
            this.txtCuponPago = new System.Windows.Forms.TextBox();
            this.txtNumeroTarjeta = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.nudCantidadCuotas = new System.Windows.Forms.NumericUpDown();
            this.label22 = new System.Windows.Forms.Label();
            this.btnNuevaTarjeta = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.cmbTarjeta = new System.Windows.Forms.ComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.nudMontoTarjeta = new System.Windows.Forms.NumericUpDown();
            this.label18 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tabControlFormaPago = new System.Windows.Forms.TabControl();
            this.lblTotalEfectivo = new System.Windows.Forms.Label();
            this.lblTotalTarjeta = new System.Windows.Forms.Label();
            this.lblTotalCheque = new System.Windows.Forms.Label();
            this.lblTotalCuentaCorriente = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblVuelto = new System.Windows.Forms.Label();
            this.lblTotalAPagar = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudPagaCon)).BeginInit();
            this.tabPageCuentaCorriente.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMontoCtaCte)).BeginInit();
            this.tabPageCheque.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMontoCheque)).BeginInit();
            this.tabPageTarjeta.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCantidadCuotas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMontoTarjeta)).BeginInit();
            this.tabControlFormaPago.SuspendLayout();
            this.SuspendLayout();
            // 
            // nudPagaCon
            // 
            this.nudPagaCon.DecimalPlaces = 2;
            this.nudPagaCon.Font = new System.Drawing.Font("Segoe UI Semibold", 12F);
            this.nudPagaCon.Location = new System.Drawing.Point(450, 301);
            this.nudPagaCon.Maximum = new decimal(new int[] {
            -727379968,
            232,
            0,
            0});
            this.nudPagaCon.Name = "nudPagaCon";
            this.nudPagaCon.Size = new System.Drawing.Size(187, 29);
            this.nudPagaCon.TabIndex = 44;
            this.nudPagaCon.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudPagaCon.ValueChanged += new System.EventHandler(this.nudPagaCon_ValueChanged);
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.Location = new System.Drawing.Point(345, 340);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(97, 21);
            this.label24.TabIndex = 43;
            this.label24.Text = "El Vuelto es:";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label25.Location = new System.Drawing.Point(355, 303);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(87, 21);
            this.label25.TabIndex = 42;
            this.label25.Text = "Abona con:";
            // 
            // btnSalir
            // 
            this.btnSalir.BackColor = System.Drawing.Color.Silver;
            this.btnSalir.FlatAppearance.BorderSize = 0;
            this.btnSalir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSalir.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSalir.Location = new System.Drawing.Point(505, 388);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(132, 36);
            this.btnSalir.TabIndex = 41;
            this.btnSalir.Text = "Cancelar";
            this.btnSalir.UseVisualStyleBackColor = false;
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            // 
            // btnConfirmar
            // 
            this.btnConfirmar.BackColor = System.Drawing.Color.SeaGreen;
            this.btnConfirmar.FlatAppearance.BorderSize = 0;
            this.btnConfirmar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfirmar.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfirmar.ForeColor = System.Drawing.Color.White;
            this.btnConfirmar.Location = new System.Drawing.Point(343, 388);
            this.btnConfirmar.Name = "btnConfirmar";
            this.btnConfirmar.Size = new System.Drawing.Size(132, 36);
            this.btnConfirmar.TabIndex = 39;
            this.btnConfirmar.Text = "Confirmar";
            this.btnConfirmar.UseVisualStyleBackColor = false;
            this.btnConfirmar.Click += new System.EventHandler(this.btnConfirmar_Click);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.ForeColor = System.Drawing.Color.Black;
            this.label19.Location = new System.Drawing.Point(340, 17);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(102, 21);
            this.label19.TabIndex = 37;
            this.label19.Text = "Total a Pagar";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(419, 242);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(80, 21);
            this.label10.TabIndex = 28;
            this.label10.Text = "A Cta. Cte.";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(436, 203);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(63, 21);
            this.label9.TabIndex = 27;
            this.label9.Text = "Cheque";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(444, 164);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(55, 21);
            this.label8.TabIndex = 26;
            this.label8.Text = "Tarjeta";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(435, 125);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(64, 21);
            this.label7.TabIndex = 25;
            this.label7.Text = "Efectivo";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(340, 99);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(141, 21);
            this.label6.TabIndex = 24;
            this.label6.Text = "Resumen de Pago";
            // 
            // tabPageCuentaCorriente
            // 
            this.tabPageCuentaCorriente.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPageCuentaCorriente.Controls.Add(this.lblLimiteCuentaCorriente);
            this.tabPageCuentaCorriente.Controls.Add(this.lblSaldoCuentaCorriente);
            this.tabPageCuentaCorriente.Controls.Add(this.label5);
            this.tabPageCuentaCorriente.Controls.Add(this.panel3);
            this.tabPageCuentaCorriente.Controls.Add(this.txtDni);
            this.tabPageCuentaCorriente.Controls.Add(this.txtNombre);
            this.tabPageCuentaCorriente.Controls.Add(this.txtApellido);
            this.tabPageCuentaCorriente.Controls.Add(this.label20);
            this.tabPageCuentaCorriente.Controls.Add(this.nudMontoCtaCte);
            this.tabPageCuentaCorriente.Controls.Add(this.label21);
            this.tabPageCuentaCorriente.Controls.Add(this.btnBuscarCliente);
            this.tabPageCuentaCorriente.Controls.Add(this.lblDni);
            this.tabPageCuentaCorriente.Controls.Add(this.lblNombre);
            this.tabPageCuentaCorriente.Controls.Add(this.lblApellido);
            this.tabPageCuentaCorriente.Controls.Add(this.label4);
            this.tabPageCuentaCorriente.Location = new System.Drawing.Point(4, 4);
            this.tabPageCuentaCorriente.Name = "tabPageCuentaCorriente";
            this.tabPageCuentaCorriente.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCuentaCorriente.Size = new System.Drawing.Size(299, 424);
            this.tabPageCuentaCorriente.TabIndex = 3;
            this.tabPageCuentaCorriente.Text = "CTA CTE";
            // 
            // lblLimiteCuentaCorriente
            // 
            this.lblLimiteCuentaCorriente.BackColor = System.Drawing.Color.Transparent;
            this.lblLimiteCuentaCorriente.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLimiteCuentaCorriente.Location = new System.Drawing.Point(154, 371);
            this.lblLimiteCuentaCorriente.Name = "lblLimiteCuentaCorriente";
            this.lblLimiteCuentaCorriente.Size = new System.Drawing.Size(137, 31);
            this.lblLimiteCuentaCorriente.TabIndex = 140;
            this.lblLimiteCuentaCorriente.Text = "$ 999 999 ,99";
            this.lblLimiteCuentaCorriente.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSaldoCuentaCorriente
            // 
            this.lblSaldoCuentaCorriente.BackColor = System.Drawing.Color.Transparent;
            this.lblSaldoCuentaCorriente.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSaldoCuentaCorriente.Location = new System.Drawing.Point(7, 371);
            this.lblSaldoCuentaCorriente.Name = "lblSaldoCuentaCorriente";
            this.lblSaldoCuentaCorriente.Size = new System.Drawing.Size(137, 31);
            this.lblSaldoCuentaCorriente.TabIndex = 139;
            this.lblSaldoCuentaCorriente.Text = "$ 999 999 ,99";
            this.lblSaldoCuentaCorriente.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(200, 402);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 15);
            this.label5.TabIndex = 137;
            this.label5.Text = "Limite";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.LightGray;
            this.panel3.Location = new System.Drawing.Point(8, 214);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(283, 4);
            this.panel3.TabIndex = 135;
            // 
            // txtDni
            // 
            this.txtDni.Enabled = false;
            this.txtDni.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtDni.Location = new System.Drawing.Point(8, 175);
            this.txtDni.Margin = new System.Windows.Forms.Padding(2);
            this.txtDni.MaxLength = 8;
            this.txtDni.Name = "txtDni";
            this.txtDni.Size = new System.Drawing.Size(136, 23);
            this.txtDni.TabIndex = 117;
            // 
            // txtNombre
            // 
            this.txtNombre.Enabled = false;
            this.txtNombre.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtNombre.Location = new System.Drawing.Point(8, 135);
            this.txtNombre.Margin = new System.Windows.Forms.Padding(2);
            this.txtNombre.MaxLength = 50;
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(283, 23);
            this.txtNombre.TabIndex = 115;
            // 
            // txtApellido
            // 
            this.txtApellido.Enabled = false;
            this.txtApellido.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtApellido.Location = new System.Drawing.Point(8, 94);
            this.txtApellido.Margin = new System.Windows.Forms.Padding(2);
            this.txtApellido.MaxLength = 50;
            this.txtApellido.Name = "txtApellido";
            this.txtApellido.Size = new System.Drawing.Size(283, 23);
            this.txtApellido.TabIndex = 114;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.ForeColor = System.Drawing.Color.Black;
            this.label20.Location = new System.Drawing.Point(53, 402);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(36, 15);
            this.label20.TabIndex = 133;
            this.label20.Text = "Saldo";
            // 
            // nudMontoCtaCte
            // 
            this.nudMontoCtaCte.DecimalPlaces = 2;
            this.nudMontoCtaCte.Enabled = false;
            this.nudMontoCtaCte.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.nudMontoCtaCte.Location = new System.Drawing.Point(44, 250);
            this.nudMontoCtaCte.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.nudMontoCtaCte.Name = "nudMontoCtaCte";
            this.nudMontoCtaCte.Size = new System.Drawing.Size(228, 23);
            this.nudMontoCtaCte.TabIndex = 132;
            this.nudMontoCtaCte.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudMontoCtaCte.Leave += new System.EventHandler(this.nudMontoCtaCte_Leave);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.Location = new System.Drawing.Point(41, 232);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(43, 15);
            this.label21.TabIndex = 131;
            this.label21.Text = "Monto";
            // 
            // btnBuscarCliente
            // 
            this.btnBuscarCliente.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBuscarCliente.ForeColor = System.Drawing.Color.Black;
            this.btnBuscarCliente.Location = new System.Drawing.Point(8, 44);
            this.btnBuscarCliente.Name = "btnBuscarCliente";
            this.btnBuscarCliente.Size = new System.Drawing.Size(283, 30);
            this.btnBuscarCliente.TabIndex = 129;
            this.btnBuscarCliente.Text = "Buscar";
            this.btnBuscarCliente.UseVisualStyleBackColor = true;
            this.btnBuscarCliente.Click += new System.EventHandler(this.btnBuscarCliente_Click);
            // 
            // lblDni
            // 
            this.lblDni.AutoSize = true;
            this.lblDni.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDni.Location = new System.Drawing.Point(5, 160);
            this.lblDni.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDni.Name = "lblDni";
            this.lblDni.Size = new System.Drawing.Size(27, 15);
            this.lblDni.TabIndex = 122;
            this.lblDni.Text = "DNI";
            // 
            // lblNombre
            // 
            this.lblNombre.AutoSize = true;
            this.lblNombre.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNombre.Location = new System.Drawing.Point(5, 118);
            this.lblNombre.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(51, 15);
            this.lblNombre.TabIndex = 120;
            this.lblNombre.Text = "Nombre";
            // 
            // lblApellido
            // 
            this.lblApellido.AutoSize = true;
            this.lblApellido.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblApellido.Location = new System.Drawing.Point(6, 77);
            this.lblApellido.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblApellido.Name = "lblApellido";
            this.lblApellido.Size = new System.Drawing.Size(51, 15);
            this.lblApellido.TabIndex = 116;
            this.lblApellido.Text = "Apellido";
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(3, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(293, 38);
            this.label4.TabIndex = 1;
            this.label4.Text = "Cuenta Corriente";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabPageCheque
            // 
            this.tabPageCheque.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPageCheque.Controls.Add(this.label15);
            this.tabPageCheque.Controls.Add(this.dtpFechaVencimientoCheque);
            this.tabPageCheque.Controls.Add(this.btnNuevoBanco);
            this.tabPageCheque.Controls.Add(this.label14);
            this.tabPageCheque.Controls.Add(this.cmbBanco);
            this.tabPageCheque.Controls.Add(this.txtNumeroCheque);
            this.tabPageCheque.Controls.Add(this.label13);
            this.tabPageCheque.Controls.Add(this.nudMontoCheque);
            this.tabPageCheque.Controls.Add(this.label12);
            this.tabPageCheque.Controls.Add(this.label3);
            this.tabPageCheque.Location = new System.Drawing.Point(4, 4);
            this.tabPageCheque.Name = "tabPageCheque";
            this.tabPageCheque.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCheque.Size = new System.Drawing.Size(299, 424);
            this.tabPageCheque.TabIndex = 2;
            this.tabPageCheque.Text = "CHEQUE";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label15.Location = new System.Drawing.Point(6, 191);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(107, 15);
            this.label15.TabIndex = 12;
            this.label15.Text = "Fecha Vencimiento";
            // 
            // dtpFechaVencimientoCheque
            // 
            this.dtpFechaVencimientoCheque.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpFechaVencimientoCheque.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaVencimientoCheque.Location = new System.Drawing.Point(6, 209);
            this.dtpFechaVencimientoCheque.Name = "dtpFechaVencimientoCheque";
            this.dtpFechaVencimientoCheque.Size = new System.Drawing.Size(228, 23);
            this.dtpFechaVencimientoCheque.TabIndex = 7;
            // 
            // btnNuevoBanco
            // 
            this.btnNuevoBanco.AutoSize = true;
            this.btnNuevoBanco.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnNuevoBanco.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnNuevoBanco.Location = new System.Drawing.Point(240, 163);
            this.btnNuevoBanco.Name = "btnNuevoBanco";
            this.btnNuevoBanco.Size = new System.Drawing.Size(26, 25);
            this.btnNuevoBanco.TabIndex = 6;
            this.btnNuevoBanco.Text = "...";
            this.btnNuevoBanco.UseVisualStyleBackColor = true;
            this.btnNuevoBanco.Click += new System.EventHandler(this.btnNuevoBanco_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label14.Location = new System.Drawing.Point(6, 147);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(40, 15);
            this.label14.TabIndex = 4;
            this.label14.Text = "Banco";
            // 
            // cmbBanco
            // 
            this.cmbBanco.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBanco.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbBanco.FormattingEnabled = true;
            this.cmbBanco.Location = new System.Drawing.Point(6, 165);
            this.cmbBanco.Name = "cmbBanco";
            this.cmbBanco.Size = new System.Drawing.Size(228, 23);
            this.cmbBanco.TabIndex = 5;
            // 
            // txtNumeroCheque
            // 
            this.txtNumeroCheque.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtNumeroCheque.Location = new System.Drawing.Point(6, 121);
            this.txtNumeroCheque.MaxLength = 8;
            this.txtNumeroCheque.Name = "txtNumeroCheque";
            this.txtNumeroCheque.Size = new System.Drawing.Size(228, 23);
            this.txtNumeroCheque.TabIndex = 3;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label13.Location = new System.Drawing.Point(6, 103);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(90, 15);
            this.label13.TabIndex = 2;
            this.label13.Text = "Nro. de Cheque";
            // 
            // nudMontoCheque
            // 
            this.nudMontoCheque.DecimalPlaces = 2;
            this.nudMontoCheque.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.nudMontoCheque.Location = new System.Drawing.Point(6, 77);
            this.nudMontoCheque.Maximum = new decimal(new int[] {
            -727379968,
            232,
            0,
            0});
            this.nudMontoCheque.Name = "nudMontoCheque";
            this.nudMontoCheque.Size = new System.Drawing.Size(228, 23);
            this.nudMontoCheque.TabIndex = 1;
            this.nudMontoCheque.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudMontoCheque.ValueChanged += new System.EventHandler(this.nudMontoCheque_ValueChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label12.Location = new System.Drawing.Point(6, 59);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(43, 15);
            this.label12.TabIndex = 0;
            this.label12.Text = "Monto";
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Green;
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(3, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(293, 38);
            this.label3.TabIndex = 1;
            this.label3.Text = "Cheque";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabPageTarjeta
            // 
            this.tabPageTarjeta.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPageTarjeta.Controls.Add(this.txtCuponPago);
            this.tabPageTarjeta.Controls.Add(this.txtNumeroTarjeta);
            this.tabPageTarjeta.Controls.Add(this.label23);
            this.tabPageTarjeta.Controls.Add(this.nudCantidadCuotas);
            this.tabPageTarjeta.Controls.Add(this.label22);
            this.tabPageTarjeta.Controls.Add(this.btnNuevaTarjeta);
            this.tabPageTarjeta.Controls.Add(this.label16);
            this.tabPageTarjeta.Controls.Add(this.cmbTarjeta);
            this.tabPageTarjeta.Controls.Add(this.label17);
            this.tabPageTarjeta.Controls.Add(this.nudMontoTarjeta);
            this.tabPageTarjeta.Controls.Add(this.label18);
            this.tabPageTarjeta.Controls.Add(this.label2);
            this.tabPageTarjeta.Location = new System.Drawing.Point(4, 4);
            this.tabPageTarjeta.Name = "tabPageTarjeta";
            this.tabPageTarjeta.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTarjeta.Size = new System.Drawing.Size(299, 424);
            this.tabPageTarjeta.TabIndex = 1;
            this.tabPageTarjeta.Text = "TARJETA";
            // 
            // txtCuponPago
            // 
            this.txtCuponPago.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtCuponPago.Location = new System.Drawing.Point(7, 165);
            this.txtCuponPago.MaxLength = 8;
            this.txtCuponPago.Name = "txtCuponPago";
            this.txtCuponPago.Size = new System.Drawing.Size(228, 23);
            this.txtCuponPago.TabIndex = 5;
            // 
            // txtNumeroTarjeta
            // 
            this.txtNumeroTarjeta.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtNumeroTarjeta.Location = new System.Drawing.Point(7, 121);
            this.txtNumeroTarjeta.MaxLength = 16;
            this.txtNumeroTarjeta.Name = "txtNumeroTarjeta";
            this.txtNumeroTarjeta.Size = new System.Drawing.Size(228, 23);
            this.txtNumeroTarjeta.TabIndex = 3;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label23.Location = new System.Drawing.Point(7, 147);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(66, 15);
            this.label23.TabIndex = 4;
            this.label23.Text = "Cupo Pago";
            // 
            // nudCantidadCuotas
            // 
            this.nudCantidadCuotas.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.nudCantidadCuotas.Location = new System.Drawing.Point(7, 210);
            this.nudCantidadCuotas.Maximum = new decimal(new int[] {
            18,
            0,
            0,
            0});
            this.nudCantidadCuotas.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudCantidadCuotas.Name = "nudCantidadCuotas";
            this.nudCantidadCuotas.Size = new System.Drawing.Size(79, 23);
            this.nudCantidadCuotas.TabIndex = 7;
            this.nudCantidadCuotas.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudCantidadCuotas.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label22.Location = new System.Drawing.Point(7, 192);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(44, 15);
            this.label22.TabIndex = 6;
            this.label22.Text = "Cuotas";
            // 
            // btnNuevaTarjeta
            // 
            this.btnNuevaTarjeta.AutoSize = true;
            this.btnNuevaTarjeta.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnNuevaTarjeta.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnNuevaTarjeta.Location = new System.Drawing.Point(241, 252);
            this.btnNuevaTarjeta.Name = "btnNuevaTarjeta";
            this.btnNuevaTarjeta.Size = new System.Drawing.Size(26, 25);
            this.btnNuevaTarjeta.TabIndex = 10;
            this.btnNuevaTarjeta.Text = "...";
            this.btnNuevaTarjeta.UseVisualStyleBackColor = true;
            this.btnNuevaTarjeta.Click += new System.EventHandler(this.btnNuevaTarjeta_Click);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label16.Location = new System.Drawing.Point(7, 236);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(41, 15);
            this.label16.TabIndex = 8;
            this.label16.Text = "Tarjeta";
            // 
            // cmbTarjeta
            // 
            this.cmbTarjeta.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTarjeta.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbTarjeta.FormattingEnabled = true;
            this.cmbTarjeta.Location = new System.Drawing.Point(7, 254);
            this.cmbTarjeta.Name = "cmbTarjeta";
            this.cmbTarjeta.Size = new System.Drawing.Size(228, 23);
            this.cmbTarjeta.TabIndex = 9;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label17.Location = new System.Drawing.Point(7, 103);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(83, 15);
            this.label17.TabIndex = 2;
            this.label17.Text = "Nro. de Tarjeta";
            // 
            // nudMontoTarjeta
            // 
            this.nudMontoTarjeta.DecimalPlaces = 2;
            this.nudMontoTarjeta.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.nudMontoTarjeta.Location = new System.Drawing.Point(7, 77);
            this.nudMontoTarjeta.Maximum = new decimal(new int[] {
            -727379968,
            232,
            0,
            0});
            this.nudMontoTarjeta.Name = "nudMontoTarjeta";
            this.nudMontoTarjeta.Size = new System.Drawing.Size(228, 23);
            this.nudMontoTarjeta.TabIndex = 1;
            this.nudMontoTarjeta.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudMontoTarjeta.ValueChanged += new System.EventHandler(this.nudMontoTarjeta_ValueChanged);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label18.Location = new System.Drawing.Point(7, 59);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(43, 15);
            this.label18.TabIndex = 11;
            this.label18.Text = "Monto";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Olive;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(3, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(293, 38);
            this.label2.TabIndex = 0;
            this.label2.Text = "Tarjeta";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabControlFormaPago
            // 
            this.tabControlFormaPago.Alignment = System.Windows.Forms.TabAlignment.Right;
            this.tabControlFormaPago.Controls.Add(this.tabPageTarjeta);
            this.tabControlFormaPago.Controls.Add(this.tabPageCheque);
            this.tabControlFormaPago.Controls.Add(this.tabPageCuentaCorriente);
            this.tabControlFormaPago.Location = new System.Drawing.Point(1, 1);
            this.tabControlFormaPago.Multiline = true;
            this.tabControlFormaPago.Name = "tabControlFormaPago";
            this.tabControlFormaPago.SelectedIndex = 0;
            this.tabControlFormaPago.Size = new System.Drawing.Size(326, 432);
            this.tabControlFormaPago.TabIndex = 1;
            // 
            // lblTotalEfectivo
            // 
            this.lblTotalEfectivo.BackColor = System.Drawing.Color.White;
            this.lblTotalEfectivo.Font = new System.Drawing.Font("Segoe UI Semibold", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalEfectivo.Location = new System.Drawing.Point(505, 120);
            this.lblTotalEfectivo.Name = "lblTotalEfectivo";
            this.lblTotalEfectivo.Size = new System.Drawing.Size(132, 31);
            this.lblTotalEfectivo.TabIndex = 46;
            this.lblTotalEfectivo.Text = "$ 999 999 ,99";
            this.lblTotalEfectivo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTotalTarjeta
            // 
            this.lblTotalTarjeta.BackColor = System.Drawing.Color.White;
            this.lblTotalTarjeta.Font = new System.Drawing.Font("Segoe UI Semibold", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalTarjeta.Location = new System.Drawing.Point(505, 159);
            this.lblTotalTarjeta.Name = "lblTotalTarjeta";
            this.lblTotalTarjeta.Size = new System.Drawing.Size(132, 31);
            this.lblTotalTarjeta.TabIndex = 47;
            this.lblTotalTarjeta.Text = "$ 999 999 ,99";
            this.lblTotalTarjeta.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTotalCheque
            // 
            this.lblTotalCheque.BackColor = System.Drawing.Color.White;
            this.lblTotalCheque.Font = new System.Drawing.Font("Segoe UI Semibold", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalCheque.Location = new System.Drawing.Point(505, 198);
            this.lblTotalCheque.Name = "lblTotalCheque";
            this.lblTotalCheque.Size = new System.Drawing.Size(132, 31);
            this.lblTotalCheque.TabIndex = 48;
            this.lblTotalCheque.Text = "$ 999 999 ,99";
            this.lblTotalCheque.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTotalCuentaCorriente
            // 
            this.lblTotalCuentaCorriente.BackColor = System.Drawing.Color.White;
            this.lblTotalCuentaCorriente.Font = new System.Drawing.Font("Segoe UI Semibold", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalCuentaCorriente.Location = new System.Drawing.Point(505, 237);
            this.lblTotalCuentaCorriente.Name = "lblTotalCuentaCorriente";
            this.lblTotalCuentaCorriente.Size = new System.Drawing.Size(132, 31);
            this.lblTotalCuentaCorriente.TabIndex = 49;
            this.lblTotalCuentaCorriente.Text = "$ 999 999 ,99";
            this.lblTotalCuentaCorriente.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Silver;
            this.panel1.Location = new System.Drawing.Point(342, 285);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(297, 4);
            this.panel1.TabIndex = 50;
            // 
            // lblVuelto
            // 
            this.lblVuelto.BackColor = System.Drawing.Color.White;
            this.lblVuelto.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVuelto.Location = new System.Drawing.Point(450, 338);
            this.lblVuelto.Name = "lblVuelto";
            this.lblVuelto.Size = new System.Drawing.Size(187, 21);
            this.lblVuelto.TabIndex = 51;
            this.lblVuelto.Text = "$ 999 999 ,99";
            this.lblVuelto.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTotalAPagar
            // 
            this.lblTotalAPagar.BackColor = System.Drawing.Color.White;
            this.lblTotalAPagar.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalAPagar.Location = new System.Drawing.Point(345, 38);
            this.lblTotalAPagar.Name = "lblTotalAPagar";
            this.lblTotalAPagar.Size = new System.Drawing.Size(292, 48);
            this.lblTotalAPagar.TabIndex = 52;
            this.lblTotalAPagar.Text = "$ 999 999 ,99";
            this.lblTotalAPagar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _00044_FormaPago
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(645, 437);
            this.Controls.Add(this.lblTotalAPagar);
            this.Controls.Add(this.lblVuelto);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblTotalCuentaCorriente);
            this.Controls.Add(this.lblTotalCheque);
            this.Controls.Add(this.lblTotalTarjeta);
            this.Controls.Add(this.lblTotalEfectivo);
            this.Controls.Add(this.nudPagaCon);
            this.Controls.Add(this.label24);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.btnSalir);
            this.Controls.Add(this.btnConfirmar);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tabControlFormaPago);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximumSize = new System.Drawing.Size(661, 476);
            this.MinimumSize = new System.Drawing.Size(661, 476);
            this.Name = "_00044_FormaPago";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Forma de Pago";
            this.Load += new System.EventHandler(this._00044_FormaPago_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudPagaCon)).EndInit();
            this.tabPageCuentaCorriente.ResumeLayout(false);
            this.tabPageCuentaCorriente.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMontoCtaCte)).EndInit();
            this.tabPageCheque.ResumeLayout(false);
            this.tabPageCheque.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMontoCheque)).EndInit();
            this.tabPageTarjeta.ResumeLayout(false);
            this.tabPageTarjeta.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCantidadCuotas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMontoTarjeta)).EndInit();
            this.tabControlFormaPago.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.NumericUpDown nudPagaCon;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Button btnSalir;
        private System.Windows.Forms.Button btnConfirmar;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TabPage tabPageCuentaCorriente;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox txtDni;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.TextBox txtApellido;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.NumericUpDown nudMontoCtaCte;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Button btnBuscarCliente;
        private System.Windows.Forms.Label lblDni;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.Label lblApellido;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TabPage tabPageCheque;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.DateTimePicker dtpFechaVencimientoCheque;
        private System.Windows.Forms.Button btnNuevoBanco;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox cmbBanco;
        private System.Windows.Forms.TextBox txtNumeroCheque;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NumericUpDown nudMontoCheque;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabPage tabPageTarjeta;
        private System.Windows.Forms.TextBox txtCuponPago;
        private System.Windows.Forms.TextBox txtNumeroTarjeta;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.NumericUpDown nudCantidadCuotas;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Button btnNuevaTarjeta;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ComboBox cmbTarjeta;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.NumericUpDown nudMontoTarjeta;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabControl tabControlFormaPago;
        private System.Windows.Forms.Label lblTotalEfectivo;
        private System.Windows.Forms.Label lblTotalTarjeta;
        private System.Windows.Forms.Label lblTotalCheque;
        private System.Windows.Forms.Label lblTotalCuentaCorriente;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblVuelto;
        private System.Windows.Forms.Label lblTotalAPagar;
        private System.Windows.Forms.Label lblLimiteCuentaCorriente;
        private System.Windows.Forms.Label lblSaldoCuentaCorriente;
        private System.Windows.Forms.Label label5;
    }
}
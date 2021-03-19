using Aplicacion.Constantes;

namespace Presentacion.Core.Cliente
{
    partial class _00035_PagoCuentaCorriente
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_00035_PagoCuentaCorriente));
            this.pnlSeparador = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnPagar = new System.Windows.Forms.ToolStripButton();
            this.btnLimpiar = new System.Windows.Forms.ToolStripButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.nudMontoPago = new System.Windows.Forms.NumericUpDown();
            this.lblMontoAdeudado = new System.Windows.Forms.Label();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMontoPago)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlSeparador
            // 
            this.pnlSeparador.BackColor = System.Drawing.Color.RoyalBlue;
            this.pnlSeparador.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSeparador.Location = new System.Drawing.Point(0, 58);
            this.pnlSeparador.Name = "pnlSeparador";
            this.pnlSeparador.Size = new System.Drawing.Size(385, 3);
            this.pnlSeparador.TabIndex = 5;
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.White;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(30, 30);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnPagar,
            this.btnLimpiar});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(3);
            this.toolStrip1.Size = new System.Drawing.Size(385, 58);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnPagar
            // 
            this.btnPagar.ForeColor = System.Drawing.Color.Black;
            this.btnPagar.Image = ((System.Drawing.Image)(resources.GetObject("btnPagar.Image")));
            this.btnPagar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPagar.Name = "btnPagar";
            this.btnPagar.Size = new System.Drawing.Size(41, 49);
            this.btnPagar.Text = "Pagar";
            this.btnPagar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnPagar.Click += new System.EventHandler(this.btnPagar_Click);
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.ForeColor = System.Drawing.Color.Black;
            this.btnLimpiar.Image = ((System.Drawing.Image)(resources.GetObject("btnLimpiar.Image")));
            this.btnLimpiar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(51, 49);
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label2.Location = new System.Drawing.Point(240, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Monto a Pagar";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label1.Location = new System.Drawing.Point(41, 100);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Monto Adeudado";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // nudMontoPago
            // 
            this.nudMontoPago.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nudMontoPago.DecimalPlaces = 2;
            this.nudMontoPago.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudMontoPago.Location = new System.Drawing.Point(218, 118);
            this.nudMontoPago.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.nudMontoPago.Name = "nudMontoPago";
            this.nudMontoPago.Size = new System.Drawing.Size(133, 23);
            this.nudMontoPago.TabIndex = 4;
            this.nudMontoPago.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudMontoPago.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblMontoAdeudado
            // 
            this.lblMontoAdeudado.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMontoAdeudado.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblMontoAdeudado.Location = new System.Drawing.Point(12, 112);
            this.lblMontoAdeudado.Name = "lblMontoAdeudado";
            this.lblMontoAdeudado.Size = new System.Drawing.Size(162, 39);
            this.lblMontoAdeudado.TabIndex = 2;
            this.lblMontoAdeudado.Text = "$ 999 999 ,99";
            this.lblMontoAdeudado.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _00035_PagoCuentaCorriente
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(385, 208);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblMontoAdeudado);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nudMontoPago);
            this.Controls.Add(this.pnlSeparador);
            this.Controls.Add(this.toolStrip1);
            this.MaximumSize = new System.Drawing.Size(401, 247);
            this.MinimumSize = new System.Drawing.Size(401, 247);
            this.Name = "_00035_PagoCuentaCorriente";
            this.ShowIcon = false;
            this.Text = "Realizar Pago";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMontoPago)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlSeparador;
        private System.Windows.Forms.ToolStrip toolStrip1;
        protected System.Windows.Forms.ToolStripButton btnPagar;
        private System.Windows.Forms.ToolStripButton btnLimpiar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudMontoPago;
        private System.Windows.Forms.Label lblMontoAdeudado;
    }
}
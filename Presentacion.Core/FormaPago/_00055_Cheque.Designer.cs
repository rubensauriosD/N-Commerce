
namespace Presentacion.Core.Cliente
{
    partial class _00055_Cheque
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
            this.chkDepositados = new System.Windows.Forms.CheckBox();
            this.chkVencidos = new System.Windows.Forms.CheckBox();
            this.chkPorVencer = new System.Windows.Forms.CheckBox();
            // 
            // pnlBusqueda
            // 
            this.pnlBusqueda.Controls.Add(this.chkPorVencer);
            this.pnlBusqueda.Controls.Add(this.chkVencidos);
            this.pnlBusqueda.Controls.Add(this.chkDepositados);
            this.pnlBusqueda.Size = new System.Drawing.Size(648, 43);
            this.pnlBusqueda.Controls.SetChildIndex(this.chkDepositados, 0);
            this.pnlBusqueda.Controls.SetChildIndex(this.chkVencidos, 0);
            this.pnlBusqueda.Controls.SetChildIndex(this.chkPorVencer, 0);
            // 
            // chkDepositados
            // 
            this.chkDepositados.AutoSize = true;
            this.chkDepositados.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkDepositados.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkDepositados.Location = new System.Drawing.Point(11, 14);
            this.chkDepositados.Name = "chkDepositados";
            this.chkDepositados.Size = new System.Drawing.Size(89, 19);
            this.chkDepositados.TabIndex = 3;
            this.chkDepositados.Text = "Depositados";
            this.chkDepositados.UseVisualStyleBackColor = true;
            // 
            // chkVencidos
            // 
            this.chkVencidos.AutoSize = true;
            this.chkVencidos.Checked = true;
            this.chkVencidos.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkVencidos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkVencidos.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkVencidos.Location = new System.Drawing.Point(106, 14);
            this.chkVencidos.Name = "chkVencidos";
            this.chkVencidos.Size = new System.Drawing.Size(71, 19);
            this.chkVencidos.TabIndex = 4;
            this.chkVencidos.Text = "Vencidos";
            this.chkVencidos.UseVisualStyleBackColor = true;
            // 
            // chkPorVencer
            // 
            this.chkPorVencer.AutoSize = true;
            this.chkPorVencer.Checked = true;
            this.chkPorVencer.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPorVencer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkPorVencer.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkPorVencer.Location = new System.Drawing.Point(183, 14);
            this.chkPorVencer.Name = "chkPorVencer";
            this.chkPorVencer.Size = new System.Drawing.Size(80, 19);
            this.chkPorVencer.TabIndex = 5;
            this.chkPorVencer.Text = "Por Vencer";
            this.chkPorVencer.UseVisualStyleBackColor = true;
            // 
            // _00055_Cheque
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(648, 450);
            this.Name = "_00055_Cheque";
            this.Text = "Cheque";
            this.Load += new System.EventHandler(this._00055_Cheque_Load);
            this.pnlBusqueda.ResumeLayout(false);
            this.pnlBusqueda.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkPorVencer;
        private System.Windows.Forms.CheckBox chkVencidos;
        private System.Windows.Forms.CheckBox chkDepositados;
    }
}
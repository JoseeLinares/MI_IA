namespace Mi_IA
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            txtTema = new TextBox();
            btnBuscar = new Button();
            rtxContenidoGeneral = new RichTextBox();
            btnResumen = new Button();
            rtxResumen = new RichTextBox();
            btnGuardarWord = new Button();
            btnGenerarPowerPoint = new Button();
            SuspendLayout();
            // 
            // txtTema
            // 
            txtTema.Location = new Point(27, 25);
            txtTema.Name = "txtTema";
            txtTema.Size = new Size(500, 27);
            txtTema.TabIndex = 0;
            // 
            // btnBuscar
            // 
            btnBuscar.Location = new Point(550, 25);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(120, 29);
            btnBuscar.TabIndex = 1;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = true;
            btnBuscar.Click += btnBuscar_Click;
            // 
            // rtxContenidoGeneral
            // 
            rtxContenidoGeneral.Location = new Point(27, 74);
            rtxContenidoGeneral.Name = "rtxContenidoGeneral";
            rtxContenidoGeneral.ReadOnly = true;
            rtxContenidoGeneral.Size = new Size(643, 120);
            rtxContenidoGeneral.TabIndex = 2;
            rtxContenidoGeneral.Text = "";
            // 
            // btnResumen
            // 
            btnResumen.Location = new Point(27, 200);
            btnResumen.Name = "btnResumen";
            btnResumen.Size = new Size(120, 29);
            btnResumen.TabIndex = 3;
            btnResumen.Text = "Resumen IA";
            btnResumen.UseVisualStyleBackColor = true;
            btnResumen.Click += btnResumen_Click;
            // 
            // rtxResumen
            // 
            rtxResumen.Location = new Point(27, 235);
            rtxResumen.Name = "rtxResumen";
            rtxResumen.ReadOnly = true;
            rtxResumen.Size = new Size(643, 188);
            rtxResumen.TabIndex = 4;
            rtxResumen.Text = "";
            // 
            // btnGuardarWord
            // 
            btnGuardarWord.Location = new Point(27, 440);
            btnGuardarWord.Name = "btnGuardarWord";
            btnGuardarWord.Size = new Size(150, 35);
            btnGuardarWord.TabIndex = 6;
            btnGuardarWord.Text = "Guardar en Word";
            btnGuardarWord.UseVisualStyleBackColor = true;
            btnGuardarWord.Click += btnGuardarWord_Click;
            // 
            // btnGenerarPowerPoint
            // 
            btnGenerarPowerPoint.Location = new Point(200, 440);
            btnGenerarPowerPoint.Name = "btnGenerarPowerPoint";
            btnGenerarPowerPoint.Size = new Size(180, 35);
            btnGenerarPowerPoint.TabIndex = 7;
            btnGenerarPowerPoint.Text = "Guardar en PowerPoint";
            btnGenerarPowerPoint.UseVisualStyleBackColor = true;
            btnGenerarPowerPoint.Click += btnGenerarPowerPoint_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(700, 500);
            Controls.Add(txtTema);
            Controls.Add(btnBuscar);
            Controls.Add(rtxContenidoGeneral);
            Controls.Add(btnResumen);
            Controls.Add(rtxResumen);
            Controls.Add(btnGuardarWord);
            Controls.Add(btnGenerarPowerPoint);
            Name = "Form1";
            Text = "Investigador Académico";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox txtTema;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.RichTextBox rtxContenidoGeneral;
        private System.Windows.Forms.Button btnResumen;
        private System.Windows.Forms.RichTextBox rtxResumen;
        private System.Windows.Forms.Button btnGuardarWord;
        private System.Windows.Forms.Button btnGenerarPowerPoint;
    }
}
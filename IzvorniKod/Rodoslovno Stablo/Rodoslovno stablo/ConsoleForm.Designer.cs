namespace Rodoslovno_stablo
{
    partial class ConsoleForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConsoleForm));
            this.textBoxOutput = new System.Windows.Forms.TextBox();
            this.buttonExecute = new System.Windows.Forms.Button();
            this.buttonHelp = new System.Windows.Forms.Button();
            this.labelConsoleMarker = new System.Windows.Forms.Label();
            this.comboBoxInput = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // textBoxOutput
            // 
            this.textBoxOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxOutput.BackColor = System.Drawing.Color.Black;
            this.textBoxOutput.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textBoxOutput.ForeColor = System.Drawing.Color.Lime;
            this.textBoxOutput.Location = new System.Drawing.Point(13, 12);
            this.textBoxOutput.Multiline = true;
            this.textBoxOutput.Name = "textBoxOutput";
            this.textBoxOutput.ReadOnly = true;
            this.textBoxOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxOutput.Size = new System.Drawing.Size(845, 429);
            this.textBoxOutput.TabIndex = 0;
            this.textBoxOutput.Text = "Dobro došli u aplikaciju Krv nije voda, verzija 1.1\r\nKopirajt grupa born2code sa " +
    "FER-a, najjačeg fakulteta u Zagrebu i šire\r\n\r\n";
            // 
            // buttonExecute
            // 
            this.buttonExecute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExecute.Location = new System.Drawing.Point(780, 447);
            this.buttonExecute.Name = "buttonExecute";
            this.buttonExecute.Size = new System.Drawing.Size(78, 26);
            this.buttonExecute.TabIndex = 2;
            this.buttonExecute.Text = "Izvrši";
            this.buttonExecute.UseVisualStyleBackColor = true;
            this.buttonExecute.Click += new System.EventHandler(this.buttonExecute_Click);
            // 
            // buttonHelp
            // 
            this.buttonHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonHelp.Location = new System.Drawing.Point(754, 447);
            this.buttonHelp.Name = "buttonHelp";
            this.buttonHelp.Size = new System.Drawing.Size(20, 26);
            this.buttonHelp.TabIndex = 3;
            this.buttonHelp.Text = "?";
            this.buttonHelp.UseVisualStyleBackColor = true;
            this.buttonHelp.Click += new System.EventHandler(this.buttonHelp_Click);
            // 
            // labelConsoleMarker
            // 
            this.labelConsoleMarker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelConsoleMarker.AutoSize = true;
            this.labelConsoleMarker.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelConsoleMarker.Location = new System.Drawing.Point(13, 450);
            this.labelConsoleMarker.Name = "labelConsoleMarker";
            this.labelConsoleMarker.Size = new System.Drawing.Size(18, 19);
            this.labelConsoleMarker.TabIndex = 4;
            this.labelConsoleMarker.Text = ">";
            // 
            // comboBoxInput
            // 
            this.comboBoxInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxInput.Font = new System.Drawing.Font("Consolas", 12F);
            this.comboBoxInput.FormattingEnabled = true;
            this.comboBoxInput.Location = new System.Drawing.Point(37, 447);
            this.comboBoxInput.Name = "comboBoxInput";
            this.comboBoxInput.Size = new System.Drawing.Size(710, 27);
            this.comboBoxInput.TabIndex = 5;
            // 
            // ConsoleForm
            // 
            this.AcceptButton = this.buttonExecute;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(870, 485);
            this.Controls.Add(this.comboBoxInput);
            this.Controls.Add(this.labelConsoleMarker);
            this.Controls.Add(this.buttonHelp);
            this.Controls.Add(this.buttonExecute);
            this.Controls.Add(this.textBoxOutput);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ConsoleForm";
            this.Text = "Pristup konzoli";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ConsoleForm_FormClosing);
            this.Load += new System.EventHandler(this.ConsoleForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxOutput;
        private System.Windows.Forms.Button buttonExecute;
        private System.Windows.Forms.Button buttonHelp;
        private System.Windows.Forms.Label labelConsoleMarker;
        private System.Windows.Forms.ComboBox comboBoxInput;
    }
}
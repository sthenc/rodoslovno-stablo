namespace Rodoslovno_stablo
{
    partial class PersonControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelName = new System.Windows.Forms.Label();
            this.pictureBoxUser = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxUser)).BeginInit();
            this.SuspendLayout();
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelName.Location = new System.Drawing.Point(57, 19);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(102, 18);
            this.labelName.TabIndex = 1;
            this.labelName.Text = "Ime Prezime";
            this.labelName.Click += new System.EventHandler(this.labelName_Click);
            // 
            // pictureBoxUser
            // 
            this.pictureBoxUser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxUser.Image = global::Rodoslovno_stablo.Properties.Resources.largerperson;
            this.pictureBoxUser.Location = new System.Drawing.Point(4, 4);
            this.pictureBoxUser.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBoxUser.MaximumSize = new System.Drawing.Size(50, 50);
            this.pictureBoxUser.Name = "pictureBoxUser";
            this.pictureBoxUser.Size = new System.Drawing.Size(50, 50);
            this.pictureBoxUser.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxUser.TabIndex = 0;
            this.pictureBoxUser.TabStop = false;
            this.pictureBoxUser.Click += new System.EventHandler(this.pictureBoxUser_Click);
            // 
            // PersonControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.pictureBoxUser);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "PersonControl";
            this.Size = new System.Drawing.Size(207, 59);
            this.Click += new System.EventHandler(this.PersonControl_Click);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PersonControl_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PersonControl_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PersonControl_MouseUp);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxUser)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxUser;
        private System.Windows.Forms.Label labelName;
    }
}

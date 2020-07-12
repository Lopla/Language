namespace Lopla.Windows
{
    partial class LoplaForm
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
            this.lopla1 = new Lopla.Draw.Windows.Controls.LoplaControl();
            this.SuspendLayout();
            // 
            // lopla1
            // 
            this.lopla1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lopla1.Location = new System.Drawing.Point(0, 0);
            this.lopla1.Name = "lopla1";
            this.lopla1.Size = new System.Drawing.Size(320, 240);
            this.lopla1.TabIndex = 1;
            this.lopla1.OnLoplaDone += new Lopla.Draw.Windows.Controls.LoplaDoneHandler(this.Lopla1_OnLoplaDone);
            // 
            // LoplaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.ClientSize = new System.Drawing.Size(689, 337);
            this.Controls.Add(this.lopla1);
            this.Name = "LoplaForm";
            this.Text = "Lopla";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.LoplaForm_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion
        private Lopla.Draw.Windows.Controls.LoplaControl lopla1;
    }
}


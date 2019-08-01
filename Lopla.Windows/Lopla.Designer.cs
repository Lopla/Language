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
            this.lopla1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lopla1.Location = new System.Drawing.Point(38, 31);
            this.lopla1.Name = "lopla1";
            this.lopla1.Size = new System.Drawing.Size(1124, 376);
            this.lopla1.TabIndex = 1;
            this.lopla1.OnLoplaDone += new Lopla.Draw.Windows.Controls.LoplaDoneHandler(this.Lopla1_OnLoplaDone);
            // 
            // LoplaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.ClientSize = new System.Drawing.Size(1196, 444);
            this.Controls.Add(this.lopla1);
            this.Name = "LoplaForm";
            this.Text = "Lopla";
            this.ResumeLayout(false);

        }

        #endregion
        private Lopla.Draw.Windows.Controls.LoplaControl lopla1;
    }
}


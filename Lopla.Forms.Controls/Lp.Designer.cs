namespace Lopla.Forms.Controls
{
    partial class Lp
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
            this.skControl1 = new SkiaSharp.Views.Desktop.SKControl();
            this.SuspendLayout();
            // 
            // skControl1
            // 
            this.skControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skControl1.Location = new System.Drawing.Point(0, 0);
            this.skControl1.Name = "skControl1";
            this.skControl1.Size = new System.Drawing.Size(320, 240);
            this.skControl1.TabIndex = 0;
            this.skControl1.Text = "skControl1";
            // 
            // Lp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Controls.Add(this.skControl1);
            this.Name = "Lp";
            this.Size = new System.Drawing.Size(320, 240);
            this.ResumeLayout(false);

        }

        #endregion

        private SkiaSharp.Views.Desktop.SKControl skControl1;
    }
}

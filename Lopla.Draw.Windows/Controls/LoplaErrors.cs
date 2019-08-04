namespace Lopla.Draw.Windows.Controls
{
    using System;
    using System.Windows.Forms;

    public partial class LoplaErrors : Form
    {
        private readonly string _errors;

        public LoplaErrors(string errors)
        {
            _errors = errors;
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void LoplaErrors_Load(object sender, EventArgs e)
        {
            textBox1.Text = _errors;
        }
    }
}
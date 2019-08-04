using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lopla.Draw.Windows.Controls
{
    using Language.Interfaces;

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
            this.Close();
        }

        private void LoplaErrors_Load(object sender, EventArgs e)
        {
            this.textBox1.Text = _errors;
        }
    }
}

using Lopla.Starting;

namespace Lopla.Windows
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    public partial class LoplaForm : Form
    {
        private readonly string[] _args;

        public LoplaForm(string[] args)
        {
            _args = args;
            InitializeComponent();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Invoke((MethodInvoker) Close);
        }

        private void Lopla_Load(object sender, EventArgs e)
        {
            lopla1.Project(_args);

            lopla1.Run();
        }
    }
}
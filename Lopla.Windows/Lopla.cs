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

        public void Run()
        {
            lopla1.Project(_args);

            lopla1.Run();
        }

        private void Lopla1_OnLoplaDone(object sender, EventArgs args)
        {
            Application.Exit();
        }

        private void LoplaForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            lopla1.Stop();
        }
    }
}
using System;
using System.Windows.Forms;

namespace Lopla.Windows
{

    public partial class LoplaForm : Form
    {
        private readonly string[] _args;

        public LoplaForm(string[] args, bool parentConsoleAvailble)
        {
            _args = args;
            InitializeComponent();

            this.lopla1.ParentConsole = parentConsoleAvailble;
        }

        public void Run()
        {
            lopla1.Run(_args);
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
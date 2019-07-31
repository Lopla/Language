namespace Lopla.Windows
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    public partial class LoplaForm : Form
    {
        public LoplaForm()
        {
            InitializeComponent();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Invoke((MethodInvoker) Close);
        }

        private void Lopla_Load(object sender, EventArgs e)
        {
            //// load project and set it up
            /// start

            var script =
                //Code.Events;
                Code.Perf + Code.Anim;

            lopla1.Project(script);

            lopla1.Run();
        }
    }
}
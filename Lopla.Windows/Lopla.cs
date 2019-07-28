namespace Lopla.Windows
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows.Forms;
    using Draw.Libs;
    using Draw.SkiaLayer;
    using Language.Interfaces;
    using Language.Processing;
    using Language.Providers;
    using Libs;
    using WinLopla;

    public partial class Lopla : Form
    {
        private SkiaDrawLoplaEngine _engine;
        private LoplaGuiEventProcessor _uiEventsProvider;

        public Lopla()
        {
            InitializeComponent();

            this.SetupLopla();
        }

        private void SetupLopla()
        {
            var drawCtx = new LoplaRequests(skControl);

            _engine = new SkiaDrawLoplaEngine(drawCtx);
            _uiEventsProvider = new LoplaGuiEventProcessor(_engine);

            var windowsDesktopEvents =
                new WindowsDesktopEvents(skControl, _uiEventsProvider);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            var script =
                Code.Events;
                //CodeClass.Perf + CodeClass.Anim;

            var p = new Runner();

            var result = p.Run(new MemoryScripts("Test", new List<ILibrary>
                {
                    new Draw(_engine, _uiEventsProvider.UiEvents),
                    new Lp(),
                    new IO()
                }, script
            ));

            if (result.HasErrors)
                MessageBox.Show(
                    result.ToString(), "Lopla", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

            Invoke((MethodInvoker) Close);
        }

        private void Lopla_Load(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }
    }
}
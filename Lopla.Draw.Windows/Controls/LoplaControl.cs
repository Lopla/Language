namespace Lopla.Draw.Windows.Controls
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows.Forms;
    using Language.Interfaces;
    using Language.Processing;
    using Language.Providers;
    using Libs;
    using Logic;
    using Lopla.Libs;
    using SkiaLayer;

    public partial class LoplaControl : UserControl
    {
        private SkiaDrawLoplaEngine _engine;
        private LoplaGuiEventProcessor _uiEventsProvider;
        private MemoryScripts project;

        public LoplaControl()
        {
            InitializeComponent();
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                SetupLopla();
            }
        }

        private void SetupLopla()
        {
            var drawCtx = new LoplaRequests(skControl1);

            _engine = new SkiaDrawLoplaEngine(drawCtx);
            _uiEventsProvider = new LoplaGuiEventProcessor(_engine);

            var windowsDesktopEvents =
                new WindowsDesktopEvents(skControl1, _uiEventsProvider);
        }

        public void Run()
        {
            this.backgroundWorker1.RunWorkerAsync();
        }

        public void Project(string code)
        {
            this.project = new MemoryScripts("Test", new List<ILibrary>
                {
                    new Draw(_engine, _uiEventsProvider.UiEvents),
                    new Lp(),
                    new IO()
                }, code
            );
        }

        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            var p = new Runner();

            var result = p.Run(project);

            if (result.HasErrors)
                MessageBox.Show(
                    result.ToString(), "Lopla", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
        }
    }
}
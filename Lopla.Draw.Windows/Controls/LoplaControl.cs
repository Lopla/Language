using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Lopla.Draw.SkiaLayer;
using Lopla.Draw.Windows.Logic;
using Lopla.Language.Interfaces;
using Lopla.Language.Processing;
using Lopla.Libs;
using Lopla.Starting;

namespace Lopla.Draw.Windows.Controls
{
    using System;

    public delegate void LoplaDoneHandler(object sender, System.EventArgs args);

    public partial class LoplaControl : UserControl
    {
        private SkiaDrawLoplaEngine _engine;
        private IProject _project;
        private LoplaGuiEventProcessor _uiEventsProvider;

        public event LoplaDoneHandler OnLoplaDone;

        public LoplaControl()
        {
            InitializeComponent();
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime) SetupLopla();
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
            backgroundWorker1.RunWorkerAsync();
        }

        public void Project(string[] code)
        {
            _project = new MainHandler(new List<ILibrary>
            {
                new WinFormsDraw(this.ParentForm, _engine, _uiEventsProvider.UiEvents),
                new Lp(),
                new IO()
            }).GetProject(code);
        }

        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            var p = new Runner();
            if (_project != null)
            {
                var result = p.Run(_project);

                if (this?.ParentForm?.Visible == true)
                {
                    if (result.HasErrors)
                    {
                        new LoplaErrors(result.ToString()).Show();
                    }
                }
                else
                {
                    Console.WriteLine(result.ToString());
                }
            }
        }

        

        private void BackgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.OnLoplaDone?.Invoke(sender, e);
        }
    }
}
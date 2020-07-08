using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Lopla.Draw.SkiaLayer;
using Lopla.Draw.Windows.Logic;
using Lopla.Language.Interfaces;
using Lopla.Language.Processing;
using Lopla.Libs;
using Lopla.Libs.Interfaces;
using Lopla.Starting;
using LoplaGuiEventProcessor = Lopla.Draw.SkiaLayer.LoplaGuiEventProcessor;

namespace Lopla.Draw.Windows.Controls
{
    public delegate void LoplaDoneHandler(object sender, EventArgs args);

    public partial class LoplaControl : UserControl
    {
        private IProject _project;
        private LoplaGuiEventProcessor _uiEventsProvider;
        private Thread _loplaThread;
        public bool ParentConsole = false;
        public ISkiaDrawLoplaEngine Engine { get; set; }
        private Runner _runner;

        public LoplaControl()
        {
            InitializeComponent();
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime) SetupLopla();
        }

        public event LoplaDoneHandler OnLoplaDone;

        private void SetupLopla()
        {
            var drawCtx = new LoplaRequests(skControl1);

            Engine =  new SkiaDrawLoplaEngine(drawCtx);
            _uiEventsProvider = new LoplaGuiEventProcessor(Engine);

            var windowsDesktopEvents =
                new WindowsDesktopEvents(skControl1, _uiEventsProvider);
        }

        public void Run()
        {
            _loplaThread = new Thread(StartWorker);
            _loplaThread.Start();
        }

        private void StartWorker()
        {
            this._runner = new Runner();
            if (_project != null)
            {
                var result = _runner.Run(_project);

                if (this?.ParentForm?.Visible == true && this.ParentConsole == false)
                {
                    if (result.HasErrors) new LoplaErrors(result.ToString()).ShowDialog();
                }
                else
                {
                    Console.WriteLine(result.ToString());
                }
            }


            OnLoplaDone?.Invoke(this, new EventArgs());
        }

        public void Project(string[] code)
        {
            _project = new MainHandler(new List<ILibrary>
            {
                new WinFormsDraw(ParentForm, Engine, _uiEventsProvider.UiEvents),
                new Lp(code?.ToList().Skip(1).ToArray()),
                new IO()
            }).GetProject(code);
        }

        public void Project(IProject project)
        {
            this._project = project;
        }

        public ISender UiEvents => this._uiEventsProvider.UiEvents;

        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
        }


        private void BackgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        private void LoplaControl_Load(object sender, EventArgs e)
        {
        }

        public void Stop()
        {
            try{
                _runner.Stop();
                _loplaThread.Abort();
                _uiEventsProvider.Stop();
            }catch
            {

            }
            OnLoplaDone?.Invoke(this, new EventArgs());
        }
    }
}
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
using Lopla.Starting;

namespace Lopla.Draw.Windows.Controls
{
    public delegate void LoplaDoneHandler(object sender, EventArgs args);

    public partial class LoplaControl : UserControl
    {
        private Thread _loplaThread;
        private IProject _project;
        private Runner _runner;
        private LoplaGuiEventProcessor _uiEventsProvider;
        private WinFormsDraw _windowsDraw;
        public bool ParentConsole = false;

        public LoplaControl()
        {
            InitializeComponent();

            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime) SetupLopla();
        }

        private ISkiaDrawLoplaEngine Engine { get; set; }

        public event LoplaDoneHandler OnLoplaDone;

        private void SetupLopla()
        {
            var drawCtx = new LoplaRequests(skControl1);

            Engine = new SkiaDrawLoplaEngine(drawCtx);
            
            _uiEventsProvider = new LoplaGuiEventProcessor(Engine);
            
            var windowsDesktopEvents =
                new WindowsDesktopEvents(skControl1, _uiEventsProvider);

        }
        public void Run(string[] code)
        {
            _windowsDraw = new WinFormsDraw(ParentForm, Engine, _uiEventsProvider.UiEvents);

            _project = new MainHandler(new List<ILibrary>
            {
                _windowsDraw,
                new Lp(code?.ToList().Skip(1).ToArray()),
                new IO()
            }).GetProject(code);

            _loplaThread = new Thread(StartWorker);
            _loplaThread.Start();
        }

        private void StartWorker()
        {
            _runner = new Runner();
            if (_project != null)
            {
                var result = _runner.Run(_project);

                if (this?.ParentForm?.Visible == true && ParentConsole == false)
                {
                    if (result.HasErrors) new LoplaErrors(result.ToString()).ShowDialog();
                }
                else
                {
                    Console.WriteLine(result.ToString());
                }
                
                _windowsDraw.Dispose();
            }

            OnLoplaDone?.Invoke(this, new EventArgs());
        }

       
        private void LoplaControl_Load(object sender, EventArgs e)
        {
        }

        public void Stop()
        {
            try
            {
                _uiEventsProvider.Stop();
            }
            catch
            {
                // ignored
            }
            try
            {
                _loplaThread.Abort();
            }
            catch
            {
                // ignored
            }
            try
            {
                _runner.Stop();
            }
            catch
            {
                // ignored
            }

            OnLoplaDone?.Invoke(this, new EventArgs());
        }
    }
}
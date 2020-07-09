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

            Engine =  new SkiaD
            
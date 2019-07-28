﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Lopla.Draw;
using Lopla.Language.Interfaces;
using Lopla.Language.Processing;
using Lopla.Language.Providers;
using Lopla.Libs;
using Lopla.Libs.Messaging;

namespace Lopla.Windows
{
    using Draw.SkiaLayer;

    public partial class Lopla : Form
    {
        private readonly LockingBus _uiEvents;
        private readonly SkiaDrawLoplaEngine _engine;

        public Lopla()
        {
            InitializeComponent();

            _uiEvents = new LockingBus();

            var drawCtx = new LoplaRequests(skControl);

            _engine = new SkiaDrawLoplaEngine(drawCtx);

            var windowsDesktopEvents = 
                new WindowsDesktopEvents(skControl, 
                    new LoplaGuiEventProcessor(_uiEvents, _engine));
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            var script = CodeClass.Perf + CodeClass.Anim;

            var p = new Runner();

            var result = p.Run(new MemoryScripts("Test", new List<ILibrary>
                {
                    new Draw.Libs.Draw(_engine, _uiEvents),
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
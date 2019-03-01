using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Lopla.Draw;
using Lopla.Language.Interfaces;
using Lopla.Language.Processing;
using Lopla.Language.Providers;
using Lopla.Libs;
using Lopla.Libs.Messaging;
using SkiaSharp.Views.Desktop;

namespace Lopla.Windows
{
    public partial class Lopla : Form
    {
        private readonly LockingBus _uiEvents;
        private SkiaDrawLoplaEngine engine;

        public Lopla()
        {
            InitializeComponent();
            _uiEvents = new LockingBus();

            var userInterface = new UserInterfaceEventsWrapper(_uiEvents, skControl);

            var drawCtx = new WindowsDesktopDrawCTX(skControl);

            Script1 = new CodeClass();

            engine = new SkiaDrawLoplaEngine(drawCtx);
        }

        public CodeClass Script1 { get; }

        private void skControl_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var surface = e.Surface;
            var surfaceWidth = e.Info.Width;
            var surfaceHeight = e.Info.Height;

            var canvas = surface.Canvas;
            engine.Render(canvas);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            var p = new Runner();

            var result = p.Run(new MemoryScripts("Test", new List<ILibrary>
                {
                    new Draw.Libs.Draw(engine, _uiEvents),
                    new Lp(),
                    new IO()
                }, Script1.DrawLines
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
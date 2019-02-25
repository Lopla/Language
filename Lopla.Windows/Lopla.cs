namespace Lopla.Windows
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows.Forms;
    using Draw;
    using Draw.Libs;
    using Language.Interfaces;
    using Language.Processing;
    using Language.Providers;
    using Libs;
    using Libs.Messaging;
    using SkiaSharp.Views.Desktop;

    public partial class Lopla : Form
    {
        private readonly SkiaDrawLopla _loplaRenderer;
        private readonly WindowsDesktopDrawCTX _drawCtx;
        private readonly Bus _messaging;
        private readonly LockingBus _uiEvents;

        public Lopla()
        {
            InitializeComponent();
            _uiEvents = new LockingBus();

            var userInterface = new UserInterface(_uiEvents, skControl);

            _messaging = new Bus();
            _drawCtx = new WindowsDesktopDrawCTX(skControl);

            _loplaRenderer = new SkiaDrawLopla(_messaging, _drawCtx);
        }

        private void skControl_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var surface = e.Surface;
            var surfaceWidth = e.Info.Width;
            var surfaceHeight = e.Info.Height;

            var canvas = surface.Canvas;
            _loplaRenderer.Render(canvas);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            var p = new Runner();

            var result = p.Run(new MemoryScripts("Test", new List<ILibrary>
                {
                    new Draw(_messaging, _drawCtx, _uiEvents),
                    new Lp(),
                    new IO()
                },
                @"
            Draw.Clear(1,1,1)
            Draw.SetColor(255,255,255)
            Draw.Log(""HI"")
            Draw.Flush()
            
            Draw.
            while (1)
            {
                Draw.WaitForEvent()
            }
"
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
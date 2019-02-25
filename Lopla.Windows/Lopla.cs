namespace Lopla.Windows
{
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Draw;
    using Draw.Libs;
    using Language.Interfaces;
    using Language.Processing;
    using Language.Providers;
    using Libs;
    using Libs.Interfaces;
    using Libs.Messaging;

    public partial class Lopla : Form
    {
        private readonly SkiaDrawLopla _loplaRenderer;
        private WindowsDesktopDrawCTX _drawCtx;
        private LockingBus _uiEvents;
        private Bus _messaging;

        public Lopla()
        {
            InitializeComponent();
            this._uiEvents = new LockingBus();

            var userInterface = new UserInterface(_uiEvents, this.skControl);

            this._messaging = new Bus();
            this._drawCtx = new WindowsDesktopDrawCTX(this.skControl);

            _loplaRenderer = new SkiaDrawLopla(_messaging, _drawCtx);
        }

        private void skControl_PaintSurface(object sender, SkiaSharp.Views.Desktop.SKPaintSurfaceEventArgs e)
        {
            var surface = e.Surface;
            var surfaceWidth = e.Info.Width;
            var surfaceHeight = e.Info.Height;

            var canvas = surface.Canvas;
            _loplaRenderer.Render(canvas);
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            var p = new Runner();

            var result = p.Run(new MemoryScripts("Test", new List<ILibrary>()
                {
                    new Draw(_messaging, _drawCtx, _uiEvents),
                    new Lp(),
                    new IO(),
                },
                @"Draw.Clear(1,1,1)

Draw.SetColor(255,255,255)
Draw.Log(""HI"")
            Draw.Flush()

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

            Invoke((MethodInvoker)Close);
        }

        private void Lopla_Load(object sender, System.EventArgs e)
        {
            this.backgroundWorker1.RunWorkerAsync();
        }
    }
}
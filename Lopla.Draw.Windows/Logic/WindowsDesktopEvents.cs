namespace Lopla.Draw.Windows.Logic
{
    using System;
    using System.Windows.Forms;
    using SkiaLayer;
    using SkiaSharp.Views.Desktop;

    public class WindowsDesktopEvents
    {
        private readonly LoplaGuiEventProcessor _processor;

        public WindowsDesktopEvents(
            SKControl skiaControl,
            LoplaGuiEventProcessor processor)
        {
            _processor = processor;

            skiaControl.Click += C_Click;
            skiaControl.KeyUp += C_KeyUp;
            skiaControl.SizeChanged += C_SizeChanged;
            skiaControl.PaintSurface += C_PaintSurface;
        }

        private void C_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            _processor.Render(e.Surface.Canvas);
        }

        private void C_SizeChanged(object sender, EventArgs e)
        {
            if (sender is SKControl skc)
            {
                _processor.SizeChanged(skc.Width, skc.Height);
            }
        }

        private void C_KeyUp(object sender, KeyEventArgs e)
        {
            _processor.Keyboard(e.KeyValue);
        }

        private void C_Click(object sender, EventArgs e)
        {
            if (e is MouseEventArgs mea) _processor.Click(mea.X, mea.Y);
        }
    }
}
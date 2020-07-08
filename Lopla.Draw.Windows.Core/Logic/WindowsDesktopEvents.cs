namespace Lopla.Draw.Windows.Logic
{
    using System;
    using System.Windows.Forms;
    using SkiaLayer;
    using SkiaSharp.Views.Desktop;

    public class WindowsDesktopEvents
    {
        private readonly LoplaGuiEventProcessor _eventsConsumer;

        public WindowsDesktopEvents(
            SKControl skiaControl,
            LoplaGuiEventProcessor eventsConsumer)
        {
            _eventsConsumer = eventsConsumer;

            skiaControl.Click += C_Click;
            skiaControl.KeyUp += C_KeyUp;
            skiaControl.SizeChanged += C_SizeChanged;
            skiaControl.PaintSurface += C_PaintSurface;
        }

        private void C_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            _eventsConsumer.Render(e.Surface.Canvas);
        }

        private void C_SizeChanged(object sender, EventArgs e)
        {
            if(sender is Control c){
                var size = c?.FindForm()?.ClientSize;
                if(size!=null)
                {
                    _eventsConsumer.SizeChanged(size.Value.Width, size.Value.Height);
                }
            }

            // if (sender is SKControl skc)
            // {
            //     _eventsConsumer.SizeChanged(skc.Width, skc.Height);
            // }
        }

        private void C_KeyUp(object sender, KeyEventArgs e)
        {
            _eventsConsumer.Keyboard(e.KeyValue);
        }

        private void C_Click(object sender, EventArgs e)
        {
            if (e is MouseEventArgs mea) _eventsConsumer.Click(mea.X, mea.Y);
        }
    }
}
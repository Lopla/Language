using System;
using System.Windows.Forms;
using Lopla.Draw;
using Lopla.Draw.Messages;
using Lopla.Language.Binary;
using Lopla.Libs.Interfaces;
using SkiaSharp.Views.Desktop;

namespace Lopla.Windows
{
    public class WindowsDesktopEvents
    {
        private readonly ISender _sender;
        private readonly SkiaDrawLoplaEngine _engine;

        public WindowsDesktopEvents(ISender sender, 
            SKControl skiaControl,
            SkiaDrawLoplaEngine engine)
        {
            _sender = sender;
            _engine = engine;
            Setup(skiaControl);
        }

        private void Setup(SKControl c)
        {
            c.Click += C_Click;
            c.KeyUp += C_KeyUp;
            c.SizeChanged += C_SizeChanged;
            c.PaintSurface += C_PaintSurface;
        }

        private void C_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            _engine.Render(e.Surface.Canvas);
        }

        private void C_SizeChanged(object sender, EventArgs e)
        {
            if (sender is SKControl skc)
                _sender.Send(new SetCanvas
                {
                    Size = new Point
                    {
                        X = skc.Width,
                        Y = skc.Height
                    }
                });
        }

        private void C_KeyUp(object sender, KeyEventArgs e)
        {
            _sender.Send(new Key
            {
                Char = new Number(e.KeyValue)
            });
        }

        private void C_Click(object sender, EventArgs e)
        {
            if (e is MouseEventArgs mea)
                _sender.Send(new Click
                {
                    Pos = new Point
                    {
                        Y = mea.Y,
                        X = mea.X
                    }
                });
        }
    }
}
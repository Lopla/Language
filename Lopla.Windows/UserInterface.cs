namespace Lopla.Windows
{
    using System;
    using System.Windows.Forms;
    using Draw.Messages;
    using Language.Binary;
    using Libs.Interfaces;
    using SkiaSharp.Views.Desktop;

    public class UserInterface
    {
        private readonly ISender _sender;

        public UserInterface(ISender sender, SKControl skiaControl)
        {
            _sender = sender;
            this.Setup(skiaControl);
        }

        public void Setup(SKControl c)
        {
            c.Click += C_Click;
            c.KeyUp += C_KeyUp;
            c.SizeChanged += C_SizeChanged;
        }

        private void C_SizeChanged(object sender, EventArgs e)
        {
            if (sender is SKControl skc)
            {
                _sender.Send(new SetCanvas()
                {
                    Size = new Point()
                    {
                        X = skc.Width,
                        Y = skc.Height
                    }
                });
            }
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
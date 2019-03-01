using Lopla.Libs.Messaging;

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
        private readonly SKControl _skiaControl;

        public UserInterface(ISender sender, SKControl skiaControl, Bus messaging)
        {
            _sender = sender;
            _skiaControl = skiaControl;
            this.Setup(skiaControl);


            messaging.Subscribe<Flush>(Flush);
            messaging.Subscribe<SetCanvas>(SetCanvas);
        }

        void Flush(Flush flush)
        {
            _skiaControl.Invalidate();
        }

        private void SetCanvas(SetCanvas setCanvas)
        {
            _skiaControl.Invoke((MethodInvoker)delegate
            {
                int newSizeX = (int)setCanvas.Size.X;
                int newSizeY = (int)setCanvas.Size.Y;
            });
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
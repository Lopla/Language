﻿namespace Lopla.Draw.Windows.Logic
{
    using System.Windows.Forms;
    using Draw;
    using Messages;
    using SkiaSharp.Views.Desktop;

    public class LoplaRequests : ILoplaRequestsHandler
    {
        private readonly SKControl _skiaControl;

        public LoplaRequests(SKControl skiaControl)
        {
            _skiaControl = skiaControl;
        }

        public Point GetCanvasSize()
        {
            var size = new Point
            {
                X = (decimal) _skiaControl.Bounds.Width,
                Y = (decimal) _skiaControl.Bounds.Height
            };
            return size;
        }

        public void Invalidate()
        {
            _skiaControl.Invalidate();
        }

        public void SetCanvasSize(decimal sizeX, decimal sizeY)
        {
            //if (_skiaControl.InvokeRequired)
            //{
            //    _skiaControl.Invoke((MethodInvoker) (() => { _skiaControl.SetBounds(0, 0, (int) sizeX, (int) sizeY); }));
            //}
            //else
            //{
            //    _skiaControl.SetBounds(0, 0, (int)sizeX, (int)sizeY);
            //}
        }
    }
}
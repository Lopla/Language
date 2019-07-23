namespace Lopla.Windows
{
    using System;
    using System.IO;
    using Draw;
    using Draw.Messages;
    using SkiaSharp.Views.Desktop;

    // ReSharper disable once InconsistentNaming
    public class WindowsDesktopDrawCTX : IDrawContext
    {
        private readonly SKControl _skiaControl;

        public WindowsDesktopDrawCTX(SKControl skiaControl)
        {
            _skiaControl = skiaControl;
        }

        public Stream GetResourceStream(
            string folder, string file)
        {
            throw new NotImplementedException();
        }

        public Point GetCanvasSize()
        {
            return new Point
            {
                X = (decimal) _skiaControl.CanvasSize.Width,
                Y = (decimal) _skiaControl.CanvasSize.Height
            };
        }

        public Stream GetStream(string imgFile)
        {
            StreamReader sr = new StreamReader(imgFile);
            return sr.BaseStream;
        }

        public void Invalidate()
        {
            _skiaControl.Invalidate();
        }

        public void SetCanvasSize(int sizeX, int sizeY)
        {
            
        }
    }
}
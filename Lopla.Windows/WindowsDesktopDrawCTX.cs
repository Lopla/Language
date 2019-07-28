namespace Lopla.Windows
{
    using Draw;
    using Draw.Messages;
    using SkiaSharp;
    using SkiaSharp.Views.Desktop;

    // ReSharper disable once InconsistentNaming
    public class WindowsDesktopDrawCTX : IDrawContext
    {
        private readonly SKControl _skiaControl;

        public WindowsDesktopDrawCTX(SKControl skiaControl)
        {
            _skiaControl = skiaControl;
        }

        public Point GetCanvasSize()
        {
            return new Point
            {
                X = (decimal) _skiaControl.CanvasSize.Width,
                Y = (decimal) _skiaControl.CanvasSize.Height
            };
        }

        public void Invalidate()
        {
            _skiaControl.Invalidate();
        }

        public void SetCanvasSize(int sizeX, int sizeY)
        {
            _skiaControl.SetBounds(0,0,sizeX, sizeY);

        }
    }
}
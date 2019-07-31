namespace Lopla.Draw.Windows.Logic
{
    using Draw;
    using Messages;
    using SkiaSharp.Views.Desktop;

    public class LoplaRequests : ILoplaRequests
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
                X = (decimal) _skiaControl.CanvasSize.Width,
                Y = (decimal) _skiaControl.CanvasSize.Height
            };
            return size;
        }

        public void Invalidate()
        {
            _skiaControl.Invalidate();
        }

        public void SetCanvasSize(decimal sizeX, decimal sizeY)
        {
            _skiaControl.SetBounds(0, 0, (int)sizeX, (int)sizeY);
        }
    }
}
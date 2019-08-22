namespace Lopla.Draw.SkiaLayer
{
    using System;
    using Lopla.Libs.Interfaces;
    using SkiaSharp;

    public class SkiaDrawLoplaEngine : IDisposable
    {
        private readonly SkiaRenderer _renderer;
        private SKBitmap _bitMap;
        private SKCanvas _canvas = null;

        public SkiaDrawLoplaEngine(ILoplaRequestsHandler loplaResRequestsHandler)
        {
            LoplaRequestsHandler = loplaResRequestsHandler;
            _renderer = new SkiaRenderer(loplaResRequestsHandler);

            SetupCanvas(32, 32);
        }

        public ILoplaRequestsHandler LoplaRequestsHandler { get; }

        public void Dispose()
        {
            lock (_canvas)
            {
                _bitMap?.Dispose();
                _canvas?.Dispose();
            }
        }

        public void SetupCanvas(int x, int y)
        {
            var newBitMap = new SKBitmap(x, y, SKColorType.Argb4444, SKAlphaType.Premul);

            if (_bitMap != null)
            {
                using (var c = new SKCanvas(newBitMap))
                {
                    c.Clear();
                    c.DrawBitmap(
                        _bitMap, new SKRect(0, 0, 
                        _bitMap.Width, _bitMap.Height));
                }
            }

            _bitMap?.Dispose();
            _canvas?.Dispose();

            _bitMap = newBitMap;
            _canvas = new SKCanvas(_bitMap);
            _canvas.Clear(SKColor.Empty);
        }

        public void Send(ILoplaMessage instruction)
        {
            _renderer.LoplaPainter(_canvas, instruction);
        }

        public void Render(SKCanvas canvas)
        {
            canvas?.DrawBitmap(_bitMap, 0, 0);
        }
    }
}
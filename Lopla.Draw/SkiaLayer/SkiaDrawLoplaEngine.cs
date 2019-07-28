namespace Lopla.Draw.SkiaLayer
{
    using System;
    using Lopla.Libs.Interfaces;
    using SkiaSharp;

    public class SkiaDrawLoplaEngine : IDisposable
    {
        private readonly SKBitmap _bitMap;
        private readonly SKCanvas _canvas;
        private readonly SkiaRenderer _renderer;

        public SkiaDrawLoplaEngine(ILoplaRequests loplaResRequestsHandler)
        {
            LoplaRequestsHandler = loplaResRequestsHandler;
            _renderer = new SkiaRenderer(loplaResRequestsHandler);

            _bitMap = new SKBitmap(512, 256, SKColorType.Argb4444, SKAlphaType.Opaque);
            _canvas = new SKCanvas(_bitMap);
        }

        public ILoplaRequests LoplaRequestsHandler { get; }

        public void Dispose()
        {
            _bitMap?.Dispose();
            _canvas?.Dispose();
        }

        public void Send(ILoplaMessage instruction)
        {
            _renderer.LoplaPainter(_canvas, instruction);
        }

        public void Render(SKCanvas canvas)
        {
            canvas.DrawBitmap(_bitMap, 0, 0);
        }
    }
}
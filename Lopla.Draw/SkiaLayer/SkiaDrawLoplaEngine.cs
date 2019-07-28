namespace Lopla.Draw.SkiaLayer
{
    using System;
    using Lopla.Libs.Interfaces;
    using Messages;
    using SkiaSharp;

    public class SkiaDrawLoplaEngine : IDisposable
    {
        public ILoplaRequests DrawContext { get; }
        private readonly SkiaRenderer _renderer;
        private readonly SKBitmap _bitMap;
        private readonly SKCanvas _canvas;

        public SkiaDrawLoplaEngine(ILoplaRequests drawContext)
        {
            DrawContext = drawContext;
            _renderer = new SkiaRenderer(drawContext);

            _bitMap = new SKBitmap(256,256, SKColorType.Argb4444, SKAlphaType.Opaque);
            _canvas = new SKCanvas(_bitMap);
        }

        public void Send(ILoplaMessage instruction)
        {
            if(instruction is Flush f)
            {
                DrawContext.Invalidate();
            }
            else if (instruction is SetCanvas sc)
            {
                DrawContext.SetCanvasSize((int)sc.Size.X, (int)sc.Size.Y);
            }
            else
            {
                _renderer.LoplaPainter(_canvas, instruction);
            }
        }

        public void Render(SKCanvas canvas)
        {
            canvas.DrawBitmap(_bitMap, 0, 0);
        }

        public void Dispose()
        {
            _bitMap?.Dispose();
            _canvas?.Dispose();
        }
    }
}
using System;
using Lopla.Draw.Messages;
using Lopla.Libs.Interfaces;
using SkiaSharp;

namespace Lopla.Draw
{
    public class SkiaDrawLoplaEngine : IDisposable
    {
        public IDrawContext DrawContext { get; }
        private readonly SkiaRenderer _renderer;
        private readonly SKBitmap _bitMap;
        private readonly SKCanvas _canvas;

        public SkiaDrawLoplaEngine(IDrawContext drawContext)
        {
            DrawContext = drawContext;
            _renderer = new SkiaRenderer(drawContext);

            _bitMap = new SKBitmap(1024,1024,  SKImageInfo.PlatformColorType, SKAlphaType.Premul);
            _canvas=new SKCanvas(_bitMap);
        }

        public void Send(ILoplaMessage instruction)
        {
            if(instruction is Flush f)
            {
                DrawContext.Invalidate();
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
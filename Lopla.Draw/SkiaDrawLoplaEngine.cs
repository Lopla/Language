using Lopla.Draw.Messages;
using Lopla.Libs.Interfaces;
using SkiaSharp;

namespace Lopla.Draw
{
    public class SkiaDrawLoplaEngine
    {
        public IDrawContext DrawContext { get; }
        private readonly SKImageInfo _info;
        private readonly SkiaRenderer _renderer;
        private readonly SKSurface _surface;

        public SkiaDrawLoplaEngine(IDrawContext drawContext)
        {
            DrawContext = drawContext;
            _renderer = new SkiaRenderer(drawContext);
            _info = new SKImageInfo(1024, 1024);
            _surface = SKSurface.Create(_info);
        }

        public void Send(ILoplaMessage instruction)
        {
            if(instruction is Flush f)
            {
                DrawContext.Invalidate();
            }
            else
            {
                _renderer.LoplaPainter(_surface.Canvas, instruction);
            }
        }

        public void Render(SKCanvas canvas)
        {
            canvas.DrawImage(_surface.Snapshot(), 0, 0, null);
        }
    }
}
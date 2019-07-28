namespace Lopla.Windows
{
    using Draw;
    using Draw.Messages;
    using Draw.SkiaLayer;
    using Language.Binary;
    using Libs.Interfaces;
    using SkiaSharp;

    public class LoplaGuiEventProcessor
    {
        private readonly SkiaDrawLoplaEngine _engine;
        private readonly ISender _sender;

        public LoplaGuiEventProcessor(ISender sender,
            SkiaDrawLoplaEngine engine)
        {
            _sender = sender;
            _engine = engine;
        }

        public void Click(int x, int y)
        {
            _sender.Send(new Click
            {
                Pos = new Point
                {
                    Y = y,
                    X = x
                }
            });
        }

        public void Keyboard(int eKeyValue)
        {
            _sender.Send(new Key
            {
                Char = new Number(eKeyValue)
            });
        }

        public void SizeChanged(int width, int height)
        {
            _sender.Send(new SetCanvas
            {
                Size = new Point
                {
                    X = width,
                    Y = height
                }
            });
        }

        public void Render(SKCanvas surfaceCanvas)
        {
            _engine.Render(surfaceCanvas);
        }
    }
}
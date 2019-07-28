namespace Lopla.Draw.SkiaLayer
{
    using Language.Binary;
    using Lopla.Libs.Interfaces;
    using Lopla.Libs.Messaging;
    using Messages;
    using SkiaSharp;

    public class LoplaGuiEventProcessor
    {
        private readonly SkiaDrawLoplaEngine _engine;
        
        public LoplaGuiEventProcessor(SkiaDrawLoplaEngine engine)
        {
            UiEvents = new LockingBus();
            _engine = engine;
        }

        public LockingBus UiEvents { get; set; }

        public void Click(int x, int y)
        {
            UiEvents.Send(new Click
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
            UiEvents.Send(new Key
            {
                Char = new Number(eKeyValue)
            });
        }

        public void SizeChanged(int width, int height)
        {
            UiEvents.Send(new SetCanvas
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
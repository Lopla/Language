﻿namespace Lopla.Draw.SkiaLayer
{
    using Language.Binary;
    using Lopla.Libs.Interfaces;
    using Lopla.Libs.Messages;
    using Lopla.Libs.Messaging;
    using Messages;
    using SkiaSharp;

    public class LoplaGuiEventProcessor
    {
        private readonly ISkiaDrawLoplaEngine _engine;

        public LoplaGuiEventProcessor(ISkiaDrawLoplaEngine engine)
        {
            UiEvents = new LockingBus();
            _engine = engine;
        }

        public ISender UiEvents { get; set; }

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

        public void Keyboard(int eKeyValue, bool keyDown)
        {
            UiEvents.Send(new Key
            {
                Char = new Number(eKeyValue),
                Down = keyDown
            });
        }

        public void SizeChanged(int width, int height)
        {
            _engine.SetupCanvas(width, height);
            
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

        public void Stop(){
            this.UiEvents.Send(new ShutDownQueue());
        }
    }
}
namespace Lopla.Draw
{
    using System.Collections.Generic;
    using System.Linq;
    using Lopla.Libs.Interfaces;
    using Messages;
    using SkiaSharp;

    public class SkiaQueuedRenderer
    {
        private readonly List<ILoplaMessage> _drawStack = new List<ILoplaMessage>();
        private readonly SkiaRenderer _skiaRenderer;

        public SkiaQueuedRenderer(ISubscribe subscriber, IDrawContext provider)
        {
            _skiaRenderer = new SkiaRenderer(provider);

            Reset();

            subscriber.Subscribe<Clear>(Queue);
            subscriber.Subscribe<Log>(Queue);
            subscriber.Subscribe<Line>(Queue);
            subscriber.Subscribe<SetColor>(Queue);
            subscriber.Subscribe<Box>(Queue);
            subscriber.Subscribe<Image>(Queue);
            subscriber.Subscribe<Sprite>(Queue);
            subscriber.Subscribe<Write>(Queue);
            subscriber.Subscribe<Text>(Queue);
        }

        private void Queue<TArgs>(TArgs data)
            where TArgs : ILoplaMessage
        {
            lock (_drawStack)
            {
                _drawStack.Add(data);
            }
        }

        public void Render(SKCanvas canvas)
        {
            lock (_drawStack)
            {
                if (_drawStack.Any())
                {
                    _skiaRenderer.TextReset();
                    _drawStack.ForEach(m => { _skiaRenderer.LoplaPainter(canvas, m); });
                }
            }
        }

        private void Reset()
        {
            lock (_drawStack)
            {
                _drawStack.Clear();
            }

            _skiaRenderer.Reset();
            _skiaRenderer.TextReset();
        }
    }
}
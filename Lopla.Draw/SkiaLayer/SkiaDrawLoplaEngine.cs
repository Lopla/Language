namespace Lopla.Draw.SkiaLayer
{
    using Lopla.Libs.Interfaces;
    using SkiaSharp;

    public class SkiaDrawLoplaEngine : ISkiaDrawLoplaEngine
    {
        private readonly SkiaRenderer _renderer;
        private SKBitmap _bitMap;

        public SkiaDrawLoplaEngine(ILoplaRequestsHandler loplaResRequestsHandler)
        {
            LoplaRequestsHandler = loplaResRequestsHandler;
            _renderer = new SkiaRenderer(loplaResRequestsHandler);

            SetupCanvas(1, 1);
        }

        public ILoplaRequestsHandler LoplaRequestsHandler { get; }

        public void Dispose()
        {
            _bitMap?.Dispose();
        }

        public void SetupCanvas(int x, int y)
        {
            if (
                _bitMap != null 
                && x == _bitMap.Width
                && y == _bitMap.Height
                )
            {
                return;
            }

            SKBitmap newBitMap;
            lock (this)
            {
                newBitMap = new SKBitmap(x, y, SKColorType.Argb4444, SKAlphaType.Premul);

                if (_bitMap != null)
                {
                    using (var c = new SKCanvas(newBitMap))
                    {
                        c.Clear(SKColor.Empty);
                        c.DrawBitmap(
                            _bitMap, new SKRect(0, 0,
                                _bitMap.Width, _bitMap.Height));
                    }
                }
                _bitMap?.Dispose();
                _bitMap = newBitMap;                
            }
        }

        public void Perform(ILoplaMessage instruction)
        {
            using (var canvas = new SKCanvas(_bitMap))
            {
                lock (this)
                {
                    _renderer.LoplaPainter(canvas, instruction);
                }
               
            }
        }

        public void Render(SKCanvas targetCanvas)
        {
                targetCanvas?.DrawBitmap(_bitMap, 0, 0);
            
        }
    }
}
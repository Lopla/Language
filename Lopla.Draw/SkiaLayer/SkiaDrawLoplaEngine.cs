namespace Lopla.Draw.SkiaLayer
{
    using Lopla.Libs.Interfaces;
    using SkiaSharp;

    public class SkiaDrawLoplaEngine : ISkiaDrawLoplaEngine
    {
        private readonly SkiaRenderer _renderer;
        private SKBitmap _bitMap;
        private SKCanvas _canvas = null;

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
            _canvas?.Dispose();
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
                _bitMap = null;
                _canvas?.Dispose();
                _canvas = null;

                _bitMap = newBitMap;
                
                _canvas = new SKCanvas(_bitMap);
 
            }
        }

        public void Perform(ILoplaMessage instruction)
        {
            lock (this)
            {
                _renderer.LoplaPainter(_canvas, instruction);
            }
        }

        public void Render(SKCanvas targetCanvas)
        {
            lock (this)
            {
                targetCanvas?.DrawBitmap(_bitMap, 0, 0);
                
                // canvas.DrawRect(_bitMap.Width - 100, 
                // _bitMap.Height - 96, 100, 96, new SKPaint(){
                //     Color = new SKColor(0,0,0)
                // });
                // canvas?.DrawText($"{_bitMap.Width} {_bitMap.Height}",
                //      _bitMap.Width - 100, _bitMap.Height-48,
                //     new SKPaint(){
                //     Color = new SKColor(255, 255, 255)
                // });
            }
        }
    }
}
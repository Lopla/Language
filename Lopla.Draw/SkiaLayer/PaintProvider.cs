namespace Lopla.Draw.SkiaLayer
{
    using System.Reflection;
    using SkiaSharp;

    public class PaintProvider
    {
        private static SKTypeface _fontFiraMono;

        public SKPaint GetPaintDevice(SKTypefaceStyle typeFaceStyle = SKTypefaceStyle.Normal)
        {
            var paintDevice = new SKPaint
            {
                TextSize = 16,

                /// if antialias is enabled then it's nicer
                IsAntialias = true,
                IsStroke = false,
                Typeface = GetFiraMono()
            };
            
            return paintDevice;
        }

        public static SKTypeface GetCourier(SKTypefaceStyle typeFaceStyle = SKTypefaceStyle.Normal)
        {
            return  SKTypeface.FromFamilyName("Courier New", typeFaceStyle);
        }

        public static SKTypeface GetFiraMono()
        {
            if (_fontFiraMono == null)
            {
                var assembly = Assembly.GetExecutingAssembly();
                var stream = assembly.GetManifestResourceStream("Lopla.Draw.Fonts.FiraMono-Regular.otf");
                if (stream == null)
                    return null;

                _fontFiraMono = SKTypeface.FromStream(stream, 0);
            }

            return _fontFiraMono;
        }
    }
}
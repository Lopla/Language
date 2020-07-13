namespace Lopla.Draw.SkiaLayer
{
    using System.Reflection;
    using SkiaSharp;

    public class PaintProvider
    {
        private static SKTypeface _fontFiraMono;

        public SKPaint GetPaintDevice()
        {
            var paintDevice = new SKPaint
            {
                TextSize = 16,

                /// if antialias is enabled then it's nicer
                IsAntialias = false,
                IsStroke = false,
                Typeface = GetFiraMono()
            };
            
            return paintDevice;
        }

        public static SKTypeface GetCourier()
        {
            return  SKTypeface.FromFamilyName("Courier New", SKFontStyleWeight.Normal, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright);
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
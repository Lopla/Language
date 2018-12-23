namespace Lopla.Draw
{
    using System.Reflection;
    using SkiaSharp;

    public class PaintProvider
    {
        private static SKTypeface _fontFiraMono;

        public SKPaint GetPaintDevice(
            SKTypefaceStyle typeFaceStyle = SKTypefaceStyle.Normal
        )
        {
            var paintDevice = new SKPaint
            {
                TextSize = 16,
                IsAntialias = true,
                IsStroke = false,
                Typeface = GetTypeface()
                //SKTypeface.FromFamilyName("Courier New", typeFaceStyle);
            };
            
            return paintDevice;
        }

        public static SKTypeface GetTypeface()
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
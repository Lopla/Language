using System.IO;
using System.Reflection;
using SkiaSharp;

namespace Lopla.Draw
{
    public class PaintProvider
    {
        private static SKTypeface _fontFiraMono = null;

        public SKPaint GetPaintDevice(
            SKTypefaceStyle typeFaceStyle = SKTypefaceStyle.Normal
        )
        {
            var paintDevice = new SKPaint();
            paintDevice.TextSize = 16;
            paintDevice.IsAntialias = true;
            paintDevice.IsStroke = false;
            paintDevice.Typeface = GetTypeface();
                //SKTypeface.FromFamilyName("Courier New", typeFaceStyle);

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
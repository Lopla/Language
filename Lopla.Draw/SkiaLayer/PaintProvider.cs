namespace Lopla.Draw.SkiaLayer
{
    using System.Reflection;
    using SkiaSharp;

    public class PaintProvider
    {
        private static SKTypeface _fontFiraMono;

        public SKTypeface TypeFace { get; private set; }

        public SKFont Font {
            get; protected set;
        }

        public int TextSize { get; protected set; } = 16;
        public SKTextAlign TextAlign { get; internal set; } = SKTextAlign.Left;

        public SKPaint GetPaintDevice()
        {
            TypeFace = GetFiraMono();
            Font = new SKFont(TypeFace, 16);
            TextAlign = SKTextAlign.Left;
            
            var paintDevice = new SKPaint();
            paintDevice.IsAntialias = true;
            paintDevice.IsStroke = false;
            
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
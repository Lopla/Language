using System;
using System.IO;
using Lopla.Draw.Messages;
using Lopla.Language.Environment;
using Lopla.Libs.Interfaces;
using SkiaSharp;

namespace Lopla.Draw
{
    public class SkiaRenderer
    {
        private readonly PaintProvider _paintProvider = new PaintProvider();
        private readonly IDrawContext _provider;
        private int _logTextPosition;
        private SKPaint _paintDevice;
        private int _writeTextPosition;

        public SkiaRenderer(IDrawContext provider)
        {
            _provider = provider;
            _paintDevice = _paintProvider.GetPaintDevice();
        }

        public void LoplaPainter(SKCanvas canvas, ILoplaMessage m)
        {
            switch (m)
            {
                case SetColor setColor:
                    _paintDevice.Color = new SKColor(setColor.Color.R, setColor.Color.G, setColor.Color.B, 255);
                    break;
                case Clear clear:
                    if (clear.Color != null)
                        canvas.Clear(new SKColor(clear.Color.R, clear.Color.G, clear.Color.B, 255));
                    else
                        canvas.Clear();
                    TextReset();
                    break;
                case Log log:
                    Log(canvas, log);
                    break;
                case Line line:
                    Line(canvas, line);
                    break;
                case Box box:
                    Box(canvas, box);
                    break;
                case Animation animation:
                    Animation(canvas, animation);
                    break;
                case Image img:
                    Image(canvas, img);
                    break;
                case Write text:
                    Write(canvas, text);
                    break;
                case Text text:
                    Text(canvas, text);
                    break;
            }
        }

        private void Text(SKCanvas canvas, Text text)
        {
            canvas.DrawText(text.Label, (float) text.Position.X, (float) text.Position.Y, _paintDevice);
        }

        public void TextReset()
        {
            _logTextPosition = 0;
            _writeTextPosition = 0;
        }

        private void Write(SKCanvas canvas, Write text)
        {
            var paint = _paintDevice.Clone();
            paint.TextAlign = (SKTextAlign) text.Align;
            _writeTextPosition += (int) paint.TextSize;

            var x = (float) text.Offset;
            if (text.Align == Aligmnent.Right)
                x = canvas.LocalClipBounds.Width;
            else if (text.Align == Aligmnent.Center)
                x = canvas.LocalClipBounds.Width / 2;
            canvas.DrawText(text.Text, x, _writeTextPosition, paint);
        }
        
        private void Animation(SKCanvas canvas, Animation img)
        {
            if (img.BinaryImage != null && img.BinaryImage.Length > 0)
            {
                using (SKStream stream = new SKMemoryStream(img.BinaryImage))
                using (SKCodec codec = SKCodec.Create(stream))
                {
                    int frameCount = codec.FrameCount;

                    int frame = 0;
                    SKImageInfo imageInfo = new SKImageInfo(
                        codec.Info.Width, 
                        codec.Info.Height,
                        codec.Info.ColorType,
                        codec.Info.AlphaType,
                        codec.Info.ColorSpace);

                    var bmp = new SKBitmap(imageInfo);
                    IntPtr pointer = bmp.GetPixels();

                    SKCodecOptions codecOptions = new SKCodecOptions(frame);

                    codec.GetPixels(imageInfo, pointer, codecOptions);

                    canvas.DrawBitmap(bmp, (float) img.Position.X, (float) img.Position.Y);
                }
            }
            else
            {
                throw new Exception("Bitmap not provided");
            }
        }

        private void Image(SKCanvas canvas, Image img)
        {
            var resourceBitmap = SKBitmap.Decode(img.BinaryImage);

            canvas.DrawBitmap(resourceBitmap, (float) img.Position.X, (float) img.Position.Y);
        }

        private void Box(SKCanvas canvas, Box line)
        {
            var region = new SKRegion();
            region.SetRect(new SKRectI((int) line.Start.X, (int) line.Start.Y, (int) line.End.X, (int) line.End.Y));
            canvas.DrawRegion(region, _paintDevice);
        }

        private void Line(SKCanvas canvas, Line line)
        {
            canvas.DrawLine(
                (float) line.Start.X, (float) line.Start.Y,
                (float) line.End.X, (float) line.End.Y,
                _paintDevice
            );
        }

        private void Log(SKCanvas canvas, Log log1)
        {
            _logTextPosition += (int) _paintDevice.TextSize;
            canvas.DrawText(log1.Text, 0, _logTextPosition, _paintDevice);
        }

        public void Reset()
        {
            _paintDevice = _paintProvider.GetPaintDevice();
        }
    }
}
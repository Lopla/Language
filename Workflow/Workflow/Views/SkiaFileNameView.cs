using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Skia;
using SkiaSharp;

namespace Workflow.Views;

public class SkiaFileNameView : Control
{
    public static readonly StyledProperty<string> FileNameProperty =
        AvaloniaProperty.Register<SkiaFileNameView, string>(nameof(FileName), "Examples/Game/Main.lpc");

    public string FileName
    {
        get => GetValue(FileNameProperty);
        set => SetValue(FileNameProperty, value);
    }

    protected override void OnPropertyChanged<T>(AvaloniaPropertyChangedEventArgs<T> change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == FileNameProperty)
        {
            InvalidateVisual();
        }
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);

        if (context is SkiaDrawingContext skiaContext)
        {
            var canvas = skiaContext.SkCanvas;
            canvas.Clear(SKColors.DarkSlateGray);

            using var paint = new SKPaint
            {
                Color = SKColors.White,
                TextSize = 24,
                IsAntialias = true,
                Typeface = SKTypeface.FromFamilyName("Segoe UI", SKFontStyle.Bold)
            };

            var text = $"Loaded file: {FileName}";
            canvas.DrawText(text, 10, 40, paint);
        }
        else
        {
            var text = $"Loaded file: {FileName}";
            var formattedText = new FormattedText
            {
                Text = text,
                Typeface = new Typeface("Segoe UI"),
                FontSize = 24,
                Constraint = new Size(Bounds.Width, Bounds.Height)
            };

            context.DrawText(Brushes.White, new Point(10, 10), formattedText);
        }
    }
}

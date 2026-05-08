using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Workflow.Views;

public partial class SkiaFileNameView : UserControl
{
    public static readonly StyledProperty<string> FileNameProperty =
        AvaloniaProperty.Register<SkiaFileNameView, string>(nameof(FileName), "Examples/Game/Main.lpc");

    public string FileName
    {
        get => GetValue(FileNameProperty);
        set => SetValue(FileNameProperty, value);
    }
}

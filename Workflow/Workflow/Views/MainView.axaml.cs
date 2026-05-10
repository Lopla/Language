using Avalonia.Controls;
using Avalonia.Input;
using Workflow.ViewModels;

namespace Workflow.Views;

public partial class MainView : UserControl
{
    private WorkflowBlockType? _draggingBlockType;

    public MainView()
    {
        InitializeComponent();
    }

    private void PaletteItem_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (sender is Control control && control.DataContext is WorkflowPaletteItemViewModel paletteItem)
        {
            _draggingBlockType = paletteItem.Type;
        }
    }

    private void WorkflowCanvas_PointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (_draggingBlockType.HasValue && DataContext is MainViewModel vm && sender is Control control)
        {
            var position = e.GetPosition(control);
            vm.AddBlock(_draggingBlockType.Value, position.X - 100, position.Y - 60);
        }

        _draggingBlockType = null;
    }
}

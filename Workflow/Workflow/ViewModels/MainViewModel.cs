using CommunityToolkit.Mvvm.ComponentModel;

namespace Workflow.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _greeting = "Welcome to Avalonia!";

    public string ExampleFileName { get; } = "Examples/Game/Main.lpc";
}

using CommunityToolkit.Mvvm.ComponentModel;

namespace Workflow.ViewModels;

public partial class WorkflowElementViewModel : ViewModelBase
{
    [ObservableProperty]
    private WorkflowBlockType _type;

    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty]
    private string _description = string.Empty;

    [ObservableProperty]
    private double _left;

    [ObservableProperty]
    private double _top;
}

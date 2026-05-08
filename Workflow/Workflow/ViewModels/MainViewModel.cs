using System;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using Lopla.Language.Compiler;

namespace Workflow.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _greeting = "Welcome to Avalonia!";

    [ObservableProperty]
    private string _parseStatus = string.Empty;

    [ObservableProperty]
    private string _parseTree = string.Empty;

    [ObservableProperty]
    private string _errorsText = string.Empty;

    public string ExampleFileName { get; } = "../Examples/Game/Main.lpc";

    public MainViewModel()
    {
        LoadExample();
    }

    private void LoadExample()
    {
        var examplePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", ExampleFileName.Replace('/', Path.DirectorySeparatorChar)));

        if (!File.Exists(examplePath))
        {
            ParseStatus = $"Example file not found: {examplePath}";
            return;
        }

        var content = File.ReadAllText(examplePath);
        var script = new Script { Name = ExampleFileName, Content = content };

        var languageService = new LanguageService();
        var result = languageService.ParseAndCompile(script);

        ParseStatus = result.HasErrors ? "Parse/compile failed" : "Parse and compile succeeded";
        ErrorsText = result.HasErrors ? result.ToString() : string.Empty;
        ParseTree = result.Root != null ? ToTreeText(result.Root) : "No parse tree available.";
    }

    private static string ToTreeText(ParseTreeNode node, string indent = "")
    {
        if (node == null)
            return string.Empty;

        var valuePart = string.IsNullOrWhiteSpace(node.Value) ? string.Empty : $": {node.Value}";
        var text = $"{indent}{node.Symbol}{valuePart}";

        foreach (var child in node.Children)
        {
            text += "\n" + ToTreeText(child, indent + "  ");
        }

        return text;
    }
}

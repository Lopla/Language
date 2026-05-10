using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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

    public ObservableCollection<WorkflowElementViewModel> WorkflowElements { get; } = new ObservableCollection<WorkflowElementViewModel>();

    public ObservableCollection<WorkflowPaletteItemViewModel> PaletteItems { get; } = new ObservableCollection<WorkflowPaletteItemViewModel>();

    public double CanvasWidth { get; } = 1200;
    public double CanvasHeight { get; } = 600;

    public MainViewModel()
    {
        InitializePalette();
        LoadExample();
    }

    private void InitializePalette()
    {
        PaletteItems.Add(new WorkflowPaletteItemViewModel
        {
            Type = WorkflowBlockType.If,
            Title = "IF",
            Description = "If condition block"
        });

        PaletteItems.Add(new WorkflowPaletteItemViewModel
        {
            Type = WorkflowBlockType.While,
            Title = "WHILE",
            Description = "Placeholder block"
        });

        PaletteItems.Add(new WorkflowPaletteItemViewModel
        {
            Type = WorkflowBlockType.FunctionCall,
            Title = "FUNCTION CALL",
            Description = "Placeholder block"
        });

        PaletteItems.Add(new WorkflowPaletteItemViewModel
        {
            Type = WorkflowBlockType.Assignment,
            Title = "ASSIGNMENT",
            Description = "Placeholder block"
        });
    }

    public void AddBlock(WorkflowBlockType type, double left, double top)
    {
        WorkflowElements.Add(new WorkflowElementViewModel
        {
            Type = type,
            Title = GetBlockTitle(type),
            Description = GetBlockDescription(type),
            Left = left,
            Top = top
        });
    }

    private string GetBlockTitle(WorkflowBlockType type)
    {
        return type switch
        {
            WorkflowBlockType.If => "IF",
            WorkflowBlockType.While => "WHILE",
            WorkflowBlockType.FunctionCall => "FUNCTION CALL",
            WorkflowBlockType.Assignment => "ASSIGNMENT",
            _ => "BLOCK"
        };
    }

    private string GetBlockDescription(WorkflowBlockType type)
    {
        return type switch
        {
            WorkflowBlockType.If => "If condition block",
            WorkflowBlockType.While => "Loop placeholder",
            WorkflowBlockType.FunctionCall => "Call placeholder",
            WorkflowBlockType.Assignment => "Assignment placeholder",
            _ => string.Empty
        };
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

        PopulateWorkflowElements(result.Root);
    }

    private void PopulateWorkflowElements(ParseTreeNode? root)
    {
        WorkflowElements.Clear();
        if (root == null)
            return;

        var nodes = CollectWorkflowNodes(root).ToList();
        for (var i = 0; i < nodes.Count; i++)
        {
            var node = nodes[i];
            WorkflowElements.Add(new WorkflowElementViewModel
            {
                Title = node.Symbol,
                Description = string.IsNullOrWhiteSpace(node.Value) ? string.Empty : node.Value,
                Left = i * 220,
                Top = 0
            });
        }
    }

    private static IEnumerable<ParseTreeNode> CollectWorkflowNodes(ParseTreeNode node, int depth = 0)
    {
        if (node == null)
            yield break;

        if (depth > 0)
            yield return node;

        if (depth < 2)
        {
            foreach (var child in node.Children)
            {
                foreach (var childNode in CollectWorkflowNodes(child, depth + 1))
                {
                    yield return childNode;
                }
            }
        }
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

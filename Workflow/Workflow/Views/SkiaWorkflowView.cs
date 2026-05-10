using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Workflow.ViewModels;

namespace Workflow.Views;

public class SkiaWorkflowView : Control
{
    public static readonly StyledProperty<IEnumerable<WorkflowElementViewModel>> ElementsProperty =
        AvaloniaProperty.Register<SkiaWorkflowView, IEnumerable<WorkflowElementViewModel>>(nameof(Elements), Array.Empty<WorkflowElementViewModel>());

    public IEnumerable<WorkflowElementViewModel> Elements
    {
        get => GetValue(ElementsProperty);
        set => SetValue(ElementsProperty, value);
    }

    private INotifyCollectionChanged? _collection;

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == ElementsProperty)
        {
            if (_collection != null)
            {
                _collection.CollectionChanged -= OnCollectionChanged;
                _collection = null;
            }

            if (change.NewValue is INotifyCollectionChanged collection)
            {
                _collection = collection;
                _collection.CollectionChanged += OnCollectionChanged;
            }

            InvalidateMeasure();
            InvalidateVisual();
        }
    }

    private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        InvalidateMeasure();
        InvalidateVisual();
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);

        context.FillRectangle(Brushes.Black, new Rect(Bounds.Size));

        var items = Elements?.ToList() ?? new List<WorkflowElementViewModel>();
        for (var i = 0; i < items.Count; i++)
        {
            var element = items[i];
            var rect = new Rect(10 + i * 220, 10, 200, 120);
            context.FillRectangle(Brushes.DodgerBlue, rect);

            var titleText = new FormattedText(element.Title, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight,
                new Typeface("Segoe UI"), 16, Brushes.White);
            context.DrawText(titleText, rect.Position + new Point(10, 10));

            var descText = new FormattedText(element.Description, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight,
                new Typeface("Segoe UI"), 12, Brushes.White)
            {
                MaxTextWidth = rect.Width - 20
            };
            context.DrawText(descText, rect.Position + new Point(10, 36));
        }
    }

    protected override Size MeasureOverride(Size available)
    {
        var count = Elements?.Count() ?? 0;
        var contentWidth = 10 + count * 220;
        var width = double.IsInfinity(available.Width) ? contentWidth : Math.Max(available.Width, contentWidth);
        return new Size(width, 150);
    }
}

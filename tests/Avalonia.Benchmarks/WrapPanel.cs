// Copyright (c) Avalonia.Benchmarks.
// Licensed under the GPL-3.0 License.

using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using BenchmarkDotNet.Attributes;

namespace Avalonia.Benchmarks;

[MemoryDiagnoser]
[MinIterationTime(150)]
[MaxWarmupCount(15)]
public class WrapPanelTest
{
    private WrapPanel wrapPanel;

    private UvWrapPanel uvWrapPanel;

    [GlobalSetup]
    public void Setup()
    {
        var test = Enumerable.Range(0, 30).Select(i => Random.Shared.Next(1, 4) * 50d).ToArray();
        wrapPanel = new WrapPanel()
        {
            ItemHeight = 50,
            ItemSpacing = 5,
            ItemsAlignment = WrapPanelItemsAlignment.Start,
            LineSpacing = 5,
        };
        wrapPanel.Children.AddRange(test.Select(t => new Rectangle()
        {
            MinWidth = t
        }));
        uvWrapPanel = new UvWrapPanel()
        {
            ItemHeight = 50,
            ItemSpacing = 5,
            ItemsAlignment = WrapPanelItemsAlignment.Start,
            LineSpacing = 5,
        };
        uvWrapPanel.Children.AddRange(test.Select(t => new Rectangle()
        {
            MinWidth = t
        }));

    }

    [Benchmark]
    public WrapPanel WrapPanel()
    {
        wrapPanel.Measure(Size.Infinity);
        wrapPanel.Arrange(new Rect(wrapPanel.DesiredSize));
        return wrapPanel;
    }

    [Benchmark]
    public UvWrapPanel UVWrapPanel()
    {
        uvWrapPanel.Measure(Size.Infinity);
        uvWrapPanel.Arrange(new Rect(uvWrapPanel.DesiredSize));
        return uvWrapPanel;
    }
}

﻿using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;
using Avalonia.Data;
using BenchmarkDotNet.Attributes;

namespace Avalonia.Benchmarks.Base
{
    [MemoryDiagnoser]
    public class StyledPropertyBenchmarks
    {
        [Benchmark]
        public void Set_Int_Property_LocalValue()
        {
            var obj = new StyledClass();

            for (var i = 0; i < 100; ++i)
            {
                obj.IntValue += 1;
            }
        }

        [Benchmark]
        public void Set_Int_Property_Multiple_Priorities()
        {
            var obj = new StyledClass();
            var value = 0;

            for (var i = 0; i < 100; ++i)
            {
                for (var p = BindingPriority.Animation; p <= BindingPriority.Style; ++p)
                {
                    obj.SetValue(StyledClass.IntValueProperty, value++, p);
                }
            }
        }

        [Benchmark]
        public void Set_Int_Property_TemplatedParent()
        {
            var obj = new StyledClass();

            for (var i = 0; i < 100; ++i)
            {
                obj.SetValue(StyledClass.IntValueProperty, obj.IntValue + 1, BindingPriority.TemplatedParent);
            }
        }

        [Benchmark]
        public void Bind_Int_Property_LocalValue()
        {
            var obj = new StyledClass();
            var source = new Subject<BindingValue<int>>();

            obj.Bind(StyledClass.IntValueProperty, source);

            for (var i = 0; i < 100; ++i)
            {
                source.OnNext(i);
            }
        }

        [Benchmark]
        public void Bind_Int_Property_Multiple_Priorities()
        {
            var obj = new StyledClass();
            var sources = new List<Subject<BindingValue<int>>>();
            var value = 0;

            for (var p = BindingPriority.Animation; p <= BindingPriority.Style; ++p)
            {
                var source = new Subject<BindingValue<int>>();
                sources.Add(source);
                obj.Bind(StyledClass.IntValueProperty, source, p);
            }

            for (var i = 0; i < 100; ++i)
            {
                foreach (var source in sources)
                {
                    source.OnNext(value++);
                }
            }
        }

        class StyledClass : AvaloniaObject
        {
            private int _intValue;

            public static readonly StyledProperty<int> IntValueProperty =
                AvaloniaProperty.Register<StyledClass, int>(nameof(IntValue));

            public int IntValue
            {
                get => GetValue(IntValueProperty);
                set => SetValue(IntValueProperty, value);
            }
        }
    }
}

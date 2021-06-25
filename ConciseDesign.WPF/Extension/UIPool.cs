using System;
using System.Collections.Concurrent;
using System.Windows;
using System.Windows.Controls;
using Castle.MicroKernel.Registration;
using FORCEBuild;

namespace ConciseDesign.WPF.Extension
{
    public class UIPool<T>: ObjectPool<T> where T: UIElement
    {
        public UIPool(Func<T> objectGenerator) : base(objectGenerator)
        {
            
        }

        /// <summary>
        /// 预分配
        /// </summary>
        public void PreAllocate(int size)
        {
            for (int i = 0; i < size; i++)
            {
                var uiElement = _objectGenerator.Invoke();
                _objects.Add(uiElement);
            }
        }

        public void PreRender()
        {
            var availableSize = new Size(double.PositiveInfinity,double.PositiveInfinity);
            foreach (var uiElement in _objects)
            {
                uiElement.Measure(availableSize);
                uiElement.Arrange(new Rect(uiElement.RenderSize));
                uiElement.UpdateLayout();
            }
        }

    }
}
using System;
using System.Windows;
using FORCEBuild;

namespace ConciseDesign.WPF
{
    /*public class ViewPool
    {


    }

    interface IPreRender
    {
        void PreRend();
    }
    
    public class UIElementPool<T>:ObjectPool<T>,IPreRender where T: UIElement
    {
        public int PreRenderCount { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectGenerator"></param>
        /// <param name="preRenderCount">ui element default is 1</param>
        public UIElementPool(Func<T> objectGenerator, int preRenderCount=1) : base(objectGenerator)
        {
            PreRenderCount = preRenderCount;
        }

        private readonly Size _defaultSize = new Size(double.PositiveInfinity, double.PositiveInfinity);
        public void PreRend()
        {
            for (var i = 0; i < PreRenderCount; i++)
            {
                var uiElement = this.GetObject();
                uiElement.Measure(_defaultSize);
                uiElement.Arrange(new Rect(uiElement.RenderSize));
            }
        }
    }*/
}
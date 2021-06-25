using System.Windows;
using Castle.Windsor;

namespace ConciseDesign.WPF.Extension
{
    /// <summary>
    /// 适配各种容器的对象池，必须设置为单例
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class WindsorContainerUIPool<T> where T: UIElement
    {
        private readonly UIPool<T> _uiPool;
        
        public WindsorContainerUIPool(IWindsorContainer container)
        {
            _uiPool = new UIPool<T>(container.Resolve<T>);
        }

        public void PreAllocate(int size)
        {
            this._uiPool.PreAllocate(size);
        }

        public void PreRender()
        {
            this._uiPool.PreRender();
        }
    }
}
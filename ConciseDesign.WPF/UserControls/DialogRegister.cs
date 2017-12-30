using System.Collections.Concurrent;
using System.Windows;

namespace ConciseDesign.WPF.UserControls
{
    /*2017.8 本类也可以使用behavior实现*/

    /// <summary>
    /// 对话框分发器
    /// </summary>
    public static class DialogRegister
    {
        private static readonly ConcurrentDictionary<string, DialogHostControl> DialogHostControls =
            new ConcurrentDictionary<string, DialogHostControl>();

        public static readonly DependencyProperty RegisterProperty = DependencyProperty.RegisterAttached(
            "Register", typeof(string), typeof(DialogRegister), new PropertyMetadata(default(string), RegisterPropertyChangedCallback));

        private static void RegisterPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            if (dependencyPropertyChangedEventArgs.OldValue != null)
            {
                DialogHostControls.TryRemove(dependencyPropertyChangedEventArgs.OldValue.ToString(),
                    out DialogHostControl control);
            }
            if (dependencyPropertyChangedEventArgs.NewValue != null) {
                DialogHostControls.TryAdd(dependencyPropertyChangedEventArgs.NewValue.ToString(),
                    dependencyObject as DialogHostControl);
            }
        }

        /// <summary>
        /// 注册对话框id
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetRegister(DialogHostControl element, string value)
        {
            element.SetValue(RegisterProperty, value);
        }

        public static string GetRegister(DialogHostControl element)
        {
            return (string) element.GetValue(RegisterProperty);
        }

        public static DialogHostControl GetById(string id)
        {
            return DialogHostControls.TryGetValue(id, out DialogHostControl value) ? value : null;
        }

    }
}
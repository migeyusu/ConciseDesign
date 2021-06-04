using System;
using System.Diagnostics;
using System.Windows;
using Microsoft.Xaml.Behaviors;

namespace ConciseDesign.WPF.Behavior
{
    public class StateMachineBehavior: Behavior<FrameworkElement>
    {
        public static readonly DependencyProperty StateProperty = DependencyProperty.Register(
            "State", typeof(string), typeof(StateMachineBehavior), new PropertyMetadata(default(string),(PropertyChangedCallback)));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var frameworkElement = d as FrameworkElement;
            var value = (bool) frameworkElement.GetValue(UseTransitionsProperty);
            var goToElementState = VisualStateManager.GoToElementState(frameworkElement, e.NewValue.ToString(), value);
            if (!goToElementState)
            {
                Debug.WriteLine($"Go to element state {e.NewValue} failed");
            }
        }

        public string State
        {
            get { return (string) GetValue(StateProperty); }

            set { SetValue(StateProperty, value); }
        }

        public static readonly DependencyProperty UseTransitionsProperty = DependencyProperty.Register(
            "UseTransitions", typeof(bool), typeof(StateMachineBehavior), new PropertyMetadata(true));

        public bool UseTransitions
        {
            get { return (bool) GetValue(UseTransitionsProperty); }
            set
            {
                SetValue(UseTransitionsProperty, value);
            }
        }
    }
    
    public class StateMachineHelper
    {
        public static readonly DependencyProperty StateProperty = DependencyProperty.RegisterAttached(
            "State", typeof(string), typeof(StateMachineHelper),
            defaultMetadata: new UIPropertyMetadata(null, PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var frameworkElement = d as FrameworkElement;
            VisualStateManager.GoToElementState(frameworkElement, e.NewValue.ToString(), true);
        }

        public static void SetState(FrameworkElement element, string value)
        {
            element.SetValue(StateProperty, value);
        }

        public static string GetState(FrameworkElement element)
        {
            return (string) element.GetValue(StateProperty);
        }
    }
}
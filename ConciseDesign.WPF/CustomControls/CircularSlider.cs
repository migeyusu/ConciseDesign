using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ConciseDesign.WPF.CustomControls
{
    public class CircularSlider : Slider
    {
        static CircularSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CircularSlider),
                new FrameworkPropertyMetadata(typeof(CircularSlider)));
        }

        public static readonly DependencyProperty IsReadonlyProperty = DependencyProperty.Register(
            "IsReadonly", typeof(bool), typeof(CircularSlider), new PropertyMetadata(default(bool)));

        /// <summary>
        /// 只读模式时，作为progressbar
        /// </summary>
        public bool IsReadonly
        {
            get { return (bool) GetValue(IsReadonlyProperty); }
            set { SetValue(IsReadonlyProperty, value); }
        }  

        public CircularSlider()
        {
        }

        public static RoutedEvent ClickEvent =
            EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler),
                typeof(CircularSlider));

        public event RoutedEventHandler Click
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }

        readonly Point _zeroPoint = new Point(0, 0);

        protected virtual void OnClick()
        {
            var args = new RoutedEventArgs(ClickEvent, this);
            var position = Mouse.GetPosition(this);
            //转换坐标系
            var x = position.X - ActualWidth / 2.0;
            var y = -position.Y + ActualHeight / 2.0;
            var actualWidth = ActualWidth / 2.0 - Thickness;
            var actualHeight = ActualHeight / 2.0 - Thickness;
            if ((int) actualWidth == (int) actualHeight)
            {
                //圆形算法
                var point = new Point(x, y);
                var length = Point.Subtract(_zeroPoint, point).Length;
                if (actualWidth < length && length < ActualWidth / 2.0)
                {
                    x = -x;
                    var atan = Math.Atan2(y, x) * 180.0 / Math.PI;
                    if (atan < 0.0d)
                    {
                        atan = 360.0 + atan;
                    }

                    this.CalculateValue((this.Maximum - this.Minimum) *
                        ((atan <= 90.0 ? atan + 270.0 : atan - 90.0) / 360.0) + this.Minimum);
                }
            }


            RaiseEvent(args);
        }

        protected virtual void CalculateValue(double value)
        {
            if (this.IsSnapToTickEnabled)
            {
                double num1 = this.Minimum;
                double num2 = this.Maximum;
                if (DoubleUtil.GreaterThan(this.TickFrequency, 0.0))
                {
                    num1 = this.Minimum + Math.Round((value - this.Minimum) / this.TickFrequency) * this.TickFrequency;
                    num2 = Math.Min(this.Maximum, num1 + this.TickFrequency);
                }

                value = DoubleUtil.GreaterThanOrClose(value, (num1 + num2) * 0.5) ? num2 : num1;
            }

            this.Value = value;
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            if (IsReadonly)
            {
                return;
            }
            OnClick();
        }

        public object Content
        {
            get { return (object) GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Content.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(object), typeof(CircularSlider),
                new PropertyMetadata(null));


        protected override void OnMaximumChanged(double oldMaximum, double newMaximum)
        {
            base.OnMaximumChanged(oldMaximum, newMaximum);
            PercentageValue = ProcessValue(newMaximum, this.Minimum, this.Value);
        }

        protected override void OnMinimumChanged(double oldMinimum, double newMinimum)
        {
            base.OnMinimumChanged(oldMinimum, newMinimum);
            PercentageValue = ProcessValue(this.Maximum, newMinimum, this.Value);
        }

        protected override void OnValueChanged(double oldValue, double newValue)
        {
            base.OnValueChanged(oldValue, newValue);
            PercentageValue = ProcessValue(this.Maximum, this.Minimum, newValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maximum"></param>
        /// <param name="minimum"></param>
        /// <param name="value"></param>
        /// <returns>percentage value</returns>
        private static double ProcessValue(double maximum, double minimum, double value)
        {
            return (value) / (maximum - minimum) * 100;
        }

        /// <summary>
        /// percentage value
        /// </summary>
        public double PercentageValue
        {
            get { return (double) GetValue(PercentageValueProperty); }
            set { SetValue(PercentageValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Percentage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PercentageValueProperty =
            DependencyProperty.Register("PercentageValue", typeof(double), typeof(CircularSlider),
                new PropertyMetadata(0d));

        public double Thickness
        {
            get { return (double) GetValue(ThicknessProperty); }
            set { SetValue(ThicknessProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Thickness.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ThicknessProperty =
            DependencyProperty.Register("Thickness", typeof(double), typeof(CircularSlider),
                new PropertyMetadata(10d));
    }
}
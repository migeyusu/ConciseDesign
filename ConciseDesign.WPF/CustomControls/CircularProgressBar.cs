using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ConciseDesign.WPF.CustomControls
{
    public class CircularProgressBar : ProgressBar
    {
        static CircularProgressBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CircularProgressBar),
                new FrameworkPropertyMetadata(typeof(CircularProgressBar)));
        }

        public CircularProgressBar() { }

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
        public double PercentageValue {
            get { return (double) GetValue(PercentageValueProperty); }
            set { SetValue(PercentageValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Percentage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PercentageValueProperty =
            DependencyProperty.Register("PercentageValue", typeof(double), typeof(CircularProgressBar),
                new PropertyMetadata(0d));


        public Brush HeaderBrush {
            get { return (Brush) GetValue(HeaderBrushProperty); }
            set { SetValue(HeaderBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeaderBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderBrushProperty =
            DependencyProperty.Register("HeaderBrush", typeof(Brush), typeof(CircularProgressBar),
                new PropertyMetadata(Brushes.DimGray));

        public double Thickness {
            get { return (int) GetValue(ThicknessProperty); }
            set { SetValue(ThicknessProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Thickness.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ThicknessProperty =
            DependencyProperty.Register("Thickness", typeof(double), typeof(CircularProgressBar),
                new PropertyMetadata(10d));
    }
}
using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ConciseDesign.WPF.CustomControls
{
    public class RoundShape : Shape
    {
        static RoundShape()
        {
            Brush myGreenBrush = new SolidColorBrush(Color.FromArgb(255, 6, 176, 37));
            myGreenBrush.Freeze();

            StrokeProperty.OverrideMetadata(
                typeof(RoundShape),
                new FrameworkPropertyMetadata(myGreenBrush));
            FillProperty.OverrideMetadata(
                typeof(RoundShape),
                new FrameworkPropertyMetadata(Brushes.Transparent));

            StrokeThicknessProperty.OverrideMetadata(
                typeof(RoundShape),
                new FrameworkPropertyMetadata(10.0));
        }

        // Value (0-100)
        public double Value {
            get { return (double) GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // DependencyProperty - Value (0 - 100)
        private static readonly FrameworkPropertyMetadata valueMetadata =
            new FrameworkPropertyMetadata(
                0.0, // Default value
                FrameworkPropertyMetadataOptions.AffectsRender,
                null, // Property changed callback
                CoerceValue); // Coerce value callback

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(RoundShape), valueMetadata);

        private static object CoerceValue(DependencyObject depObj, object baseVal)
        {
            var val = (double) baseVal;
            val = Math.Min(val, 99.999);
            val = Math.Max(val, 0.0);
            return val;
        }

        protected override Geometry DefiningGeometry {
            get {
                var startAngle = 90.0;
                var endAngle = 90.0 - ((Value / 100.0) * 360.0);
                var maxWidth = Math.Max(0.0, RenderSize.Width - StrokeThickness);
                var maxHeight = Math.Max(0.0, RenderSize.Height - StrokeThickness);
                var xStart = maxWidth / 2.0 * Math.Cos(startAngle * Math.PI / 180.0);
                var yStart = maxHeight / 2.0 * Math.Sin(startAngle * Math.PI / 180.0);
                var xEnd = maxWidth / 2.0 * Math.Cos(endAngle * Math.PI / 180.0);
                var yEnd = maxHeight / 2.0 * Math.Sin(endAngle * Math.PI / 180.0);
                var streamGeometry = new StreamGeometry();
                var endPoint = new Point(RenderSize.Width / 2.0 + xEnd,
                    RenderSize.Height / 2.0 - yEnd);
                using (var context = streamGeometry.Open()) {
                    context.BeginFigure(
                        new Point(RenderSize.Width / 2.0 + xStart,
                            RenderSize.Height / 2.0 - yStart),
                        true, // Filled
                        false); // Closed
                    
                    context.ArcTo(
                        endPoint,
                        new Size(maxWidth / 2.0, maxHeight / 2),
                        0.0, // rotationAngle
                        (startAngle - endAngle) > 180, // greater than 180 deg?
                        SweepDirection.Clockwise,
                        true, // isStroked
                        false);
                    return streamGeometry;
                }
            }
        }
        
        void DrawBezierFigure(StreamGeometryContext ctx, PathFigure figure)
        {
            ctx.BeginFigure(figure.StartPoint, figure.IsFilled, figure.IsClosed);
            foreach(var segment in figure.Segments.OfType<BezierSegment>())
                ctx.BezierTo(segment.Point1, segment.Point2, segment.Point3, segment.IsStroked, segment.IsSmoothJoin);
        }
        
    }
}
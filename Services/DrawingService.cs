using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace NullAI.Services
{
    /// <summary>
    /// Provides basic drawing capabilities on a WPF Canvas.  This
    /// service wires up mouse events to allow freehand drawing.  The
    /// drawing logic is intentionally simple; more advanced features
    /// (e.g. shapes, undo/redo) can be added later.
    /// </summary>
    public class DrawingService
    {
        private Point? _lastPoint;

        public void AttachCanvas(Canvas canvas)
        {
            canvas.MouseDown += Canvas_MouseDown;
            canvas.MouseMove += Canvas_MouseMove;
            canvas.MouseUp += Canvas_MouseUp;
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var canvas = sender as Canvas;
            _lastPoint = e.GetPosition(canvas);
            canvas.CaptureMouse();
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_lastPoint == null) return;
            var canvas = sender as Canvas;
            var current = e.GetPosition(canvas);
            var line = new System.Windows.Shapes.Line
            {
                X1 = _lastPoint.Value.X,
                Y1 = _lastPoint.Value.Y,
                X2 = current.X,
                Y2 = current.Y,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };
            canvas.Children.Add(line);
            _lastPoint = current;
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var canvas = sender as Canvas;
            canvas.ReleaseMouseCapture();
            _lastPoint = null;
        }
    }
}
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SnapNShare.Overlay
{
    public class ClippingRectangle : IDisposable
    {
        System.Windows.Shapes.Rectangle _topRectangle;
        System.Windows.Shapes.Rectangle _leftRectangle;
        System.Windows.Shapes.Rectangle _rightRectangle;
        System.Windows.Shapes.Rectangle _bottomRectangle;
        System.Windows.Controls.TextBlock _textBlockX;
        System.Drawing.Rectangle _screenBounds = System.Windows.Forms.Screen.PrimaryScreen.Bounds;

        Canvas _canvas;
        public ClippingRectangle(Canvas canvas)
        {
            _canvas = canvas;
            var brush = new SolidColorBrush(Color.FromArgb(150, 0, 0, 0));

            _topRectangle = new System.Windows.Shapes.Rectangle();
            _topRectangle.Fill = brush;

            _leftRectangle = new System.Windows.Shapes.Rectangle();
            _leftRectangle.Fill = brush;

            _rightRectangle = new System.Windows.Shapes.Rectangle();
            _rightRectangle.Fill = brush;

            _bottomRectangle = new System.Windows.Shapes.Rectangle();
            _bottomRectangle.Fill = brush;

            _textBlockX = new TextBlock();
            _textBlockX.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#99FF55"));
            _textBlockX.FontSize = 10;

            canvas.Children.Add(_topRectangle);
            canvas.Children.Add(_leftRectangle);
            canvas.Children.Add(_rightRectangle);
            canvas.Children.Add(_bottomRectangle);
            canvas.Children.Add(_textBlockX);

            ArrangeRectangle(_topRectangle, new Point(0, 0), new Size(_screenBounds.Width, _screenBounds.Height));
        }

        public void Arrange(Point initialPositionWPF, Point currentPositionWPF,
            Point absoluteInitialPosition, Point absoluteCurrentPosition)
        {
            double width = Math.Round(Math.Abs(currentPositionWPF.X - initialPositionWPF.X), 0);
            double height = Math.Round(Math.Abs(currentPositionWPF.Y - initialPositionWPF.Y), 0);

            double widthAbsolute = Math.Round(Math.Abs(absoluteCurrentPosition.X - absoluteInitialPosition.X), 0);
            double heightAbsolute = Math.Round(Math.Abs(absoluteCurrentPosition.Y - absoluteInitialPosition.Y), 0);

            ArrangeRectangle(_topRectangle, new Point(0, 0), new Size(_screenBounds.Width, initialPositionWPF.Y));
            ArrangeRectangle(_leftRectangle, new Point(0, initialPositionWPF.Y), new Size(initialPositionWPF.X, Math.Abs(_screenBounds.Height - initialPositionWPF.Y)));

            ArrangeRectangle(_rightRectangle, new Point(currentPositionWPF.X, initialPositionWPF.Y),
                new Size(Math.Abs(_screenBounds.Width - currentPositionWPF.X), Math.Abs(_screenBounds.Height - initialPositionWPF.Y)));
            ArrangeRectangle(_bottomRectangle, new Point(initialPositionWPF.X, currentPositionWPF.Y),
                new Size(Math.Abs(currentPositionWPF.X - initialPositionWPF.X), Math.Abs(_screenBounds.Height - currentPositionWPF.Y)));

            ArrangeTextBlock(_textBlockX, new Point(currentPositionWPF.X + 2, currentPositionWPF.Y + 2),
                string.Format("W:{0} H:{1}", widthAbsolute, heightAbsolute));
        }

        private void ArrangeRectangle(System.Windows.Shapes.Rectangle rectangle, Point position, Size size)
        {
            rectangle.Width = size.Width;
            rectangle.Height = size.Height;
            PositionOnCanvas(rectangle, position);
        }

        private void ArrangeTextBlock(TextBlock textBlock, Point position, string value)
        {
            textBlock.Text = value;
            PositionOnCanvas(textBlock, position);
        }

        private void PositionOnCanvas(FrameworkElement element, Point position)
        {
            Canvas.SetTop(element, position.Y);
            Canvas.SetLeft(element, position.X);
        }

        public void Dispose()
        {
            _canvas.Children.Clear();
        }
    }
}

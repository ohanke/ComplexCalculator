using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using ComplexCalculator.Models;

namespace ComplexCalculator.Services
{
    public class ComplexPlaneDrawingService
    {
        private const double DefaultScale = 40;
        private const double PointSize = 10;
        private const double AxisStrokeThickness = 2;
        private const double GridStrokeThickness = 1;
        private const double VectorStrokeThickness = 3;
        private const double ArrowLength = 12;
        private const double ArrowAngle = Math.PI / 6;
        private const double CanvasMargin = 30;

        public void DrawPlane(Canvas canvas)
        {
            ValidateCanvas(canvas);

            canvas.Children.Clear();

            double width = canvas.Width;
            double height = canvas.Height;

            double centerX = width / 2;
            double centerY = height / 2;

            DrawGrid(canvas, centerX, centerY, width, height);
            DrawAxis(canvas, centerX, centerY, width, height);
            DrawAxisLabels(canvas, centerX, centerY, width, height);
        }

        public void DrawVector(Canvas canvas, ComplexNumber number)
        {
            ValidateCanvas(canvas);

            if (number is null)
            {
                throw new ArgumentNullException(nameof(number));
            }

            double width = canvas.Width;
            double height = canvas.Height;

            double centerX = width / 2;
            double centerY = height / 2;

            double scale = CalculateScale(canvas, number);

            double endX = centerX + number.Real * scale;
            double endY = centerY - number.Imaginary * scale;

            Line vectorLine = new Line
            {
                X1 = centerX,
                Y1 = centerY,
                X2 = endX,
                Y2 = endY,
                Stroke = Brushes.Red,
                StrokeThickness = VectorStrokeThickness
            };

            Ellipse point = new Ellipse
            {
                Width = PointSize,
                Height = PointSize,
                Fill = Brushes.Red
            };

            Canvas.SetLeft(point, endX - PointSize / 2);
            Canvas.SetTop(point, endY - PointSize / 2);

            TextBlock pointLabel = new TextBlock
            {
                Text = $"z = {number}",
                Foreground = Brushes.Red,
                FontWeight = FontWeights.Bold
            };

            Canvas.SetLeft(pointLabel, endX + 8);
            Canvas.SetTop(pointLabel, endY - 20);

            canvas.Children.Add(vectorLine);

            if (!number.IsZero())
            {
                DrawArrowHead(canvas, centerX, centerY, endX, endY);
            }

            canvas.Children.Add(point);
            canvas.Children.Add(pointLabel);
        }

        private void DrawAxis(Canvas canvas, double centerX, double centerY, double width, double height)
        {
            Line realAxis = new Line
            {
                X1 = 0,
                Y1 = centerY,
                X2 = width,
                Y2 = centerY,
                Stroke = Brushes.Black,
                StrokeThickness = AxisStrokeThickness
            };

            Line imaginaryAxis = new Line
            {
                X1 = centerX,
                Y1 = 0,
                X2 = centerX,
                Y2 = height,
                Stroke = Brushes.Black,
                StrokeThickness = AxisStrokeThickness
            };

            canvas.Children.Add(realAxis);
            canvas.Children.Add(imaginaryAxis);
        }

        private void DrawGrid(Canvas canvas, double centerX, double centerY, double width, double height)
        {
            for (double x = centerX + DefaultScale; x < width; x += DefaultScale)
            {
                DrawVerticalGridLine(canvas, x, height);
            }

            for (double x = centerX - DefaultScale; x > 0; x -= DefaultScale)
            {
                DrawVerticalGridLine(canvas, x, height);
            }

            for (double y = centerY + DefaultScale; y < height; y += DefaultScale)
            {
                DrawHorizontalGridLine(canvas, y, width);
            }

            for (double y = centerY - DefaultScale; y > 0; y -= DefaultScale)
            {
                DrawHorizontalGridLine(canvas, y, width);
            }
        }

        private void DrawVerticalGridLine(Canvas canvas, double x, double height)
        {
            Line line = new Line
            {
                X1 = x,
                Y1 = 0,
                X2 = x,
                Y2 = height,
                Stroke = Brushes.LightGray,
                StrokeThickness = GridStrokeThickness
            };

            canvas.Children.Add(line);
        }

        private void DrawHorizontalGridLine(Canvas canvas, double y, double width)
        {
            Line line = new Line
            {
                X1 = 0,
                Y1 = y,
                X2 = width,
                Y2 = y,
                Stroke = Brushes.LightGray,
                StrokeThickness = GridStrokeThickness
            };

            canvas.Children.Add(line);
        }

        private void DrawAxisLabels(Canvas canvas, double centerX, double centerY, double width, double height)
        {
            TextBlock realLabel = new TextBlock
            {
                Text = "Re",
                FontWeight = FontWeights.Bold
            };

            Canvas.SetLeft(realLabel, width - 30);
            Canvas.SetTop(realLabel, centerY + 5);
            canvas.Children.Add(realLabel);

            TextBlock imaginaryLabel = new TextBlock
            {
                Text = "Im",
                FontWeight = FontWeights.Bold
            };

            Canvas.SetLeft(imaginaryLabel, centerX + 5);
            Canvas.SetTop(imaginaryLabel, 5);
            canvas.Children.Add(imaginaryLabel);

            TextBlock zeroLabel = new TextBlock
            {
                Text = "0"
            };

            Canvas.SetLeft(zeroLabel, centerX + 5);
            Canvas.SetTop(zeroLabel, centerY + 5);
            canvas.Children.Add(zeroLabel);
        }

        private void DrawArrowHead(Canvas canvas, double startX, double startY, double endX, double endY)
        {
            double angle = Math.Atan2(endY - startY, endX - startX);

            double x1 = endX - ArrowLength * Math.Cos(angle - ArrowAngle);
            double y1 = endY - ArrowLength * Math.Sin(angle - ArrowAngle);

            double x2 = endX - ArrowLength * Math.Cos(angle + ArrowAngle);
            double y2 = endY - ArrowLength * Math.Sin(angle + ArrowAngle);

            Line arrowLine1 = new Line
            {
                X1 = endX,
                Y1 = endY,
                X2 = x1,
                Y2 = y1,
                Stroke = Brushes.Red,
                StrokeThickness = VectorStrokeThickness
            };

            Line arrowLine2 = new Line
            {
                X1 = endX,
                Y1 = endY,
                X2 = x2,
                Y2 = y2,
                Stroke = Brushes.Red,
                StrokeThickness = VectorStrokeThickness
            };

            canvas.Children.Add(arrowLine1);
            canvas.Children.Add(arrowLine2);
        }

        private double CalculateScale(Canvas canvas, ComplexNumber number)
        {
            double maxCoordinate = Math.Max(Math.Abs(number.Real), Math.Abs(number.Imaginary));

            if (maxCoordinate == 0)
            {
                return DefaultScale;
            }

            double maxCanvasRadius = Math.Min(canvas.Width, canvas.Height) / 2 - CanvasMargin;

            return Math.Min(DefaultScale, maxCanvasRadius / maxCoordinate);
        }

        private void ValidateCanvas(Canvas canvas)
        {
            if (canvas is null)
            {
                throw new ArgumentNullException(nameof(canvas));
            }
        }
    }
}
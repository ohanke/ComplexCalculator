using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using ComplexCalculator.Models;

namespace ComplexCalculator.Services
{

    /// <summary>
    /// Draws the complex plane and complex number vectors on a WPF canvas.
    /// </summary>
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

        /// <summary>
        /// Draws the empty complex plane on the given canvas.
        /// </summary>
        /// <param name="canvas">The canvas where the complex plane should be drawn.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the canvas is null.
        /// </exception>
        public void DrawPlane(Canvas canvas)
        {
            ValidateCanvas(canvas);

            // Remove all previous elements before drawing the plane again.
            canvas.Children.Clear();

            double width = canvas.Width;
            double height = canvas.Height;

            // The center of the canvas is the point 0 + 0i.
            double centerX = width / 2;
            double centerY = height / 2;

            DrawGrid(canvas, centerX, centerY, width, height);
            DrawAxis(canvas, centerX, centerY, width, height);
            DrawAxisLabels(canvas, centerX, centerY, width, height);
        }

        /// <summary>
        /// Draws a complex number as a vector on the complex plane.
        /// </summary>
        /// <param name="canvas">The canvas where the vector should be drawn.</param>
        /// <param name="number">The complex number to draw.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the canvas or the complex number is null.
        /// </exception>
        public void DrawVector(Canvas canvas, ComplexNumber number)
        {
            ValidateCanvas(canvas);

            if (number is null)
            {
                throw new ArgumentNullException(nameof(number));
            }

            double width = canvas.Width;
            double height = canvas.Height;

            // The vector starts in the center of the canvas.
            double centerX = width / 2;
            double centerY = height / 2;

            double scale = CalculateScale(canvas, number);

            // In WPF, the Y axis grows downward - that is why the imaginary part is subtracted from centerY.
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

            // Move the point so that its center is placed at the vector end.
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

            // A zero vector has no visible direction, so the arrow head is skipped.
            if (!number.IsZero())
            {
                DrawArrowHead(canvas, centerX, centerY, endX, endY);
            }

            canvas.Children.Add(point);
            canvas.Children.Add(pointLabel);
        }

        /// <summary>
        /// Draws the real and imaginary axes.
        /// </summary>
        /// <param name="canvas">The canvas where the axes should be drawn.</param>
        /// <param name="centerX">The X coordinate of the canvas center.</param>
        /// <param name="centerY">The Y coordinate of the canvas center.</param>
        /// <param name="width">The canvas width.</param>
        /// <param name="height">The canvas height.</param>
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

        /// <summary>
        /// Draws helper grid lines on the complex plane.
        /// </summary>
        /// <param name="canvas">The canvas where the grid should be drawn.</param>
        /// <param name="centerX">The X coordinate of the canvas center.</param>
        /// <param name="centerY">The Y coordinate of the canvas center.</param>
        /// <param name="width">The canvas width.</param>
        /// <param name="height">The canvas height.</param>
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

        /// <summary>
        /// Draws one vertical grid line.
        /// </summary>
        /// <param name="canvas">The canvas where the line should be drawn.</param>
        /// <param name="x">The X coordinate of the vertical line.</param>
        /// <param name="height">The canvas height.</param>
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

        /// <summary>
        /// Draws one horizontal grid line.
        /// </summary>
        /// <param name="canvas">The canvas where the line should be drawn.</param>
        /// <param name="y">The Y coordinate of the horizontal line.</param>
        /// <param name="width">The canvas width.</param>
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

        /// <summary>
        /// Draws labels for the real axis, imaginary axis, and zero point.
        /// </summary>
        /// <param name="canvas">The canvas where the labels should be drawn.</param>
        /// <param name="centerX">The X coordinate of the canvas center.</param>
        /// <param name="centerY">The Y coordinate of the canvas center.</param>
        /// <param name="width">The canvas width.</param>
        /// <param name="height">The canvas height.</param>
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

        /// <summary>
        /// Draws the arrow head at the end of a vector.
        /// </summary>
        /// <param name="canvas">The canvas where the arrow head should be drawn.</param>
        /// <param name="startX">The X coordinate of the vector start.</param>
        /// <param name="startY">The Y coordinate of the vector start.</param>
        /// <param name="endX">The X coordinate of the vector end.</param>
        /// <param name="endY">The Y coordinate of the vector end.</param>
        private void DrawArrowHead(Canvas canvas, double startX, double startY, double endX, double endY)
        {
            // Calculate the angle of the vector.
            double angle = Math.Atan2(endY - startY, endX - startX);

            // Calculate two short lines that create the arrow head.
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

        /// <summary>
        /// Calculates a scale that keeps the vector inside the canvas.
        /// </summary>
        /// <param name="canvas">The canvas where the vector is drawn.</param>
        /// <param name="number">The complex number represented by the vector.</param>
        /// <returns>The scale used to convert number values to canvas coordinates.</returns>
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

        /// <summary>
        /// Checks if the canvas is not null.
        /// </summary>
        /// <param name="canvas">The canvas to validate.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the canvas is null.
        /// </exception>
        private void ValidateCanvas(Canvas canvas)
        {
            if (canvas is null)
            {
                throw new ArgumentNullException(nameof(canvas));
            }
        }
    }
}
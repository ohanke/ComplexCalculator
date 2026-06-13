using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using ComplexCalculator.Models;
using ComplexCalculator.Utils;

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

        private const double AxisLabelEdgeOffset = 30;
        private const double AxisLabelOffset = 5;

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

            // Get width and height of the canvas
            double width = canvas.Width;
            double height = canvas.Height;

            // Get canvas center
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

            // Get width and height of the canvas
            double width = canvas.Width;
            double height = canvas.Height;

            // Get canvas center (vector start point)
            double centerX = width / 2;
            double centerY = height / 2;

            // Calculate drawing scale to keep the vector inside the canvas.
            double scale = CalculateScale(canvas, number);

            // Calculate the end point of the vector based on the complex number values and the scale.
            double endX = centerX + number.Real * scale;
            double endY = centerY - number.Imaginary * scale; // In WPF the Y coordinate increases downwards instead of upwards, thats why the imaginary part is subtracted.

            // Create vector line from the center to the calculated end point.
            Line vectorLine = new Line
            {
                X1 = centerX,
                Y1 = centerY,
                X2 = endX,
                Y2 = endY,
                Stroke = Brushes.Red,
                StrokeThickness = VectorStrokeThickness
            };

            // Create a point at the end of the vector to represent the complex number.
            Ellipse point = new Ellipse
            {
                Width = PointSize,
                Height = PointSize,
                Fill = Brushes.Red
            };

            // Adjust the position of the point so that it is centered at the end of the vector - otherwise the point would be positioned at its top-left corner.
            Canvas.SetLeft(point, endX - PointSize / 2);
            Canvas.SetTop(point, endY - PointSize / 2);

            // Create a label with the value of the complex number.
            TextBlock pointLabel = new TextBlock
            {
                Text = $"z = {number}",
                Foreground = Brushes.Red,
                FontWeight = FontWeights.Bold
            };

            // Adjust the position of the label so that it is placed slightly above and to the right of the end of the vector.
            Canvas.SetLeft(pointLabel, endX + 8);
            Canvas.SetTop(pointLabel, endY - 20);

            canvas.Children.Add(vectorLine);

            // Vector without length (0 + 0i) does not need an arrow head.
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
            // horizontal Re axis
            Line realAxis = new Line
            {
                X1 = 0,
                Y1 = centerY,
                X2 = width,
                Y2 = centerY,
                Stroke = Brushes.Black,
                StrokeThickness = AxisStrokeThickness
            };

            // vertical Im axis
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
            // Draw vertical grid line to the right of the center.
            for (double x = centerX + DefaultScale; x < width; x += DefaultScale)
            {
                DrawVerticalGridLine(canvas, x, height);
            }

            // Draw vertical grid line to the left of the center.
            for (double x = centerX - DefaultScale; x > 0; x -= DefaultScale)
            {
                DrawVerticalGridLine(canvas, x, height);
            }

            // Draw horizontal grid line above the center.
            for (double y = centerY + DefaultScale; y < height; y += DefaultScale)
            {
                DrawHorizontalGridLine(canvas, y, width);
            }

            // Draw horizontal grid line below the center.
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
            // Draw vertical grid line at the given X coordinate, from top to bottom of the canvas.
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
            // Draw horizontal grid line at the given Y coordinate, from left to right of the canvas.
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
            // Create label for the real axis (Re)
            TextBlock realLabel = new TextBlock
            {
                Text = Constants.RealPartLabel,
                FontWeight = FontWeights.Bold
            };

            // Adjust the position of the real axis label so that it is placed slightly below and to the right of the end of the real axis.
            Canvas.SetLeft(realLabel, width - AxisLabelEdgeOffset);
            Canvas.SetTop(realLabel, centerY + AxisLabelOffset);
            canvas.Children.Add(realLabel);

            // Create label for the imaginary axis (Im)
            TextBlock imaginaryLabel = new TextBlock
            {
                Text = Constants.ImaginaryPartLabel,
                FontWeight = FontWeights.Bold
            };

            Canvas.SetLeft(imaginaryLabel, centerX + AxisLabelOffset);
            Canvas.SetTop(imaginaryLabel, AxisLabelOffset);
            canvas.Children.Add(imaginaryLabel);

            // Create label for the zero point (0)
            TextBlock zeroLabel = new TextBlock
            {
                Text = Constants.ZeroLabel,
            };

            Canvas.SetLeft(zeroLabel, centerX + AxisLabelOffset);
            Canvas.SetTop(zeroLabel, centerY + AxisLabelOffset);
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

            // Create two lines for the arrow head and add them to the canvas.
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

            // Calculate the maximum radius that can fit in the canvas, considering the margin.
            double maxCanvasRadius = Math.Min(canvas.Width, canvas.Height) / 2 - CanvasMargin;

            // Calculate the scale that fits the vector inside the canvas.
            double scaleThatFitsCanvas = maxCanvasRadius / maxCoordinate;

            // Use the default scale for small values or reduced it for large values.
            return Math.Min(DefaultScale, scaleThatFitsCanvas);
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
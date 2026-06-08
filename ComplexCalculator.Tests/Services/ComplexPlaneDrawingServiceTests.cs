using System.Runtime.ExceptionServices;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using ComplexCalculator.Models;
using ComplexCalculator.Services;

namespace ComplexCalculator.Tests.Services
{
    public class ComplexPlaneDrawingServiceTests
    {
        [Fact]
        public void DrawPlane_WhenCanvasIsNull_ShouldThrowArgumentNullException()
        {
            //WTF requires STA thread for testing WPF components, so we need to run the test in a separate thread with STA apartment state
            RunInStaThread(() =>
            {
                // Arrange
                ComplexPlaneDrawingService drawingService = new ComplexPlaneDrawingService();
                Canvas canvas = null!;

                // Act
                Action action = () => drawingService.DrawPlane(canvas);

                // Assert
                Assert.Throws<ArgumentNullException>(action);
            });
        }

        [Fact]
        public void DrawPlane_WhenCanvasIsValid_ShouldClearExistingElements()
        {
            RunInStaThread(() =>
            {
                // Arrange
                ComplexPlaneDrawingService drawingService = new ComplexPlaneDrawingService();
                Canvas canvas = CreateCanvas();

                canvas.Children.Add(new TextBlock
                {
                    Text = "Old element"
                });

                // Act
                drawingService.DrawPlane(canvas);

                // Assert
                Assert.DoesNotContain(
                    canvas.Children.OfType<TextBlock>(),
                    textBlock => textBlock.Text == "Old element"
                );
            });
        }

        [Fact]
        public void DrawPlane_WhenCanvasIsValid_ShouldDrawAxisLabels()
        {
            RunInStaThread(() =>
            {
                // Arrange
                ComplexPlaneDrawingService drawingService = new ComplexPlaneDrawingService();
                Canvas canvas = CreateCanvas();

                // Act
                drawingService.DrawPlane(canvas);

                // Assert
                Assert.Contains(
                    canvas.Children.OfType<TextBlock>(),
                    textBlock => textBlock.Text == "Re"
                );

                Assert.Contains(
                    canvas.Children.OfType<TextBlock>(),
                    textBlock => textBlock.Text == "Im"
                );

                Assert.Contains(
                    canvas.Children.OfType<TextBlock>(),
                    textBlock => textBlock.Text == "0"
                );
            });
        }

        [Fact]
        public void DrawPlane_WhenCanvasIsValid_ShouldDrawTwoBlackAxisLines()
        {
            RunInStaThread(() =>
            {
                // Arrange
                ComplexPlaneDrawingService drawingService = new ComplexPlaneDrawingService();
                Canvas canvas = CreateCanvas();

                // Act
                drawingService.DrawPlane(canvas);

                // Assert
                List<Line> blackLines = canvas.Children
                    .OfType<Line>()
                    .Where(line => line.Stroke == Brushes.Black)
                    .ToList();

                Assert.Equal(2, blackLines.Count);
            });
        }

        [Fact]
        public void DrawVector_WhenCanvasIsNull_ShouldThrowArgumentNullException()
        {
            RunInStaThread(() =>
            {
                // Arrange
                ComplexPlaneDrawingService drawingService = new ComplexPlaneDrawingService();
                Canvas canvas = null!;
                ComplexNumber number = new ComplexNumber(1, 2);

                // Act
                Action action = () => drawingService.DrawVector(canvas, number);

                // Assert
                Assert.Throws<ArgumentNullException>(action);
            });
        }

        [Fact]
        public void DrawVector_WhenNumberIsNull_ShouldThrowArgumentNullException()
        {
            RunInStaThread(() =>
            {
                // Arrange
                ComplexPlaneDrawingService drawingService = new ComplexPlaneDrawingService();
                Canvas canvas = CreateCanvas();
                ComplexNumber number = null!;

                // Act
                Action action = () => drawingService.DrawVector(canvas, number);

                // Assert
                Assert.Throws<ArgumentNullException>(action);
            });
        }

        [Fact]
        public void DrawVector_WhenNumberIsNotZero_ShouldDrawVectorPointLabelAndArrowHead()
        {
            RunInStaThread(() =>
            {
                // Arrange
                ComplexPlaneDrawingService drawingService = new ComplexPlaneDrawingService();
                Canvas canvas = CreateCanvas();
                ComplexNumber number = new ComplexNumber(2, 3);

                // Act
                drawingService.DrawVector(canvas, number);

                // Assert
                List<Line> redLines = canvas.Children
                    .OfType<Line>()
                    .Where(line => line.Stroke == Brushes.Red)
                    .ToList();

                List<Ellipse> redPoints = canvas.Children
                    .OfType<Ellipse>()
                    .Where(ellipse => ellipse.Fill == Brushes.Red)
                    .ToList();

                Assert.Equal(3, redLines.Count);
                Assert.Single(redPoints);

                Assert.Contains(
                    canvas.Children.OfType<TextBlock>(),
                    textBlock => textBlock.Text == "z = 2 + 3i"
                );
            });
        }

        [Fact]
        public void DrawVector_WhenNumberIsZero_ShouldDrawVectorPointAndLabelWithoutArrowHead()
        {
            RunInStaThread(() =>
            {
                // Arrange
                ComplexPlaneDrawingService drawingService = new ComplexPlaneDrawingService();
                Canvas canvas = CreateCanvas();
                ComplexNumber number = new ComplexNumber(0, 0);

                // Act
                drawingService.DrawVector(canvas, number);

                // Assert
                List<Line> redLines = canvas.Children
                    .OfType<Line>()
                    .Where(line => line.Stroke == Brushes.Red)
                    .ToList();

                List<Ellipse> redPoints = canvas.Children
                    .OfType<Ellipse>()
                    .Where(ellipse => ellipse.Fill == Brushes.Red)
                    .ToList();

                Assert.Single(redLines);
                Assert.Single(redPoints);

                Assert.Contains(
                    canvas.Children.OfType<TextBlock>(),
                    textBlock => textBlock.Text == "z = 0 + 0i"
                );
            });
        }

        [Fact]
        public void DrawVector_WhenNumberHasPositiveRealAndPositiveImaginaryPart_ShouldDrawVectorToRightAndUp()
        {
            RunInStaThread(() =>
            {
                // Arrange
                ComplexPlaneDrawingService drawingService = new ComplexPlaneDrawingService();
                Canvas canvas = CreateCanvas();
                ComplexNumber number = new ComplexNumber(2, 3);

                double centerX = canvas.Width / 2;
                double centerY = canvas.Height / 2;

                // Act
                drawingService.DrawVector(canvas, number);

                // Assert
                Line vectorLine = canvas.Children
                    .OfType<Line>()
                    .First(line => line.Stroke == Brushes.Red);

                Assert.Equal(centerX, vectorLine.X1);
                Assert.Equal(centerY, vectorLine.Y1);

                Assert.True(vectorLine.X2 > centerX);
                Assert.True(vectorLine.Y2 < centerY);
            });
        }

        [Fact]
        public void DrawVector_WhenNumberHasNegativeRealAndNegativeImaginaryPart_ShouldDrawVectorToLeftAndDown()
        {
            RunInStaThread(() =>
            {
                // Arrange
                ComplexPlaneDrawingService drawingService = new ComplexPlaneDrawingService();
                Canvas canvas = CreateCanvas();
                ComplexNumber number = new ComplexNumber(-2, -3);

                double centerX = canvas.Width / 2;
                double centerY = canvas.Height / 2;

                // Act
                drawingService.DrawVector(canvas, number);

                // Assert
                Line vectorLine = canvas.Children
                    .OfType<Line>()
                    .First(line => line.Stroke == Brushes.Red);

                Assert.Equal(centerX, vectorLine.X1);
                Assert.Equal(centerY, vectorLine.Y1);

                Assert.True(vectorLine.X2 < centerX);
                Assert.True(vectorLine.Y2 > centerY);
            });
        }

        [Fact]
        public void DrawVector_WhenNumberHasLargeValues_ShouldKeepVectorInsideCanvas()
        {
            RunInStaThread(() =>
            {
                // Arrange
                ComplexPlaneDrawingService drawingService = new ComplexPlaneDrawingService();
                Canvas canvas = CreateCanvas();
                ComplexNumber number = new ComplexNumber(100, 50);

                // Act
                drawingService.DrawVector(canvas, number);

                // Assert
                Line vectorLine = canvas.Children
                    .OfType<Line>()
                    .First(line => line.Stroke == Brushes.Red);

                Assert.True(vectorLine.X2 >= 0);
                Assert.True(vectorLine.X2 <= canvas.Width);
                Assert.True(vectorLine.Y2 >= 0);
                Assert.True(vectorLine.Y2 <= canvas.Height);
            });
        }

        private static Canvas CreateCanvas()
        {
            return new Canvas
            {
                Width = 480,
                Height = 480
            };
        }

        private static void RunInStaThread(Action action)
        {
            Exception? exception = null;

            Thread thread = new Thread(() =>
            {
                try
                {
                    action();
                }
                catch (Exception caughtException)
                {
                    exception = caughtException;
                }
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();

            if (exception is not null)
            {
                ExceptionDispatchInfo.Capture(exception).Throw();
            }
        }
    }
}
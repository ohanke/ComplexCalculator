using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using ComplexCalculator.Models;
using ComplexCalculator.Services;
using ComplexCalculator.Operations;
using ComplexCalculator.Utils;

namespace ComplexCalculator
{
    public partial class MainWindow : Window
    {
        private const string FirstNumberDescription = "pierwszej liczby";
        private const string SecondNumberDescription = "drugiej liczby";

        private readonly ComplexCalculatorService _calculatorService;
        private readonly ComplexPlaneDrawingService _complexPlaneDrawingService;

        public MainWindow()
        {
            InitializeComponent();

            ArgumentValidator argumentValidator = new ArgumentValidator();

            List<IComplexOperation> operations = new List<IComplexOperation>
    {
        new AddOperation(argumentValidator),
        new SubtractOperation(argumentValidator),
        new MultiplyOperation(argumentValidator),
        new DivideOperation(argumentValidator)
    };

            _calculatorService = new ComplexCalculatorService(operations, argumentValidator);
            _complexPlaneDrawingService = new ComplexPlaneDrawingService();

            _complexPlaneDrawingService.DrawPlane(ComplexPlaneCanvas);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Calculate(Constants.PlusSign);
        }

        private void SubtractButton_Click(object sender, RoutedEventArgs e)
        {
            Calculate(Constants.MinusSign);
        }

        private void MultiplyButton_Click(object sender, RoutedEventArgs e)
        {
            Calculate(Constants.MultiplySign);
        }

        private void DivideButton_Click(object sender, RoutedEventArgs e)
        {
            Calculate(Constants.DivideSign);
        }

        private void Calculate(string operation)
        {
            try
            {
                ClearError();

                ComplexNumber firstNumber = ReadComplexNumber(
                    FirstRealTextBox,
                    FirstImaginaryTextBox,
                    FirstNumberDescription
                );

                ComplexNumber secondNumber = ReadComplexNumber(
                    SecondRealTextBox,
                    SecondImaginaryTextBox,
                    SecondNumberDescription
                );

                ComplexNumber result = _calculatorService.Calculate(
                    firstNumber,
                    secondNumber,
                    operation
                );

                ResultTextBlock.Text = $"{firstNumber} {operation} ({secondNumber}) = {result}";

                _complexPlaneDrawingService.DrawPlane(ComplexPlaneCanvas);
                _complexPlaneDrawingService.DrawVector(ComplexPlaneCanvas, result);
            }
            catch (FormatException exception)
            {
                ShowError(exception.Message);
            }
            catch (DivideByZeroException exception)
            {
                ShowError(exception.Message);
            }
            catch (InvalidOperationException exception)
            {
                ShowError(exception.Message);
            }
            catch (ArgumentNullException exception)
            {
                ShowError(exception.Message);
            }
        }

        private ComplexNumber CalculateResult(
            ComplexNumber firstNumber,
            ComplexNumber secondNumber,
            string operation)
        {
            return _calculatorService.Calculate(firstNumber, secondNumber, operation);
        }

        private ComplexNumber ReadComplexNumber(
            TextBox realTextBox,
            TextBox imaginaryTextBox,
            string numberDescription)
        {
            double real = ParseInput(realTextBox, $"część rzeczywista {numberDescription}");
            double imaginary = ParseInput(imaginaryTextBox, $"część urojona {numberDescription}");

            return new ComplexNumber(real, imaginary);
        }

        private double ParseInput(TextBox textBox, string fieldName)
        {
            string text = textBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(text))
            {
                throw new FormatException($"Pole '{fieldName}' nie może być puste.");
            }

            text = text.Replace(Constants.CommaSeparator, Constants.DotSeparator);

            bool isValidNumber = double.TryParse(
                text,
                NumberStyles.Float,
                CultureInfo.InvariantCulture,
                out double value
            );

            if (!isValidNumber)
            {
                throw new FormatException($"Pole '{fieldName}' zawiera niepoprawną wartość liczbową.");
            }

            return value;
        }

        private void ShowError(string message)
        {
            ErrorTextBlock.Text = message;
            ResultTextBlock.Text = "Nie można wykonać obliczenia.";

            _complexPlaneDrawingService.DrawPlane(ComplexPlaneCanvas);
        }

        private void ClearError()
        {
            ErrorTextBlock.Text = string.Empty;
        }
    }
}
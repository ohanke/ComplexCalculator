using ComplexCalculator.Models;
using ComplexCalculator.Operations;
using ComplexCalculator.Persistence;
using ComplexCalculator.Services;
using ComplexCalculator.Utils;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace ComplexCalculator
{
    public partial class MainWindow : Window
    {
        private const string FirstNumberDescription = "pierwszej liczby";
        private const string SecondNumberDescription = "drugiej liczby";

        private readonly ComplexCalculatorService _calculatorService;
        private readonly ComplexPlaneDrawingService _complexPlaneDrawingService;
        private readonly CalculationHistoryRepository _calculationHistoryRepository;
        private readonly CalculationResultFileService _calculationResultFileService;

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
            _calculationHistoryRepository = new CalculationHistoryRepository();
            _calculationResultFileService = new CalculationResultFileService();

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

        private void MagnitudeButton_Click(object sender, RoutedEventArgs e)
        {
            CalculateMagnitude();
        }

        private void ConjugateButton_Click(object sender, RoutedEventArgs e)
        {
            CalculateConjugate();
        }

        private void TrigonometricFormButton_Click(object sender, RoutedEventArgs e)
        {
            ShowTrigonometricForm();
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

                ComplexNumber result = _calculatorService.Calculate
                    (
                    firstNumber,
                    secondNumber,
                    operation
                );

                _calculationHistoryRepository.Save(firstNumber, secondNumber, operation, result);
                SaveBinaryCalculationResult(firstNumber, secondNumber, operation, result);

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

        private void CalculateMagnitude()
        {
            try
            {
                ComplexNumber number = ReadFirstComplexNumber();

                double magnitude = number.Magnitude();
                string result = $"|z₁| = {magnitude:0.##}";

                ResultTextBlock.Text = result;
                SaveUnaryCalculationResult(number, "|z₁|", result);

                _complexPlaneDrawingService.DrawPlane(ComplexPlaneCanvas);
                _complexPlaneDrawingService.DrawVector(ComplexPlaneCanvas, number);
            }
            catch (Exception exception)
            {
                ResultTextBlock.Text = exception.Message;
            }
        }

        private void CalculateConjugate()
        {
            try
            {
                ComplexNumber number = ReadFirstComplexNumber();

                ComplexNumber conjugate = number.Conjugate();
                string result = $"conj(z₁) = {conjugate}";

                ResultTextBlock.Text = result;
                SaveUnaryCalculationResult(number, "conj(z₁)", result);

                _complexPlaneDrawingService.DrawPlane(ComplexPlaneCanvas);
                _complexPlaneDrawingService.DrawVector(ComplexPlaneCanvas, conjugate);
            }
            catch (Exception exception)
            {
                ResultTextBlock.Text = exception.Message;
            }
        }

        private void ShowTrigonometricForm()
        {
            try
            {
                ComplexNumber number = ReadFirstComplexNumber();

                TrigonometricForm trigonometricForm = number.ToTrigonometricForm();
                string result = $"z₁ = {trigonometricForm}";

                ResultTextBlock.Text = result;
                SaveUnaryCalculationResult(number, "trig(z₁)", result);

                _complexPlaneDrawingService.DrawPlane(ComplexPlaneCanvas);
                _complexPlaneDrawingService.DrawVector(ComplexPlaneCanvas, number);
            }
            catch (Exception exception)
            {
                ResultTextBlock.Text = exception.Message;
            }
        }

        private ComplexNumber ReadFirstComplexNumber()
        {
            return ReadComplexNumber(
                FirstRealTextBox,
                FirstImaginaryTextBox,
                "first number"
            );
        }

        private void SaveBinaryCalculationResult(
            ComplexNumber firstNumber,
            ComplexNumber secondNumber,
            string operation,
            ComplexNumber result)
        {
            CalculationResultFileEntry entry = new CalculationResultFileEntry
            {
                CreatedAt = DateTime.Now,
                OperationType = "Binary",
                Operation = operation,
                FirstNumber = firstNumber.ToString(),
                SecondNumber = secondNumber.ToString(),
                Result = result.ToString()
            };

            _calculationResultFileService.Save(entry);
        }

        private void SaveUnaryCalculationResult(
            ComplexNumber number,
            string operation,
            string result)
        {
            CalculationResultFileEntry entry = new CalculationResultFileEntry
            {
                CreatedAt = DateTime.Now,
                OperationType = "Unary",
                Operation = operation,
                FirstNumber = number.ToString(),
                SecondNumber = null,
                Result = result
            };

            _calculationResultFileService.Save(entry);
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
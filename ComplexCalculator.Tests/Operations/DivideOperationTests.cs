using ComplexCalculator.Models;
using ComplexCalculator.Operations;
using ComplexCalculator.Services;

namespace ComplexCalculator.Tests.Operations
{
    public class DivideOperationTests
    {
        private readonly ArgumentValidator _argumentValidator;

        public DivideOperationTests()
        {
            _argumentValidator = new ArgumentValidator();
        }

        [Fact]
        public void Symbol_ShouldReturnDivideSign()
        {
            // Arrange
            DivideOperation operation = new DivideOperation(_argumentValidator);

            // Act
            string symbol = operation.Symbol;

            // Assert
            Assert.Equal("/", symbol);
        }

        [Fact]
        public void Execute_WhenArgumentsAreValid_ShouldReturnQuotient()
        {
            // Arrange
            DivideOperation operation = new DivideOperation(_argumentValidator);

            ComplexNumber first = new ComplexNumber(3, 4);
            ComplexNumber second = new ComplexNumber(2, 1);

            // Act
            ComplexNumber result = operation.Execute(first, second);

            // Assert
            Assert.Equal(2, result.Real);
            Assert.Equal(1, result.Imaginary);
        }

        [Fact]
        public void Execute_WhenResultHasDecimalValues_ShouldReturnCorrectQuotient()
        {
            // Arrange
            DivideOperation operation = new DivideOperation(_argumentValidator);

            ComplexNumber first = new ComplexNumber(1, 2);
            ComplexNumber second = new ComplexNumber(3, 4);

            // Act
            ComplexNumber result = operation.Execute(first, second);

            // Assert
            Assert.Equal(0.44, result.Real, precision: 2);
            Assert.Equal(0.08, result.Imaginary, precision: 2);
        }

        [Fact]
        public void Execute_WhenFirstArgumentIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            DivideOperation operation = new DivideOperation(_argumentValidator);

            ComplexNumber first = null!;
            ComplexNumber second = new ComplexNumber(2, 1);

            // Act
            Action action = () => operation.Execute(first, second);

            // Assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void Execute_WhenSecondArgumentIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            DivideOperation operation = new DivideOperation(_argumentValidator);

            ComplexNumber first = new ComplexNumber(3, 4);
            ComplexNumber second = null!;

            // Act
            Action action = () => operation.Execute(first, second);

            // Assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void Execute_WhenSecondArgumentIsZero_ShouldThrowDivideByZeroException()
        {
            // Arrange
            DivideOperation operation = new DivideOperation(_argumentValidator);

            ComplexNumber first = new ComplexNumber(3, 4);
            ComplexNumber second = new ComplexNumber(0, 0);

            // Act
            Action action = () => operation.Execute(first, second);

            // Assert
            Assert.Throws<DivideByZeroException>(action);
        }
    }
}
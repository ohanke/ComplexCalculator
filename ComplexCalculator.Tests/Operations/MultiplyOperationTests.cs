using ComplexCalculator.Models;
using ComplexCalculator.Operations;
using ComplexCalculator.Services;

namespace ComplexCalculator.Tests.Operations
{
    public class MultiplyOperationTests
    {
        private readonly ArgumentValidator _argumentValidator;

        public MultiplyOperationTests()
        {
            _argumentValidator = new ArgumentValidator();
        }

        [Fact]
        public void Symbol_ShouldReturnMultiplySign()
        {
            // Arrange
            MultiplyOperation operation = new MultiplyOperation(_argumentValidator);

            // Act
            string symbol = operation.Symbol;

            // Assert
            Assert.Equal("*", symbol);
        }

        [Fact]
        public void Execute_WhenArgumentsAreValid_ShouldReturnProduct()
        {
            // Arrange
            MultiplyOperation operation = new MultiplyOperation(_argumentValidator);

            ComplexNumber first = new ComplexNumber(3, 4);
            ComplexNumber second = new ComplexNumber(2, 1);

            // Act
            ComplexNumber result = operation.Execute(first, second);

            // Assert
            Assert.Equal(2, result.Real);
            Assert.Equal(11, result.Imaginary);
        }

        [Fact]
        public void Execute_WhenFirstArgumentIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            MultiplyOperation operation = new MultiplyOperation(_argumentValidator);

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
            MultiplyOperation operation = new MultiplyOperation(_argumentValidator);

            ComplexNumber first = new ComplexNumber(3, 4);
            ComplexNumber second = null!;

            // Act
            Action action = () => operation.Execute(first, second);

            // Assert
            Assert.Throws<ArgumentNullException>(action);
        }
    }
}
using ComplexCalculator.Models;
using ComplexCalculator.Services;

namespace ComplexCalculator.Tests.Services
{
    public class ArgumentValidatorTests
    {
        private readonly ArgumentValidator _argumentValidator;

        public ArgumentValidatorTests()
        {
            _argumentValidator = new ArgumentValidator();
        }

        [Fact]
        public void ValidateComplexNumber_WhenNumberIsNotNull_ShouldNotThrowException()
        {
            // Arrange
            ComplexNumber number = new ComplexNumber(1, 2);

            // Act
            Action action = () => _argumentValidator.ValidateComplexNumber(number);

            // Assert
            Exception? exception = Record.Exception(action);
            Assert.Null(exception);
        }

        [Fact]
        public void ValidateComplexNumber_WhenNumberIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            ComplexNumber number = null!;

            // Act
            Action action = () => _argumentValidator.ValidateComplexNumber(number);

            // Assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void ValidateComplexNumbers_WhenBothNumbersAreNotNull_ShouldNotThrowException()
        {
            // Arrange
            ComplexNumber first = new ComplexNumber(1, 2);
            ComplexNumber second = new ComplexNumber(3, 4);

            // Act
            Action action = () => _argumentValidator.ValidateComplexNumbers(first, second);

            // Assert
            Exception? exception = Record.Exception(action);
            Assert.Null(exception);
        }

        [Fact]
        public void ValidateComplexNumbers_WhenFirstNumberIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            ComplexNumber first = null!;
            ComplexNumber second = new ComplexNumber(3, 4);

            // Act
            Action action = () => _argumentValidator.ValidateComplexNumbers(first, second);

            // Assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void ValidateComplexNumbers_WhenSecondNumberIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            ComplexNumber first = new ComplexNumber(1, 2);
            ComplexNumber second = null!;

            // Act
            Action action = () => _argumentValidator.ValidateComplexNumbers(first, second);

            // Assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void ValidateDivisorIsNotZero_WhenDivisorIsNotZero_ShouldNotThrowException()
        {
            // Arrange
            ComplexNumber divisor = new ComplexNumber(1, 2);

            // Act
            Action action = () => _argumentValidator.ValidateDivisorIsNotZero(divisor);

            // Assert
            Exception? exception = Record.Exception(action);
            Assert.Null(exception);
        }

        [Fact]
        public void ValidateDivisorIsNotZero_WhenDivisorIsZero_ShouldThrowDivideByZeroException()
        {
            // Arrange
            ComplexNumber divisor = new ComplexNumber(0, 0);

            // Act
            Action action = () => _argumentValidator.ValidateDivisorIsNotZero(divisor);

            // Assert
            Assert.Throws<DivideByZeroException>(action);
        }

        [Fact]
        public void ValidateNotNullOrBlank_WhenValueIsValid_ShouldNotThrowException()
        {
            // Arrange
            string value = "valid value";

            // Act
            Action action = () => _argumentValidator.ValidateNotNullOrBlank(value);

            // Assert
            Exception? exception = Record.Exception(action);
            Assert.Null(exception);
        }

        [Fact]
        public void ValidateNotNullOrBlank_WhenValueIsNull_ShouldThrowArgumentException()
        {
            // Arrange
            string value = null!;

            // Act
            Action action = () => _argumentValidator.ValidateNotNullOrBlank(value);

            // Assert
            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void ValidateNotNullOrBlank_WhenValueIsEmpty_ShouldThrowArgumentException()
        {
            // Arrange
            string value = string.Empty;

            // Act
            Action action = () => _argumentValidator.ValidateNotNullOrBlank(value);

            // Assert
            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void ValidateNotNullOrBlank_WhenValueIsWhitespace_ShouldThrowArgumentException()
        {
            // Arrange
            string value = "   ";

            // Act
            Action action = () => _argumentValidator.ValidateNotNullOrBlank(value);

            // Assert
            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void ValidateNotNull_WhenValueIsNotNull_ShouldNotThrowException()
        {
            // Arrange
            object value = new object();

            // Act
            Action action = () => _argumentValidator.ValidateNotNull(value);

            // Assert
            Exception? exception = Record.Exception(action);
            Assert.Null(exception);
        }

        [Fact]
        public void ValidateNotNull_WhenValueIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            object value = null!;

            // Act
            Action action = () => _argumentValidator.ValidateNotNull(value);

            // Assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void ValidateDivisorIsNotZero_WhenDivisorIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            ComplexNumber divisor = null!;

            // Act
            Action action = () => _argumentValidator.ValidateDivisorIsNotZero(divisor);

            // Assert
            Assert.Throws<ArgumentNullException>(action);
        }
    }
}
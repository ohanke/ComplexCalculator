using ComplexCalculator.Models;
using ComplexCalculator.Operations;
using ComplexCalculator.Services;

namespace ComplexCalculator.Tests.Services
{
    public class ComplexCalculatorServiceTests
    {
        private readonly ArgumentValidator _argumentValidator;

        public ComplexCalculatorServiceTests()
        {
            _argumentValidator = new ArgumentValidator();
        }

        [Fact]
        public void Constructor_WhenArgumentValidatorIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            IEnumerable<IComplexOperation> operations = new List<IComplexOperation>
            {
                new TestComplexOperation("+", new ComplexNumber(1, 1))
            };

            ArgumentValidator argumentValidator = null!;

            // Act
            Action action = () => new ComplexCalculatorService(operations, argumentValidator);

            // Assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void Constructor_WhenOperationsAreNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            IEnumerable<IComplexOperation> operations = null!;

            // Act
            Action action = () => new ComplexCalculatorService(operations, _argumentValidator);

            // Assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void Constructor_WhenOperationsAreValid_ShouldCreateService()
        {
            // Arrange
            IEnumerable<IComplexOperation> operations = new List<IComplexOperation>
            {
                new TestComplexOperation("+", new ComplexNumber(1, 1))
            };

            // Act
            ComplexCalculatorService service = new ComplexCalculatorService(
                operations,
                _argumentValidator
            );

            // Assert
            Assert.NotNull(service);
        }

        [Fact]
        public void Calculate_WhenOperationSymbolExists_ShouldReturnOperationResult()
        {
            // Arrange
            ComplexNumber expectedResult = new ComplexNumber(5, 7);

            IEnumerable<IComplexOperation> operations = new List<IComplexOperation>
            {
                new TestComplexOperation("+", expectedResult)
            };

            ComplexCalculatorService service = new ComplexCalculatorService(
                operations,
                _argumentValidator
            );

            ComplexNumber first = new ComplexNumber(1, 2);
            ComplexNumber second = new ComplexNumber(4, 5);

            // Act
            ComplexNumber result = service.Calculate(first, second, "+");

            // Assert
            Assert.Same(expectedResult, result);
        }

        [Fact]
        public void Calculate_WhenOperationSymbolExists_ShouldPassArgumentsToOperation()
        {
            // Arrange
            ComplexNumber operationResult = new ComplexNumber(5, 7);
            TestComplexOperation operation = new TestComplexOperation("+", operationResult);

            IEnumerable<IComplexOperation> operations = new List<IComplexOperation>
            {
                operation
            };

            ComplexCalculatorService service = new ComplexCalculatorService(
                operations,
                _argumentValidator
            );

            ComplexNumber first = new ComplexNumber(1, 2);
            ComplexNumber second = new ComplexNumber(4, 5);

            // Act
            service.Calculate(first, second, "+");

            // Assert
            Assert.Same(first, operation.ReceivedFirst);
            Assert.Same(second, operation.ReceivedSecond);
        }

        [Fact]
        public void Calculate_WhenOperationSymbolIsNull_ShouldThrowArgumentException()
        {
            // Arrange
            IEnumerable<IComplexOperation> operations = new List<IComplexOperation>
            {
                new TestComplexOperation("+", new ComplexNumber(1, 1))
            };

            ComplexCalculatorService service = new ComplexCalculatorService(
                operations,
                _argumentValidator
            );

            ComplexNumber first = new ComplexNumber(1, 2);
            ComplexNumber second = new ComplexNumber(3, 4);
            string operationSymbol = null!;

            // Act
            Action action = () => service.Calculate(first, second, operationSymbol);

            // Assert
            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void Calculate_WhenOperationSymbolIsEmpty_ShouldThrowArgumentException()
        {
            // Arrange
            IEnumerable<IComplexOperation> operations = new List<IComplexOperation>
            {
                new TestComplexOperation("+", new ComplexNumber(1, 1))
            };

            ComplexCalculatorService service = new ComplexCalculatorService(
                operations,
                _argumentValidator
            );

            ComplexNumber first = new ComplexNumber(1, 2);
            ComplexNumber second = new ComplexNumber(3, 4);

            // Act
            Action action = () => service.Calculate(first, second, string.Empty);

            // Assert
            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void Calculate_WhenOperationSymbolIsWhitespace_ShouldThrowArgumentException()
        {
            // Arrange
            IEnumerable<IComplexOperation> operations = new List<IComplexOperation>
            {
                new TestComplexOperation("+", new ComplexNumber(1, 1))
            };

            ComplexCalculatorService service = new ComplexCalculatorService(
                operations,
                _argumentValidator
            );

            ComplexNumber first = new ComplexNumber(1, 2);
            ComplexNumber second = new ComplexNumber(3, 4);

            // Act
            Action action = () => service.Calculate(first, second, "   ");

            // Assert
            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void Calculate_WhenOperationSymbolDoesNotExist_ShouldThrowInvalidOperationException()
        {
            // Arrange
            IEnumerable<IComplexOperation> operations = new List<IComplexOperation>
            {
                new TestComplexOperation("+", new ComplexNumber(1, 1))
            };

            ComplexCalculatorService service = new ComplexCalculatorService(
                operations,
                _argumentValidator
            );

            ComplexNumber first = new ComplexNumber(1, 2);
            ComplexNumber second = new ComplexNumber(3, 4);

            // Act
            Action action = () => service.Calculate(first, second, "-");

            // Assert
            Assert.Throws<InvalidOperationException>(action);
        }

        [Fact]
        public void Calculate_WhenOperationSymbolDoesNotExist_ShouldThrowExceptionWithCorrectMessage()
        {
            // Arrange
            IEnumerable<IComplexOperation> operations = new List<IComplexOperation>
            {
                new TestComplexOperation("+", new ComplexNumber(1, 1))
            };

            ComplexCalculatorService service = new ComplexCalculatorService(
                operations,
                _argumentValidator
            );

            ComplexNumber first = new ComplexNumber(1, 2);
            ComplexNumber second = new ComplexNumber(3, 4);

            // Act
            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(
                () => service.Calculate(first, second, "-")
            );

            // Assert
            Assert.Equal("Unknown mathematical operation.", exception.Message);
        }

        private class TestComplexOperation : IComplexOperation
        {
            private readonly ComplexNumber _result;

            public string Symbol { get; }

            public ComplexNumber? ReceivedFirst { get; private set; }

            public ComplexNumber? ReceivedSecond { get; private set; }

            public TestComplexOperation(string symbol, ComplexNumber result)
            {
                Symbol = symbol;
                _result = result;
            }

            public ComplexNumber Execute(ComplexNumber first, ComplexNumber second)
            {
                ReceivedFirst = first;
                ReceivedSecond = second;

                return _result;
            }
        }
    }
}
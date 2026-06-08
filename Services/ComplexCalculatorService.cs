using ComplexCalculator.Models;
using ComplexCalculator.Operations;

namespace ComplexCalculator.Services
{
    /// <summary>
    /// Provides calculation logic for complex numbers.
    /// </summary>
    public class ComplexCalculatorService
    {
        private readonly ArgumentValidator _argumentValidator;
        private readonly Dictionary<string, IComplexOperation> _operations;

        public ComplexCalculatorService(
            IEnumerable<IComplexOperation> operations,
            ArgumentValidator argumentValidator)
        {
            _argumentValidator = argumentValidator
                ?? throw new ArgumentNullException(nameof(argumentValidator));

            _argumentValidator.ValidateNotNull(operations);

            _operations = operations.ToDictionary(operation => operation.Symbol);
        }

        /// <summary>
        /// Calculates the result of an operation on two complex numbers.
        /// </summary>
        /// <param name="first">The first complex number.</param>
        /// <param name="second">The second complex number.</param>
        /// <param name="operationSymbol">The symbol of the operation to execute.</param>
        /// <returns>The result of the selected operation.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the operation symbol is unknown.
        /// </exception>
        public ComplexNumber Calculate(
            ComplexNumber first,
            ComplexNumber second,
            string operationSymbol)
        {
            _argumentValidator.ValidateNotNullOrBlank(operationSymbol);

            if (!_operations.TryGetValue(operationSymbol, out IComplexOperation? operation))
            {
                throw new InvalidOperationException("Unknown mathematical operation.");
            }

            return operation.Execute(first, second);
        }
    }
}
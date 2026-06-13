using ComplexCalculator.Models;
using ComplexCalculator.Services;
using ComplexCalculator.Utils;

namespace ComplexCalculator.Operations
{
    /// <summary>
    /// Subtracts one complex number from another complex number.
    /// </summary>
    public class SubtractOperation : IComplexOperation
    {
        private readonly ArgumentValidator _argumentValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubtractOperation"/> class.
        /// </summary>
        /// <param name="argumentValidator">The validator used to check operation arguments.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the argument validator is null.
        /// </exception>
        public SubtractOperation(ArgumentValidator argumentValidator)
        {
            _argumentValidator = argumentValidator
                ?? throw new ArgumentNullException(nameof(argumentValidator));
        }

        /// <summary>
        /// Gets the symbol used for subtraction.
        /// </summary>
        public string Symbol => Constants.MinusSign;

        /// <summary>
        /// Subtracts the second complex number from the first complex number.
        /// </summary>
        /// <param name="first">The complex number to subtract from.</param>
        /// <param name="second">The complex number to subtract.</param>
        /// <returns>The difference between the two complex numbers.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when one of the complex numbers is null.
        /// </exception>
        public ComplexNumber Execute(ComplexNumber first, ComplexNumber second)
        {
            _argumentValidator.ValidateComplexNumbers(first, second);

            return new ComplexNumber(
                first.Real - second.Real,
                first.Imaginary - second.Imaginary
            );
        }
    }
}
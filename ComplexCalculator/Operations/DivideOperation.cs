using ComplexCalculator.Models;
using ComplexCalculator.Services;
using ComplexCalculator.Utils;

namespace ComplexCalculator.Operations
{
    /// <summary>
    /// Divides one complex number by another complex number.
    /// </summary>
    public class DivideOperation : IComplexOperation
    {
        private readonly ArgumentValidator _argumentValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="DivideOperation"/> class.
        /// </summary>
        /// <param name="argumentValidator">The validator used to check operation arguments.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the argument validator is null.
        /// </exception>
        public DivideOperation(ArgumentValidator argumentValidator)
        {
            _argumentValidator = argumentValidator
                ?? throw new ArgumentNullException(nameof(argumentValidator));
        }

        /// <summary>
        /// Gets the symbol used for division.
        /// </summary>
        public string Symbol => Constants.DivideSign;

        /// <summary>
        /// Divides the first complex number by the second complex number.
        /// </summary>
        /// <param name="first">The complex number to divide.</param>
        /// <param name="second">The complex number used as the divisor.</param>
        /// <returns>The quotient of the two complex numbers.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when one of the complex numbers is null.
        /// </exception>
        /// <exception cref="DivideByZeroException">
        /// Thrown when the second complex number is equal to zero.
        /// </exception>
        public ComplexNumber Execute(ComplexNumber first, ComplexNumber second)
        {
            _argumentValidator.ValidateComplexNumbers(first, second);
            _argumentValidator.ValidateDivisorIsNotZero(second);

            double denominator = second.MagnitudeSquared();

            double real = (first.Real * second.Real + first.Imaginary * second.Imaginary) / denominator;
            double imaginary = (first.Imaginary * second.Real - first.Real * second.Imaginary) / denominator;

            return new ComplexNumber(real, imaginary);
        }
    }
}
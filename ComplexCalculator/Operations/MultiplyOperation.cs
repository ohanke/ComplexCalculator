using ComplexCalculator.Models;
using ComplexCalculator.Services;
using ComplexCalculator.Utils;

namespace ComplexCalculator.Operations
{
    /// <summary>
    /// Multiplies two complex numbers.
    /// </summary>
    public class MultiplyOperation : IComplexOperation
    {
        private readonly ArgumentValidator _argumentValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiplyOperation"/> class.
        /// </summary>
        /// <param name="argumentValidator">The validator used to check operation arguments.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the argument validator is null.
        /// </exception>
        public MultiplyOperation(ArgumentValidator argumentValidator)
        {
            _argumentValidator = argumentValidator
                ?? throw new ArgumentNullException(nameof(argumentValidator));
        }

        /// <summary>
        /// Gets the symbol used for multiplication.
        /// </summary>
        public string Symbol => Constants.MultiplySign;

        /// <summary>
        /// Multiplies the first complex number by the second complex number.
        /// </summary>
        /// <param name="first">The first complex number.</param>
        /// <param name="second">The second complex number.</param>
        /// <returns>The product of the two complex numbers.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when one of the complex numbers is null.
        /// </exception>
        public ComplexNumber Execute(ComplexNumber first, ComplexNumber second)
        {
            _argumentValidator.ValidateComplexNumbers(first, second);

            double real = first.Real * second.Real - first.Imaginary * second.Imaginary;
            double imaginary = first.Real * second.Imaginary + first.Imaginary * second.Real;

            return new ComplexNumber(real, imaginary);
        }
    }
}
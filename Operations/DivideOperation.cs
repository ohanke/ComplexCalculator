using ComplexCalculator.Models;
using ComplexCalculator.Services;
using ComplexCalculator.Utils;

namespace ComplexCalculator.Operations
{
    public class DivideOperation : IComplexOperation
    {
        private readonly ArgumentValidator _argumentValidator;

        public DivideOperation(ArgumentValidator argumentValidator)
        {
            _argumentValidator = argumentValidator
                ?? throw new ArgumentNullException(nameof(argumentValidator));
        }

        public string Symbol => Constants.DivideSign;

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
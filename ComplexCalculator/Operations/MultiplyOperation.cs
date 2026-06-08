using ComplexCalculator.Models;
using ComplexCalculator.Services;
using ComplexCalculator.Utils;

namespace ComplexCalculator.Operations
{
    public class MultiplyOperation : IComplexOperation
    {
        private readonly ArgumentValidator _argumentValidator;

        public MultiplyOperation(ArgumentValidator argumentValidator)
        {
            _argumentValidator = argumentValidator
                ?? throw new ArgumentNullException(nameof(argumentValidator));
        }

        public string Symbol => Constants.MultiplySign;

        public ComplexNumber Execute(ComplexNumber first, ComplexNumber second)
        {
            _argumentValidator.ValidateComplexNumbers(first, second);

            double real = first.Real * second.Real - first.Imaginary * second.Imaginary;
            double imaginary = first.Real * second.Imaginary + first.Imaginary * second.Real;

            return new ComplexNumber(real, imaginary);
        }
    }
}
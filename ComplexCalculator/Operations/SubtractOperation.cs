using ComplexCalculator.Models;
using ComplexCalculator.Services;
using ComplexCalculator.Utils;

namespace ComplexCalculator.Operations
{
    public class SubtractOperation : IComplexOperation
    {
        private readonly ArgumentValidator _argumentValidator;

        public SubtractOperation(ArgumentValidator argumentValidator)
        {
            _argumentValidator = argumentValidator
                ?? throw new ArgumentNullException(nameof(argumentValidator));
        }

        public string Symbol => Constants.MinusSign;

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
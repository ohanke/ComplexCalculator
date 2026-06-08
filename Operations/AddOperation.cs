using ComplexCalculator.Models;
using ComplexCalculator.Services;
using ComplexCalculator.Utils;

namespace ComplexCalculator.Operations
{
    public class AddOperation : IComplexOperation
    {
        private readonly ArgumentValidator _argumentValidator;

        public AddOperation(ArgumentValidator argumentValidator)
        {
            _argumentValidator = argumentValidator
                ?? throw new ArgumentNullException(nameof(argumentValidator));
        }

        public string Symbol => Constants.PlusSign;

        public ComplexNumber Execute(ComplexNumber first, ComplexNumber second)
        {
            _argumentValidator.ValidateComplexNumbers(first, second);

            return new ComplexNumber(
                first.Real + second.Real,
                first.Imaginary + second.Imaginary
            );
        }
    }
}
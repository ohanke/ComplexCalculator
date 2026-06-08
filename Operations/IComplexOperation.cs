using ComplexCalculator.Models;

namespace ComplexCalculator.Operations
{
    public interface IComplexOperation
    {
        string Symbol { get; }

        ComplexNumber Execute(ComplexNumber first, ComplexNumber second);
    }
}
using ComplexCalculator.Models;

namespace ComplexCalculator.Operations
{
    /// <summary>
    /// Defines a mathematical operation for two complex numbers.
    /// </summary>
    public interface IComplexOperation
    {
        /// <summary>
        /// Gets the symbol used to identify the operation.
        /// </summary>
        string Symbol { get; }

        /// <summary>
        /// Executes the operation on two complex numbers.
        /// </summary>
        /// <param name="first">The first complex number.</param>
        /// <param name="second">The second complex number.</param>
        /// <returns>The result of the operation.</returns>
        ComplexNumber Execute(ComplexNumber first, ComplexNumber second);
    }
}
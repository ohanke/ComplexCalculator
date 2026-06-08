using ComplexCalculator.Models;

namespace ComplexCalculator.Services
{
    /// <summary>
    /// Provides simple validation methods for calculator arguments.
    /// </summary>
    public class ArgumentValidator
    {
        /// <summary>
        /// Checks if a complex number is not null.
        /// </summary>
        /// <param name="number">The complex number to validate.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the complex number is null.
        /// </exception>
        public void ValidateComplexNumber(ComplexNumber number)
        {
            if (number is null)
            {
                throw new ArgumentNullException(
                    nameof(number),
                    "Complex number cannot be null."
                );
            }
        }

        /// <summary>
        /// Checks if both complex numbers are not null.
        /// </summary>
        /// <param name="first">The first complex number to validate.</param>
        /// <param name="second">The second complex number to validate.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when one of the complex numbers is null.
        /// </exception>
        public void ValidateComplexNumbers(ComplexNumber first, ComplexNumber second)
        {
            ValidateComplexNumber(first);
            ValidateComplexNumber(second);
        }

        /// <summary>
        /// Checks if the divisor is not null and is not equal to zero.
        /// </summary>
        /// <param name="divisor">The divisor to validate.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the divisor is null.
        /// </exception>
        /// <exception cref="DivideByZeroException">
        /// Thrown when the divisor is equal to zero.
        /// </exception>
        public void ValidateDivisorIsNotZero(ComplexNumber divisor)
        {
            ValidateComplexNumber(divisor);

            if (divisor.IsZero())
            {
                throw new DivideByZeroException("Cannot divide by complex number 0 + 0i.");
            }
        }

        /// <summary>
        /// Checks if a text value is not null, empty, or whitespace.
        /// </summary>
        /// <param name="value">The text value to validate.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when the text value is null, empty, or whitespace.
        /// </exception>
        public void ValidateNotNullOrBlank(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "Value cannot be null, empty, or whitespace.",
                    nameof(value)
                );
            }
        }

        /// <summary>
        /// Checks if a value is not null.
        /// </summary>
        /// <typeparam name="T">The type of the value to validate.</typeparam>
        /// <param name="value">The value to validate.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the value is null.
        /// </exception>
        public void ValidateNotNull<T>(T value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(
                    nameof(value),
                    "Value cannot be null."
                );
            }
        }
    }
}
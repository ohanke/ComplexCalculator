using ComplexCalculator.Utils;

namespace ComplexCalculator.Models
{
    /// <summary>
    /// Represents a complex number in algebraic form: a + bi.
    /// </summary>
    public class ComplexNumber
    {
        private const double Epsilon = 0.0000001;

        /// <summary>
        /// Gets the real part of the complex number.
        /// </summary>
        public double Real { get; }

        /// <summary>
        /// Gets the imaginary part of the complex number.
        /// </summary>
        public double Imaginary { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComplexNumber"/> class.
        /// </summary>
        /// <param name="real">The real part of the complex number.</param>
        /// <param name="imaginary">The imaginary part of the complex number.</param>
        public ComplexNumber(double real, double imaginary)
        {
            Real = real;
            Imaginary = imaginary;
        }

        /// <summary>
        /// Calculates the squared magnitude of the complex number.
        /// </summary>
        /// <returns>
        /// The squared magnitude of the complex number.
        /// </returns>
        public double MagnitudeSquared()
        {
            return Real * Real + Imaginary * Imaginary;
        }

        /// <summary>
        /// Checks whether the complex number is approximately equal to zero.
        /// </summary>
        /// <returns>
        /// True if both the real and imaginary parts are close to zero; otherwise, false.
        /// </returns>
        public bool IsZero()
        {
            return Math.Abs(Real) < Epsilon && Math.Abs(Imaginary) < Epsilon;
        }

        /// <summary>
        /// Returns the complex number as a formatted algebraic expression.
        /// </summary>
        /// <returns>
        /// The complex number in the form a + bi or a - bi.
        /// </returns>
        public override string ToString()
        {
            string sign = Imaginary >= 0 ? Constants.PlusSign : Constants.MinusSign;
            return $"{Real:0.##} {sign} {Math.Abs(Imaginary):0.##}i";
        }
    }
}
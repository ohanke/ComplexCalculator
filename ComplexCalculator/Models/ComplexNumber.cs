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
        /// Calculates the magnitude of the complex number.
        /// </summary>
        /// <returns>
        /// The magnitude of the complex number.
        /// </returns>
        public double Magnitude()
        {
            return Math.Sqrt(MagnitudeSquared());
        }

        /// <summary>
        /// Calculates the argument of the complex number in radians.
        /// </summary>
        /// <returns>
        /// The argument of the complex number in radians.
        /// </returns>
        public double ArgumentRadians()
        {
            return Math.Atan2(Imaginary, Real);
        }

        /// <summary>
        /// Calculates the argument of the complex number in degrees.
        /// </summary>
        /// <returns>
        /// The argument of the complex number in degrees.
        /// </returns>
        public double ArgumentDegrees()
        {
            return ArgumentRadians() * 180 / Math.PI;
        }

        /// <summary>
        /// Calculates the conjugate of the complex number.
        /// </summary>
        /// <returns>
        /// The conjugate of the complex number.
        /// </returns>
        public ComplexNumber Conjugate()
        {
            return new ComplexNumber(Real, -Imaginary);
        }

        /// <summary>
        /// Converts the complex number to trigonometric form.
        /// </summary>
        /// <returns>
        /// The trigonometric form of the complex number.
        /// </returns>
        public TrigonometricForm ToTrigonometricForm()
        {
            return new TrigonometricForm(Magnitude(), ArgumentRadians());
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
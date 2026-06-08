using ComplexCalculator.Utils;

namespace ComplexCalculator.Models
{
    public class ComplexNumber
    {
        private const double Epsilon = 0.0000001;

        public double Real { get; }
        public double Imaginary { get; }

        public ComplexNumber(double real, double imaginary)
        {
            Real = real;
            Imaginary = imaginary;
        }

        public double MagnitudeSquared()
        {
            return Real * Real + Imaginary * Imaginary;
        }

        public bool IsZero()
        {
            return Math.Abs(Real) < Epsilon && Math.Abs(Imaginary) < Epsilon;
        }

        public override string ToString()
        {
            string sign = Imaginary >= 0 ? Constants.PlusSign : Constants.MinusSign;
            return $"{Real:0.##} {sign} {Math.Abs(Imaginary):0.##}i";
        }
    }
}
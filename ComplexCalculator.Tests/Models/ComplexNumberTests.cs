using ComplexCalculator.Models;
using Xunit;

namespace ComplexCalculator.Tests.Models
{
    public class ComplexNumberTests
    {
        [Fact]
        public void Magnitude_WhenNumberIsThreeAndFour_ShouldReturnFive()
        {
            ComplexNumber number = new ComplexNumber(3, 4);

            double result = number.Magnitude();

            Assert.Equal(5, result, precision: 10);
        }

        [Fact]
        public void MagnitudeSquared_WhenNumberIsThreeAndFour_ShouldReturnTwentyFive()
        {
            ComplexNumber number = new ComplexNumber(3, 4);

            double result = number.MagnitudeSquared();

            Assert.Equal(25, result, precision: 10);
        }

        [Fact]
        public void ArgumentRadians_WhenNumberIsOneAndOne_ShouldReturnPiOverFour()
        {
            ComplexNumber number = new ComplexNumber(1, 1);

            double result = number.ArgumentRadians();

            Assert.Equal(Math.PI / 4, result, precision: 10);
        }

        [Fact]
        public void ArgumentDegrees_WhenNumberIsOneAndOne_ShouldReturnFortyFive()
        {
            ComplexNumber number = new ComplexNumber(1, 1);

            double result = number.ArgumentDegrees();

            Assert.Equal(45, result, precision: 10);
        }

        [Fact]
        public void Conjugate_WhenImaginaryPartIsPositive_ShouldReturnNumberWithNegativeImaginaryPart()
        {
            ComplexNumber number = new ComplexNumber(3, 4);

            ComplexNumber result = number.Conjugate();

            Assert.Equal(3, result.Real);
            Assert.Equal(-4, result.Imaginary);
        }

        [Fact]
        public void Conjugate_WhenImaginaryPartIsNegative_ShouldReturnNumberWithPositiveImaginaryPart()
        {
            ComplexNumber number = new ComplexNumber(3, -4);

            ComplexNumber result = number.Conjugate();

            Assert.Equal(3, result.Real);
            Assert.Equal(4, result.Imaginary);
        }

        [Fact]
        public void ToTrigonometricForm_WhenNumberIsThreeAndFour_ShouldReturnCorrectMagnitudeAndAngle()
        {
            ComplexNumber number = new ComplexNumber(3, 4);

            TrigonometricForm result = number.ToTrigonometricForm();

            Assert.Equal(5, result.Magnitude, precision: 10);
            Assert.Equal(Math.Atan2(4, 3), result.AngleRadians, precision: 10);
        }

        [Fact]
        public void IsZero_WhenBothPartsAreZero_ShouldReturnTrue()
        {
            ComplexNumber number = new ComplexNumber(0, 0);

            bool result = number.IsZero();

            Assert.True(result);
        }

        [Fact]
        public void IsZero_WhenNumberIsNotZero_ShouldReturnFalse()
        {
            ComplexNumber number = new ComplexNumber(1, 0);

            bool result = number.IsZero();

            Assert.False(result);
        }
    }
}
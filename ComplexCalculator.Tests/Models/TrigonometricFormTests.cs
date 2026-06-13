using ComplexCalculator.Models;
using Xunit;

namespace ComplexCalculator.Tests.Models
{
    public class TrigonometricFormTests
    {
        [Fact]
        public void AngleDegrees_WhenAngleRadiansIsPiOverTwo_ShouldReturnNinety()
        {
            TrigonometricForm form = new TrigonometricForm(5, Math.PI / 2);

            double result = form.AngleDegrees;

            Assert.Equal(90, result, precision: 10);
        }

        [Fact]
        public void Constructor_WhenValuesAreProvided_ShouldSetProperties()
        {
            TrigonometricForm form = new TrigonometricForm(5, Math.PI / 4);

            Assert.Equal(5, form.Magnitude);
            Assert.Equal(Math.PI / 4, form.AngleRadians);
        }

        [Fact]
        public void ToString_WhenFormIsCreated_ShouldReturnFormattedTrigonometricForm()
        {
            TrigonometricForm form = new TrigonometricForm(5, Math.PI / 4);

            string result = form.ToString();

            Assert.Contains("5", result);
            Assert.Contains("cos", result);
            Assert.Contains("sin", result);
            Assert.Contains("45", result);
        }
    }
}
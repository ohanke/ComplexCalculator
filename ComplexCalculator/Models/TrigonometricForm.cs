namespace ComplexCalculator.Models
{
    /// <summary>
    /// Represents a complex number in trigonometric form.
    /// </summary>
    public class TrigonometricForm
    {
        /// <summary>
        /// Gets the magnitude of the complex number.
        /// </summary>
        public double Magnitude { get; }

        /// <summary>
        /// Gets the angle of the complex number in radians.
        /// </summary>
        public double AngleRadians { get; }

        /// <summary>
        /// Gets the angle of the complex number in degrees.
        /// </summary>
        public double AngleDegrees => AngleRadians * 180 / Math.PI;

        /// <summary>
        /// Initializes a new instance of the <see cref="TrigonometricForm"/> class.
        /// </summary>
        /// <param name="magnitude">The magnitude of the complex number.</param>
        /// <param name="angleRadians">The angle of the complex number in radians.</param>
        public TrigonometricForm(double magnitude, double angleRadians)
        {
            Magnitude = magnitude;
            AngleRadians = angleRadians;
        }

        /// <summary>
        /// Returns the trigonometric form as a formatted text value.
        /// </summary>
        /// <returns>
        /// The trigonometric form in the format r(cos φ + i sin φ).
        /// </returns>
        public override string ToString()
        {
            return $"{Magnitude:0.##}(cos {AngleDegrees:0.##}° + i sin {AngleDegrees:0.##}°)";
        }
    }
}
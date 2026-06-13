namespace ComplexCalculator.Persistence
{
    /// <summary>
    /// Represents one calculation result entry saved to a JSON file.
    /// </summary>
    public class CalculationResultFileEntry
    {
        /// <summary>
        /// Gets or sets the date and time when the calculation was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the type of the operation.
        /// For example: Binary or Unary.
        /// </summary>
        public string OperationType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the operation symbol or name.
        /// For example: +, -, *, /, |z₁|, conj(z₁), trig(z₁).
        /// </summary>
        public string Operation { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the first complex number used in the calculation.
        /// </summary>
        public string FirstNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the second complex number used in the calculation.
        /// Null for unary operations.
        /// </summary>
        public string? SecondNumber { get; set; }

        /// <summary>
        /// Gets or sets the formatted calculation result.
        /// </summary>
        public string Result { get; set; } = string.Empty;
    }
}
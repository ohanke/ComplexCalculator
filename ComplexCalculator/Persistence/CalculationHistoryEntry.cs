namespace ComplexCalculator.Persistence
{
    /// <summary>
    /// Represents one calculation history entry saved in the SQLite database.
    /// </summary>
    public class CalculationHistoryEntry
    {
        /// <summary>
        /// Gets or sets the database identifier of the history entry.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the real part of the first complex number.
        /// </summary>
        public double FirstReal { get; set; }

        /// <summary>
        /// Gets or sets the imaginary part of the first complex number.
        /// </summary>
        public double FirstImaginary { get; set; }

        /// <summary>
        /// Gets or sets the real part of the second complex number.
        /// </summary>
        public double SecondReal { get; set; }

        /// <summary>
        /// Gets or sets the imaginary part of the second complex number.
        /// </summary>
        public double SecondImaginary { get; set; }

        /// <summary>
        /// Gets or sets the symbol of the mathematical operation.
        /// For example: +, -, *, or /.
        /// </summary>
        public string OperationSymbol { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the real part of the calculation result.
        /// </summary>
        public double ResultReal { get; set; }

        /// <summary>
        /// Gets or sets the imaginary part of the calculation result.
        /// </summary>
        public double ResultImaginary { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the calculation was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
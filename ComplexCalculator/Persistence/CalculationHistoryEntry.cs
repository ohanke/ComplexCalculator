namespace ComplexCalculator.Persistence
{
    /// <summary>
    /// Represents one saved calculation history entry.
    /// </summary>
    public class CalculationHistoryEntry
    {
        public int Id { get; set; }

        public double FirstReal { get; set; }

        public double FirstImaginary { get; set; }

        public double SecondReal { get; set; }

        public double SecondImaginary { get; set; }

        public string OperationSymbol { get; set; } = string.Empty;

        public double ResultReal { get; set; }

        public double ResultImaginary { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
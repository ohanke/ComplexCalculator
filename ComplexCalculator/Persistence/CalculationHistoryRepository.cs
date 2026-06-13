using ComplexCalculator.Models;
using Microsoft.Data.Sqlite;

namespace ComplexCalculator.Persistence
{
    /// <summary>
    /// Provides methods for saving and reading calculation history entries from a SQLite database.
    /// </summary>
    public class CalculationHistoryRepository
    {
        private const string DefaultDatabaseFileName = "calculation_history.db";

        private readonly string _connectionString;

        public CalculationHistoryRepository(string databaseFileName = DefaultDatabaseFileName)
        {
            _connectionString = $"Data Source={databaseFileName}";
            InitializeDatabase();
        }

        /// <summary>
        /// Creates the calculation history table if it does not already exist.
        /// </summary>
        private void InitializeDatabase()
        {
            using SqliteConnection connection = new SqliteConnection(_connectionString);
            connection.Open();

            using SqliteCommand command = connection.CreateCommand();
            command.CommandText = """
                CREATE TABLE IF NOT EXISTS CalculationHistory (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    FirstReal REAL NOT NULL,
                    FirstImaginary REAL NOT NULL,
                    SecondReal REAL NOT NULL,
                    SecondImaginary REAL NOT NULL,
                    OperationSymbol TEXT NOT NULL,
                    ResultReal REAL NOT NULL,
                    ResultImaginary REAL NOT NULL,
                    CreatedAt TEXT NOT NULL
                );
                """;

            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Saves a calculation result in the database.
        /// </summary>
        /// <param name="first">The first complex number used in the calculation.</param>
        /// <param name="second">The second complex number used in the calculation.</param>
        /// <param name="operationSymbol">The operation symbol.</param>
        /// <param name="result">The calculated result.</param>
        public void Save(
            ComplexNumber first,
            ComplexNumber second,
            string operationSymbol,
            ComplexNumber result)
        {
            using SqliteConnection connection = new SqliteConnection(_connectionString);
            connection.Open();

            using SqliteCommand command = connection.CreateCommand();
            command.CommandText = """
                INSERT INTO CalculationHistory (
                    FirstReal,
                    FirstImaginary,
                    SecondReal,
                    SecondImaginary,
                    OperationSymbol,
                    ResultReal,
                    ResultImaginary,
                    CreatedAt
                )
                VALUES (
                    @firstReal,
                    @firstImaginary,
                    @secondReal,
                    @secondImaginary,
                    @operationSymbol,
                    @resultReal,
                    @resultImaginary,
                    @createdAt
                );
                """;

            command.Parameters.AddWithValue("@firstReal", first.Real);
            command.Parameters.AddWithValue("@firstImaginary", first.Imaginary);
            command.Parameters.AddWithValue("@secondReal", second.Real);
            command.Parameters.AddWithValue("@secondImaginary", second.Imaginary);
            command.Parameters.AddWithValue("@operationSymbol", operationSymbol);
            command.Parameters.AddWithValue("@resultReal", result.Real);
            command.Parameters.AddWithValue("@resultImaginary", result.Imaginary);
            command.Parameters.AddWithValue("@createdAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            command.ExecuteNonQuery();
        }
    }
}
using ComplexCalculator.Models;
using ComplexCalculator.Persistence;
using ComplexCalculator.Utils;
using Microsoft.Data.Sqlite;
using Xunit;

namespace ComplexCalculator.Tests.Persistence
{
    public class CalculationHistoryRepositoryTests
    {
        [Fact]
        public void Constructor_WhenRepositoryIsCreated_ShouldCreateDatabaseFile()
        {
            string databasePath = CreateTestDatabasePath();

            try
            {
                _ = new CalculationHistoryRepository(databasePath);

                Assert.True(File.Exists(databasePath));
            }
            finally
            {
                DeleteDatabaseFile(databasePath);
            }
        }

        [Fact]
        public void Constructor_WhenRepositoryIsCreated_ShouldCreateCalculationHistoryTable()
        {
            string databasePath = CreateTestDatabasePath();

            try
            {
                _ = new CalculationHistoryRepository(databasePath);

                using SqliteConnection connection = new SqliteConnection($"Data Source={databasePath}");
                connection.Open();

                using SqliteCommand command = connection.CreateCommand();
                command.CommandText = """
                    SELECT name
                    FROM sqlite_master
                    WHERE type = 'table'
                    AND name = 'CalculationHistory';
                    """;

                object? result = command.ExecuteScalar();

                Assert.Equal("CalculationHistory", result);
            }
            finally
            {
                DeleteDatabaseFile(databasePath);
            }
        }

        [Fact]
        public void Save_WhenCalculationIsSaved_ShouldInsertOneHistoryEntry()
        {
            string databasePath = CreateTestDatabasePath();

            try
            {
                CalculationHistoryRepository repository = new CalculationHistoryRepository(databasePath);

                ComplexNumber first = new ComplexNumber(3, 4);
                ComplexNumber second = new ComplexNumber(2, 1);
                ComplexNumber result = new ComplexNumber(5, 5);

                repository.Save(first, second, Constants.PlusSign, result);

                using SqliteConnection connection = new SqliteConnection($"Data Source={databasePath}");
                connection.Open();

                using SqliteCommand command = connection.CreateCommand();
                command.CommandText = """
                    SELECT COUNT(*)
                    FROM CalculationHistory;
                    """;

                long count = (long)command.ExecuteScalar()!;

                Assert.Equal(1, count);
            }
            finally
            {
                DeleteDatabaseFile(databasePath);
            }
        }

        [Fact]
        public void Save_WhenCalculationIsSaved_ShouldSaveCorrectValues()
        {
            string databasePath = CreateTestDatabasePath();

            try
            {
                CalculationHistoryRepository repository = new CalculationHistoryRepository(databasePath);

                ComplexNumber first = new ComplexNumber(3, 4);
                ComplexNumber second = new ComplexNumber(2, 1);
                ComplexNumber result = new ComplexNumber(5, 5);

                repository.Save(first, second, Constants.PlusSign, result);

                using SqliteConnection connection = new SqliteConnection($"Data Source={databasePath}");
                connection.Open();

                using SqliteCommand command = connection.CreateCommand();
                command.CommandText = """
                    SELECT
                        FirstReal,
                        FirstImaginary,
                        SecondReal,
                        SecondImaginary,
                        OperationSymbol,
                        ResultReal,
                        ResultImaginary
                    FROM CalculationHistory
                    LIMIT 1;
                    """;

                using SqliteDataReader reader = command.ExecuteReader();

                Assert.True(reader.Read());

                Assert.Equal(3, reader.GetDouble(0));
                Assert.Equal(4, reader.GetDouble(1));
                Assert.Equal(2, reader.GetDouble(2));
                Assert.Equal(1, reader.GetDouble(3));
                Assert.Equal(Constants.PlusSign, reader.GetString(4));
                Assert.Equal(5, reader.GetDouble(5));
                Assert.Equal(5, reader.GetDouble(6));
            }
            finally
            {
                DeleteDatabaseFile(databasePath);
            }
        }

        private static string CreateTestDatabasePath()
        {
            return Path.Combine(
                Path.GetTempPath(),
                $"complex_calculator_test_{Guid.NewGuid()}.db"
            );
        }

        private static void DeleteDatabaseFile(string databasePath)
        {
            SqliteConnection.ClearAllPools();

            if (File.Exists(databasePath))
            {
                File.Delete(databasePath);
            }
        }
    }
}
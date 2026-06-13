using ComplexCalculator.Persistence;
using Xunit;

namespace ComplexCalculator.Tests.Persistence
{
    public class CalculationResultFileServiceTests
    {
        [Fact]
        public void Save_WhenFileDoesNotExist_ShouldCreateJsonFile()
        {
            string filePath = CreateTestFilePath();

            try
            {
                CalculationResultFileService service = new CalculationResultFileService(filePath);
                CalculationResultFileEntry entry = CreateBinaryEntry();

                service.Save(entry);

                Assert.True(File.Exists(filePath));
            }
            finally
            {
                DeleteFile(filePath);
            }
        }

        [Fact]
        public void Save_WhenEntryIsSaved_ShouldSaveCorrectEntry()
        {
            string filePath = CreateTestFilePath();

            try
            {
                CalculationResultFileService service = new CalculationResultFileService(filePath);
                CalculationResultFileEntry entry = CreateBinaryEntry();

                service.Save(entry);

                List<CalculationResultFileEntry> entries = service.ReadEntries();

                Assert.Single(entries);

                CalculationResultFileEntry savedEntry = entries[0];

                Assert.Equal(entry.CreatedAt, savedEntry.CreatedAt);
                Assert.Equal("Binary", savedEntry.OperationType);
                Assert.Equal("+", savedEntry.Operation);
                Assert.Equal("1 + 2i", savedEntry.FirstNumber);
                Assert.Equal("3 + 4i", savedEntry.SecondNumber);
                Assert.Equal("4 + 6i", savedEntry.Result);
            }
            finally
            {
                DeleteFile(filePath);
            }
        }

        [Fact]
        public void Save_WhenFileAlreadyContainsEntry_ShouldAppendNewEntry()
        {
            string filePath = CreateTestFilePath();

            try
            {
                CalculationResultFileService service = new CalculationResultFileService(filePath);

                CalculationResultFileEntry firstEntry = CreateBinaryEntry();
                CalculationResultFileEntry secondEntry = new CalculationResultFileEntry
                {
                    CreatedAt = new DateTime(2026, 6, 13, 13, 5, 0),
                    OperationType = "Binary",
                    Operation = "-",
                    FirstNumber = "1 + 2i",
                    SecondNumber = "3 + 4i",
                    Result = "-2 - 2i"
                };

                service.Save(firstEntry);
                service.Save(secondEntry);

                List<CalculationResultFileEntry> entries = service.ReadEntries();

                Assert.Equal(2, entries.Count);

                Assert.Equal("+", entries[0].Operation);
                Assert.Equal("4 + 6i", entries[0].Result);

                Assert.Equal("-", entries[1].Operation);
                Assert.Equal("-2 - 2i", entries[1].Result);
            }
            finally
            {
                DeleteFile(filePath);
            }
        }

        [Fact]
        public void Save_WhenUnaryEntryIsSaved_ShouldSaveSecondNumberAsNull()
        {
            string filePath = CreateTestFilePath();

            try
            {
                CalculationResultFileService service = new CalculationResultFileService(filePath);

                CalculationResultFileEntry entry = new CalculationResultFileEntry
                {
                    CreatedAt = new DateTime(2026, 6, 13, 13, 10, 0),
                    OperationType = "Unary",
                    Operation = "|z₁|",
                    FirstNumber = "1 + 2i",
                    SecondNumber = null,
                    Result = "|z₁| = 2,24"
                };

                service.Save(entry);

                List<CalculationResultFileEntry> entries = service.ReadEntries();

                Assert.Single(entries);
                Assert.Equal("Unary", entries[0].OperationType);
                Assert.Equal("|z₁|", entries[0].Operation);
                Assert.Equal("1 + 2i", entries[0].FirstNumber);
                Assert.Null(entries[0].SecondNumber);
                Assert.Equal("|z₁| = 2,24", entries[0].Result);
            }
            finally
            {
                DeleteFile(filePath);
            }
        }

        [Fact]
        public void Save_WhenEntryIsNull_ShouldThrowArgumentNullException()
        {
            string filePath = CreateTestFilePath();

            try
            {
                CalculationResultFileService service = new CalculationResultFileService(filePath);

                Assert.Throws<ArgumentNullException>(() => service.Save(null!));
            }
            finally
            {
                DeleteFile(filePath);
            }
        }

        [Fact]
        public void ReadEntries_WhenFileDoesNotExist_ShouldReturnEmptyList()
        {
            string filePath = CreateTestFilePath();

            try
            {
                CalculationResultFileService service = new CalculationResultFileService(filePath);

                List<CalculationResultFileEntry> entries = service.ReadEntries();

                Assert.Empty(entries);
            }
            finally
            {
                DeleteFile(filePath);
            }
        }

        [Fact]
        public void Save_WhenEntryContainsSpecialCharacters_ShouldSaveReadableJson()
        {
            string filePath = CreateTestFilePath();

            try
            {
                CalculationResultFileService service = new CalculationResultFileService(filePath);

                CalculationResultFileEntry entry = new CalculationResultFileEntry
                {
                    CreatedAt = new DateTime(2026, 6, 13, 13, 15, 0),
                    OperationType = "Unary",
                    Operation = "trig(z₁)",
                    FirstNumber = "1 + 2i",
                    SecondNumber = null,
                    Result = "z₁ = 2,24(cos 63,43° + i sin 63,43°)"
                };

                service.Save(entry);

                string json = File.ReadAllText(filePath);

                Assert.Contains("trig(z₁)", json);
                Assert.Contains("1 + 2i", json);
                Assert.Contains("63,43°", json);
                Assert.DoesNotContain("\\u002B", json);
                Assert.DoesNotContain("\\u2081", json);
                Assert.DoesNotContain("\\u00B0", json);
            }
            finally
            {
                DeleteFile(filePath);
            }
        }

        private static CalculationResultFileEntry CreateBinaryEntry()
        {
            return new CalculationResultFileEntry
            {
                CreatedAt = new DateTime(2026, 6, 13, 13, 0, 0),
                OperationType = "Binary",
                Operation = "+",
                FirstNumber = "1 + 2i",
                SecondNumber = "3 + 4i",
                Result = "4 + 6i"
            };
        }

        private static string CreateTestFilePath()
        {
            return Path.Combine(
                Path.GetTempPath(),
                $"calculation_results_test_{Guid.NewGuid()}.json"
            );
        }

        private static void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
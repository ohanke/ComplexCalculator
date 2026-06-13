using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace ComplexCalculator.Persistence
{
    /// <summary>
    /// Provides methods for saving calculation results to a JSON file.
    /// </summary>
    public class CalculationResultFileService
    {
        private const string DefaultFileName = "calculation_results.json";

        private readonly string _filePath;

        private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="CalculationResultFileService"/> class.
        /// </summary>
        /// <param name="filePath">The path to the JSON file.</param>
        public CalculationResultFileService(string filePath = DefaultFileName)
        {
            _filePath = filePath;
        }

        /// <summary>
        /// Saves a calculation result entry to the JSON file.
        /// </summary>
        /// <param name="entry">The calculation result entry to save.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the entry is null.
        /// </exception>
        public void Save(CalculationResultFileEntry entry)
        {
            if (entry is null)
            {
                throw new ArgumentNullException(nameof(entry));
            }

            List<CalculationResultFileEntry> entries = ReadEntries();

            entries.Add(entry);

            SaveEntries(entries);
        }

        /// <summary>
        /// Reads all calculation result entries from the JSON file.
        /// </summary>
        /// <returns>
        /// The list of saved calculation result entries.
        /// </returns>
        public List<CalculationResultFileEntry> ReadEntries()
        {
            if (!File.Exists(_filePath))
            {
                return new List<CalculationResultFileEntry>();
            }

            string json = File.ReadAllText(_filePath);

            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<CalculationResultFileEntry>();
            }

            return JsonSerializer.Deserialize<List<CalculationResultFileEntry>>(
                json,
                _jsonSerializerOptions
            ) ?? new List<CalculationResultFileEntry>();
        }

        /// <summary>
        /// Saves all calculation result entries to the JSON file.
        /// </summary>
        /// <param name="entries">The calculation result entries to save.</param>
        private void SaveEntries(List<CalculationResultFileEntry> entries)
        {
            string? directoryPath = Path.GetDirectoryName(_filePath);

            if (!string.IsNullOrWhiteSpace(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string json = JsonSerializer.Serialize(entries, _jsonSerializerOptions);

            File.WriteAllText(_filePath, json);
        }
    }
}
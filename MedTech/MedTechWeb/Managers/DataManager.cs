using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;

namespace MedTechWeb.Managers
{
    public class DataManager
    {
        private readonly string _connectionString;
        private static readonly string LogSource = "MedTechWeb";

        public DataManager()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ConnectionString
                ?? throw new ConfigurationErrorsException("DefaultConnection connection string not found in configuration.");
        }

        /// <summary>
        /// Gets sample data from the database
        /// </summary>
        /// <returns>DataTable containing the data</returns>
        public DataTable GetData()
        {
            var dataTable = new DataTable();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // Sample query - can be replaced with actual database query
                    var query = @"
                        SELECT TOP 100
                            1 AS ID,
                            'Sample Data 1' AS Name,
                            GETDATE() AS CreatedDate,
                            'Active' AS Status
                        UNION ALL
                        SELECT 2, 'Sample Data 2', GETDATE(), 'Active'
                        UNION ALL
                        SELECT 3, 'Sample Data 3', GETDATE(), 'Inactive'";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.CommandTimeout = 300;
                        using (var adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                LogError($"SQL Error retrieving data: {ex.Message}", ex);
                throw new InvalidOperationException("Error retrieving data from database.", ex);
            }
            catch (Exception ex)
            {
                LogError($"Unexpected error retrieving data: {ex.Message}", ex);
                throw;
            }

            return dataTable;
        }

        /// <summary>
        /// Validates data before export
        /// </summary>
        /// <param name="dataTable">The DataTable to validate</param>
        /// <returns>True if data is valid</returns>
        public bool ValidateData(DataTable dataTable)
        {
            if (dataTable == null)
            {
                LogError("Data validation failed: DataTable is null");
                return false;
            }

            if (dataTable.Rows.Count == 0)
            {
                LogWarning("No data available for export");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Logs an error message
        /// </summary>
        private void LogError(string message, Exception ex = null)
        {
            try
            {
                if (!EventLog.SourceExists(LogSource))
                {
                    EventLog.CreateEventSource(LogSource, "Application");
                }

                using (var eventLog = new EventLog("Application"))
                {
                    eventLog.Source = LogSource;
                    var logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ERROR: {message}";
                    if (ex != null)
                    {
                        logMessage += $"\n{ex}";
                    }
                    eventLog.WriteEntry(logMessage, EventLogEntryType.Error);
                }
            }
            catch
            {
                // Silently fail if logging fails
            }
        }

        /// <summary>
        /// Logs a warning message
        /// </summary>
        private void LogWarning(string message)
        {
            try
            {
                if (!EventLog.SourceExists(LogSource))
                {
                    EventLog.CreateEventSource(LogSource, "Application");
                }

                using (var eventLog = new EventLog("Application"))
                {
                    eventLog.Source = LogSource;
                    var logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] WARNING: {message}";
                    eventLog.WriteEntry(logMessage, EventLogEntryType.Warning);
                }
            }
            catch
            {
                // Silently fail if logging fails
            }
        }
    }
}

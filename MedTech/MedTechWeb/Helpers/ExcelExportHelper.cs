using System;
using System.Data;
using System.IO;
using System.Text;

namespace MedTechWeb.Helpers
{
    public static class ExcelExportHelper
    {
        /// <summary>
        /// Exports a DataTable to an Excel file in XLSX format
        /// </summary>
        /// <param name="dataTable">The DataTable to export</param>
        /// <param name="fileName">The name of the file (without extension)</param>
        /// <returns>Byte array containing the Excel file data</returns>
        public static byte[] ExportToExcel(DataTable dataTable, string fileName)
        {
            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                throw new ArgumentException("DataTable cannot be null or empty.", nameof(dataTable));
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("File name cannot be null or empty.", nameof(fileName));
            }

            try
            {
                using (var workbook = new ClosedXML.Excel.XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Data");

                    // Add headers
                    for (int colIndex = 0; colIndex < dataTable.Columns.Count; colIndex++)
                    {
                        worksheet.Cell(1, colIndex + 1).Value = dataTable.Columns[colIndex].ColumnName;
                        worksheet.Cell(1, colIndex + 1).Style.Font.Bold = true;
                        worksheet.Cell(1, colIndex + 1).Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.LightGray;
                    }

                    // Add data rows
                    for (int rowIndex = 0; rowIndex < dataTable.Rows.Count; rowIndex++)
                    {
                        for (int colIndex = 0; colIndex < dataTable.Columns.Count; colIndex++)
                        {
                            var value = dataTable.Rows[rowIndex][colIndex];
                            worksheet.Cell(rowIndex + 2, colIndex + 1).Value = value ?? string.Empty;
                        }
                    }

                    // Auto-fit columns
                    worksheet.Columns().AdjustToContents();

                    using (var ms = new MemoryStream())
                    {
                        workbook.SaveAs(ms);
                        return ms.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error exporting data to Excel: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Generates a unique file name with timestamp
        /// </summary>
        /// <param name="baseName">The base name of the file</param>
        /// <returns>File name with timestamp</returns>
        public static string GenerateFileName(string baseName)
        {
            return $"{baseName}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
        }
    }
}

using System;
using System.Data;
using System.Text;
using System.Web.UI;
using MedTechWeb.Helpers;
using MedTechWeb.Managers;

namespace MedTechWeb.Pages
{
    public partial class ExportPage : Page
    {
        private const string ExportSessionKey = "ExportInProgress";
        private const string ErrorMessageKey = "ExportError";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Clear any previous session values
                Session[ExportSessionKey] = null;
                Session[ErrorMessageKey] = null;
            }
            else
            {
                // Check if there's a pending error from the export
                if (Session[ErrorMessageKey] != null)
                {
                    DisplayError(Session[ErrorMessageKey].ToString());
                    Session[ErrorMessageKey] = null;
                }
            }
        }

        protected void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                // Set session flag to indicate export is in progress
                Session[ExportSessionKey] = true;

                // Validate input
                if (!ValidateExportRequest())
                {
                    DisplayError("Некорректный запрос экспорта.");
                    return;
                }

                // Get data from database
                var dataManager = new DataManager();
                var dataTable = dataManager.GetData();

                // Validate data
                if (!dataManager.ValidateData(dataTable))
                {
                    DisplayError("Нет данных для экспорта.");
                    return;
                }

                // Export to Excel
                var excelData = ExcelExportHelper.ExportToExcel(dataTable, "МедТех_Экспорт");

                // Send file to browser
                SendFileToClient(excelData);

                // Display success message (this will be shown after file download completes)
                DisplaySuccess("Файл успешно экспортирован.");
            }
            catch (Exception ex)
            {
                LogException(ex);
                DisplayError($"Произошла ошибка при экспорте: {ex.Message}");
            }
            finally
            {
                // Clear session flag
                Session[ExportSessionKey] = null;
            }
        }

        /// <summary>
        /// Validates the export request
        /// </summary>
        private bool ValidateExportRequest()
        {
            // Check if request is coming from the same site (CSRF protection)
            if (Request.UrlReferrer == null || !Request.UrlReferrer.Host.Equals(Request.Url.Host, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            // Check if export is not already in progress
            if (Session[ExportSessionKey] != null && (bool)Session[ExportSessionKey])
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Sends the file data to the client
        /// </summary>
        private void SendFileToClient(byte[] fileData)
        {
            if (fileData == null || fileData.Length == 0)
            {
                throw new InvalidOperationException("File data is empty.");
            }

            // Generate unique file name
            var fileName = ExcelExportHelper.GenerateFileName("МедТех_Экспорт");

            // Clear any previous response
            Response.Clear();

            // Set response headers for file download
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("Content-Disposition", $"attachment; filename=\"{fileName}\"");
            Response.AddHeader("Content-Length", fileData.Length.ToString());

            // Set encoding to UTF-8
            Response.ContentEncoding = Encoding.UTF8;

            // Write file data to response
            Response.BinaryWrite(fileData);

            // End response
            Response.End();
        }

        /// <summary>
        /// Displays an error message to the user
        /// </summary>
        private void DisplayError(string message)
        {
            var alertHtml = $@"
                <div class=""alert alert-danger alert-dismissible fade in"" role=""alert"">
                    <button type=""button"" class=""close"" data-dismiss=""alert"" aria-label=""Close"">
                        <span aria-hidden=""true"">&times;</span>
                    </button>
                    <strong>Ошибка!</strong> {SecurityHelper.HtmlEncode(message)}
                </div>";

            litAlert.Text = alertHtml;
        }

        /// <summary>
        /// Displays a success message to the user
        /// </summary>
        private void DisplaySuccess(string message)
        {
            var alertHtml = $@"
                <div class=""alert alert-success alert-dismissible fade in"" role=""alert"">
                    <button type=""button"" class=""close"" data-dismiss=""alert"" aria-label=""Close"">
                        <span aria-hidden=""true"">&times;</span>
                    </button>
                    <strong>Успешно!</strong> {SecurityHelper.HtmlEncode(message)}
                </div>";

            litAlert.Text = alertHtml;
        }

        /// <summary>
        /// Logs an exception
        /// </summary>
        private void LogException(Exception ex)
        {
            try
            {
                System.Diagnostics.EventLog eventLog = new System.Diagnostics.EventLog("Application");
                eventLog.Source = "MedTechWeb";
                eventLog.WriteEntry($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Unexpected error in ExportPage: {ex.Message}\n{ex.StackTrace}", 
                    System.Diagnostics.EventLogEntryType.Error);
            }
            catch
            {
                // Silently fail if logging fails
            }
        }
    }
}

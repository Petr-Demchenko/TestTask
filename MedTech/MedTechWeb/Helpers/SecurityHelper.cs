using System;
using System.Web;

namespace MedTechWeb.Helpers
{
    public static class SecurityHelper
    {
        /// <summary>
        /// HTML-encodes a string to prevent XSS attacks
        /// </summary>
        /// <param name="text">The text to encode</param>
        /// <returns>HTML-encoded text</returns>
        public static string HtmlEncode(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            return HttpUtility.HtmlEncode(text);
        }

        /// <summary>
        /// Validates a file name to prevent directory traversal attacks
        /// </summary>
        /// <param name="fileName">The file name to validate</param>
        /// <returns>True if the file name is safe</returns>
        public static bool IsValidFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return false;
            }

            // Check for directory traversal attempts
            if (fileName.Contains("..") || fileName.Contains("/") || fileName.Contains("\\"))
            {
                return false;
            }

            // Check for invalid characters
            char[] invalidChars = System.IO.Path.GetInvalidFileNameChars();
            foreach (char c in invalidChars)
            {
                if (fileName.Contains(c.ToString()))
                {
                    return false;
                }
            }

            return true;
        }
    }
}

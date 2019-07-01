using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.FTP
{
    public static class FTPLogBuilder
    {
        //public StringBuilder Logs { get; set; }

        public static string  AppendLogLine(string message, string currentLogString)
        {
            string newLogMessage = DateTime.UtcNow.ToString("u") + " " + message;

            return currentLogString + Environment.NewLine + newLogMessage;
        }

        public static string BuildLogLine(string message)
        {
            string newLogMessage = DateTime.UtcNow.ToString("u") + " " + message;

            return newLogMessage;
        }

        public static string GetFileSizeString(long? fileBytes)
        {
            double Byte = 1024;
            string FileSize = "";
            double fileSizeDouble;
            if (fileBytes == null)
            {
                fileBytes = 0;
            }

            double.TryParse(fileBytes.Value.ToString(), out fileSizeDouble);
            if (fileBytes < Byte)
            {
                FileSize = string.Format("{0:0.00}", fileSizeDouble) + " B";
            }
            else if (fileBytes >= Byte && fileBytes < Byte * Byte)
            {
                double result = fileSizeDouble / Byte;
                FileSize = string.Format("{0:0.00}", result) + " KB";
            }
            else if (fileBytes >= Byte * Byte && fileBytes < Byte * Byte * Byte)
            {
                double result = fileSizeDouble / (Byte * Byte);
                FileSize = string.Format("{0:0.00}", result) + " MB";
            }
            else if (fileBytes >= Byte * Byte * Byte && fileBytes < Byte * Byte * Byte * Byte)
            {
                double result = fileSizeDouble / (Byte * Byte * Byte);
                FileSize = string.Format("{0:0.00}", result) + " GB";
            }

            return FileSize;
        }


        public static string GetFTPErrorFromFtpStatusCode(WebException ex, string host, string user, string remoteFile, string operation)
        {

            string errorMessage = "";
            var ftpResponse = ex.Response as FtpWebResponse;
            switch (ftpResponse.StatusCode)
            {
                case FtpStatusCode.NotLoggedIn:
                    errorMessage += "Failed to perform 'Logon' to Host:'" + host + "' ,User:'" + user + Environment.NewLine + ftpResponse.StatusDescription;
                    break;
                case FtpStatusCode.ActionNotTakenFileUnavailable:
                    errorMessage += "Failed to " + operation + " " + remoteFile + " file" + Environment.NewLine + ftpResponse.StatusDescription;
                    break;
                default:
                    errorMessage += "Failed to " + operation + " " + (!string.IsNullOrEmpty(remoteFile) ? remoteFile + " file" : "") + Environment.NewLine + ftpResponse.StatusDescription + Environment.NewLine + ex.Message;
                    break;
            }

            errorMessage = DateTime.Now.ToString() + " : " + errorMessage;

            return errorMessage;
        }

        public static string WildcardToRegex(string p_pattern)
        {
            try
            {
                if (string.IsNullOrEmpty(p_pattern)) p_pattern = "*";
                p_pattern = Wildcard.WildcardToRegex(p_pattern);
                return (p_pattern);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }


}

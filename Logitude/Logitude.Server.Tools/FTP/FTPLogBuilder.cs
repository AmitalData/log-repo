using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.FTP
{
    public class FTPLogBuilder
    {
        //public StringBuilder Logs { get; set; }

        public string AppendLogLine(string message, string currentLogString)
        {
            string newLogMessage = DateTime.UtcNow.ToString("u") + " " + message;

            return currentLogString + Environment.NewLine + newLogMessage;
        }

        public string BuildLogLine(string message)
        {
            string newLogMessage = DateTime.UtcNow.ToString("u") + " " + message;

            return newLogMessage;
        }

        public string GetFileSizeString(long? fileBytes)
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
    }


}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationWorkerRole.Services
{
    public abstract class FTPSchedulerTaskServiceBase
    {

        public List<string> WarningsList { get; set; }
        public List<string> MessagesList { get; set; }

        public int DownloadedFilesCount = 0;
        public int FailedFilesCount = 0;
        public FTPSchedulerTaskServiceBase()
        {
            this.WarningsList = new List<string>();
            this.MessagesList = new List<string>();
        }

        public void AddWarning(string warningMessage)
        {
            if (!string.IsNullOrEmpty(warningMessage) && !this.WarningsList.Contains(warningMessage))
            {
                this.WarningsList.Add(warningMessage);
            }
        }

        public void AddMessage(string message)
        {
            if (!string.IsNullOrEmpty(message) && !this.MessagesList.Contains(message))
            {
                this.MessagesList.Add(message);
            }
        }

        public string GetFilesDownloadingSummery()
        {
            string downloadingSummery = "Downloading files completed, " + this.DownloadedFilesCount + " files downloaded successfully, " + this.FailedFilesCount + " failed files";
            return downloadingSummery;
        }

        public void AddStatusMessage(string p_message, string p_status)
        {
            if (p_status == "-1")
            {
                AddWarning(p_message);
            }
            else
            {
                AddMessage(p_message);
            }
        }

    }
}

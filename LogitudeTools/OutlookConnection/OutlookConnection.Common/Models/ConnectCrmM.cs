using System;
using System.Collections.Generic;
using System.ComponentModel;



namespace OutlookConnection.Common.Models
{
    public enum OfficeType { Outlook, Word, Excel, PowerPoint, Appointment, Task, Email };

    public class ConnectCrmM
    {
        public bool IsEmail { get; set; }
        public bool IsWordExcel { get; set; }
        public string Body { get; set; }
        public string from { get; set; }
        public List<string> ToList { get; set; }
        public string Subject { get; set; }
        public string Extension { get; set; }
        public List<AttachM> Attaches { get; set; }
        public bool IsSend { get; set; }
        public string entryID { get; set; }
        public bool connectOperation { get; set; }
        public bool showInTask { get; set; }
        public IntPtr outlookHwnd { get; set; }
        public OfficeType OfficeItemType { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string LocationStatus { get; set; }

    }


   




}









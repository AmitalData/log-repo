using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Outlook;
using OutlookConnection.Common.Models;

namespace OutlookConnection.Common.Contracts
{
    public class ContactResult
    {
        public bool Success { get; set; }
        public String ErrorMessage { get; set; }
        //public string Contacts { get; set; }
        //public Recipients Recipients { get; set; }
        public List<AppointmentContactDetails> AppointmentContactDetailsList { get; set; }

    }
}

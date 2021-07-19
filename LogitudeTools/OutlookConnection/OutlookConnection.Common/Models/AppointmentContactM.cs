using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Outlook;

namespace OutlookConnection.Common.Models
{
    public class AppointmentContactM : IDisposable
    {
        public bool ShwIntaskBar { get; set; }
        public Recipients Recipients { get; set; }
        public string OptionalAttendees { get; set; }
        public string RequiredAttendees { get; set; }
        public List<AppointmentContactDetails> AppointmentContactDetails { get; set; }

        public AppointmentContactM()
        {

        }


        public void Dispose()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }



    }
    public class AppointmentContactDetails
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
    }



}
